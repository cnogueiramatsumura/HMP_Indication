using DataAccess.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using WebApp.Areas.Usuario.Models.Register;
using WebApp.Helpers;

namespace WebApp.Areas.Usuario.Controllers
{
    public class RegisterController : MyBaseController
    {
        private readonly IServerConfigRepository _ServerConfigRepo;
        public RegisterController(IServerConfigRepository _ServerConfigRepository)
        {
            _ServerConfigRepo = _ServerConfigRepository;
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                HttpContent form = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string,string>("nome",model.nome),
                    new KeyValuePair<string,string>("sobrenome",model.sobrenome),
                    new KeyValuePair<string,string>("email",model.email),
                    new KeyValuePair<string,string>("password",model.password),
                    new KeyValuePair<string,string>("confirmPassword",model.confirmPassword)
                });
                var res = ApiUsuario.PostRegister(form);
                if (res.StatusCode == HttpStatusCode.OK || res.StatusCode == HttpStatusCode.Created)
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    var token = JsonConvert.DeserializeObject<dynamic>(result).token;
                    return RedirectToAction("ConfirmEmail", new { token = token });
                }
                else
                {
                    var result = res.Content.ReadAsStringAsync().Result;              
                    ModelState.AddModelError("error", result);
                    //aterar o retorno aqui para uma msg de erro avaliar
                    return View();
                }
            }
            return View(model);
        }

        public ActionResult ConfirmEmail(string token)
        {
            ViewBag.ApiDomainName = _ServerConfigRepo.GetApiServer();
            ViewBag.token = token;
            return View();
        }

        public ActionResult EmailConfirmado()
        {
            return View();
        }

        public ActionResult PerdeuSenha()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PerdeuSenha(string email)
        {
            HttpContent form = new FormUrlEncodedContent(new[]{
                new KeyValuePair<string,string>("Email",email)
            });
            var res = ApiUsuario.RecuperarSenha(form);
            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                return View("EmailPerdeuSenha");
            }
            else
            {
                var result = res.Content.ReadAsStringAsync().Result;
                ModelState.AddModelError("error", result);
            }
            return View();
        }

        public ActionResult TokenExpirado()
        {
            return View();
        }
    }
}