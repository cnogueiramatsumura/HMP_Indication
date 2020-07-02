using DataAccess.Helpers;
using DataAccess.Interfaces;
using DataAccess.Serialized_Objects;
using DataAccess.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApp.ActionFilters;
using WebApp.Helpers;

namespace WebApp.Areas.Usuario.Controllers
{
    [UsuarioAuthorize]
    public class BinanceController : MyBaseController
    {
        private readonly IUsuarioRepository _UserRepo;
        public BinanceController(IUsuarioRepository UserRepository)
        {
            _UserRepo = UserRepository;
        }

        [LoadViewBags]
        public ActionResult setkeys()
        {
            var user = _UserRepo.GetByEmail(User.Identity.Name);
            ViewBag.datavencimento = user.DataVencimentoLicenca.ToString("dd/MM/yyyy");
            ViewBag.username = user.Nome;
            return View();
        }

        [HttpPost]
        public ActionResult setkeys(SetKeysViewModel ViewModel)
        {
            if (ModelState.IsValid)
            {
                var token = Session["token"].ToString();
                HttpContent form = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string,string>("binanceKey",ViewModel.binanceKey),
                    new KeyValuePair<string,string>("binanceSecret",ViewModel.binanceSecret)
                });
                var res = ApiUsuario.BinanceKeys(form, token);
                if (res.IsSuccessStatusCode)
                {
                    return RedirectToAction("index", "Dashboard");
                }
                else
                {
                    ModelState.AddModelError("error", "Chaves Inválidas tente novamente");
                }
            }
            return View(ViewModel);
        }

        [HttpGet]
        public MyJsonResult TestKey(SetKeysViewModel ViewModel)
        {
            var token = Session["token"].ToString();
            HttpContent form = new FormUrlEncodedContent(new[]
            {
                    new KeyValuePair<string,string>("binanceKey",ViewModel.binanceKey),
                    new KeyValuePair<string,string>("binanceSecret",ViewModel.binanceSecret)
                });
            var res = ApiUsuario.BinanceTestKeys(form, token);
            if (res.IsSuccessStatusCode)
            {
                return new MyJsonResult(true, JsonRequestBehavior.AllowGet);
            }

            var resObj = res.Content.ReadAsStringAsync().Result;
            Response.TrySkipIisCustomErrors = true;
            Response.StatusCode = (int)res.StatusCode;
            return new MyJsonResult(resObj, JsonRequestBehavior.AllowGet);

        }
    }
}