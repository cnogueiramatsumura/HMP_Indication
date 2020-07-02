using DataAccess.Helpers;
using DataAccess.Repository;
using DataAccess.Serialized_Objects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebApp.ActionFilters
{
    public class GetLimitBTC : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            using (var _UserRepo = new UsuarioRepository())
            {
                var user = _UserRepo.GetByEmail(filterContext.HttpContext.User.Identity.Name);
                if (!string.IsNullOrEmpty(user.BinanceAPIKey) && !string.IsNullOrWhiteSpace(user.BinanceAPISecret) && user.IsValidBinanceKeys)
                {
                    var apicontent = BinanceRestApi.GetAccountInformation(user.BinanceAPIKey, user.BinanceAPISecret);
                    if (apicontent.IsSuccessStatusCode)
                    {
                        var result = apicontent.Content.ReadAsStringAsync().Result;
                        var AccounInformation = JsonConvert.DeserializeObject<Account_Information>(result);
                        filterContext.Controller.ViewBag.BTC = AccounInformation.balances.Where(x => x.asset == "BTC").Select(x => x.free).FirstOrDefault();
                        filterContext.Controller.ViewBag.datavencimento = user.DataVencimentoLicenca.ToString("dd/MM/yyyy");
                        filterContext.Controller.ViewBag.username = user.Nome;                        
                    }
                    else
                    {
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { area = "usuario", controller = "Errors", action = "BinanceApiError" }));
                    }
                }
                else
                {                    
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { area = "usuario", controller = "binance", action = "setkeys" }));
                }
            }
        }
    }
}