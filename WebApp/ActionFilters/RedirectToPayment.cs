using DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebApp.ActionFilters
{
    public class RedirectToActionPayment : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                using (var _UserRepo = new UsuarioRepository())
                {
                    var user = _UserRepo.GetByEmail(filterContext.HttpContext.User.Identity.Name);
                    if (!user.EmailConfirmado)
                    {
                        using (var _ConfirmEmailRepo = new ConfirmEmailRepository())
                        {
                            var _ConfirmEmail = _ConfirmEmailRepo.GetByUserID(user.Id);
                            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { area = "usuario", controller = "Register", action = "ConfirmEmail", token = _ConfirmEmail.Token }));
                        }
                    }
                    else if (string.IsNullOrEmpty(user.BinanceAPIKey) || string.IsNullOrEmpty(user.BinanceAPISecret) || !user.IsValidBinanceKeys)
                    {
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { area = "usuario", controller = "binance", action = "setkeys" }));
                    }
                    else if (user.DataVencimentoLicenca < DateTime.UtcNow)
                    {
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { area = "usuario", controller = "Pagamentos", action = "Index" }));
                    }
                }
            }
        }
    }
}