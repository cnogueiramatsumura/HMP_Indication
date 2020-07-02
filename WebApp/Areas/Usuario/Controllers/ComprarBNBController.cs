using DataAccess.Serialized_Objects;
using DataAccess.ViewModels;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Mvc;
using WebApp.ActionFilters;

using WebApp.Helpers;

namespace WebApp.Areas.Usuario.Controllers
{
    [UsuarioAuthorize]
    public class ComprarBNBController : MyBaseController
    {
        [GetLimitBTC]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [GetLimitBTC]
        public ActionResult Index(ComprarBNBViewModel model)
        {
            if (ModelState.IsValid)
            {
                var token = Session["token"];
                HttpContent form = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string,string>("qtd",model.qtd.ToString())
                });
                var binanceres = ApiUsuario.ComprarBNB(form, token.ToString());
                if (binanceres.IsSuccessStatusCode)
                {
                    return View("CompraBNBSuccesso");
                }
                else
                {
                    var result = binanceres.Content.ReadAsStringAsync().Result;
                    var Error = JsonConvert.DeserializeObject<BinanceErrors>(result);
                    if (Error.msg == "Account has insufficient balance for requested action.")
                    {
                        ModelState.AddModelError("error", "Saldo Insuficiente");
                    }
                    else
                    {
                        ModelState.AddModelError("error", Error.msg);
                    }
                }
            }
            return View(model);
        }
    }
}