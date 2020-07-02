using DataAccess.Helpers;
using DataAccess.Interfaces;
using DataAccess.Serialized_Objects;
using DataAccess.ViewModels;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;
using WebApp.ActionFilters;
using WebApp.Helpers;

namespace WebApp.Areas.Usuario.Controllers
{
    public class AlterarSenhaController : Controller
    {
        [LoadViewBags]     
        [UsuarioAuthorize]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]       
        [LoadViewBags]
        [UsuarioAuthorize]
        public ActionResult Index(AlterarSenhaViewModel model)
        {
            if (ModelState.IsValid)
            {
                var token = Session["token"];
                HttpContent form = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string,string>("SenhaAntiga",model.SenhaAntiga),
                    new KeyValuePair<string,string>("NovaSenha",model.NovaSenha),
                    new KeyValuePair<string,string>("ConfirmarSenha",model.ConfirmarSenha)
                });
                var res = ApiUsuario.MudarSenha(form, token.ToString());
                if (res.IsSuccessStatusCode)
                {
                    //avaliar se vale a pena deslogar o usuario
                    return View("MudarSenha");
                }
                var result = res.Content.ReadAsStringAsync().Result;
                ModelState.AddModelError("error", result);
            }
            return View(model);
        }
    }
}