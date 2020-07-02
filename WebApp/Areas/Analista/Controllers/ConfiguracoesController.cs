using DataAccess.Entidades;
using DataAccess.Helpers;
using DataAccess.Interfaces;
using DataAccess.ViewModels.Analista;
using Microsoft.AspNet.SignalR.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Compilation;
using System.Web.Mvc;
using WebApp.ActionFilters.Analista;
using WebApp.Areas.Analista.Models;
using WebApp.Helpers;

namespace WebApp.Areas.Analista.Controllers
{
    [AnalistaAuthorize(Roles = "Analista")]
    public class ConfiguracoesController : Controller
    {
        private readonly IServerConfigRepository _serverConfigRepo;
        public ConfiguracoesController(IServerConfigRepository serverConfigRepository)
        {
            _serverConfigRepo = serverConfigRepository;
        }

        public ActionResult SMTP()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SMTP(UpdateSmtpViewModel model)
        {
            if (ModelState.IsValid)
            {
                HttpContent form = new FormUrlEncodedContent(new[] {
                    new KeyValuePair<string,string>("SmtpAdress",model.SmtpAdress),
                    new KeyValuePair<string,string>("SmtpPort",model.SmtpPort.ToString()),
                    new KeyValuePair<string,string>("SmtpUsername",model.SmtpUsername),
                    new KeyValuePair<string,string>("SmtpPassword",model.SmtpPassword)
                });
                var res = ApiAnalista.EmailTrocaSMTP(form);
                if (res.IsSuccessStatusCode)
                {
                    Session.Add("trocaSMTP", model);
                    return View(model);
                }
                else
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    ModelState.AddModelError("error", result);
                    return View(model);
                }
            }
            else
            {
                return View(model);
            }
        }

        [AllowAnonymous]
        public ActionResult ConfirmaTroca()
        {
            var conf = Session["trocaSMTP"] as UpdateSmtpViewModel;
            if (conf != null)
            {
                var cryptograph = new AESEncription();
                ViewBag.novoemail = conf.SmtpUsername;
                var config = _serverConfigRepo.GetAllConfig();
                config.SmtpAdress = conf.SmtpAdress;
                config.SmtpPort = conf.SmtpPort;
                config.SmtpUsername = conf.SmtpUsername;
                config.SmtpPassword = cryptograph.EncryptMensage(conf.SmtpPassword);
                _serverConfigRepo.Update(config);
                Session.Remove("trocaSMTP");
            }
            return View();
        }

        [HttpPost]
        public MyJsonResult ResetSMTP()
        {
            try
            {
                var config = _serverConfigRepo.GetAllConfig();
                var cryptograph = new AESEncription();
                config.SmtpAdress = "smtp.mail.yahoo.com";
                config.SmtpPort = 587;
                config.SmtpUsername = "ccidhighwind@yahoo.com.br";
                config.SmtpPassword = cryptograph.EncryptMensage("epncrejjztlleysk");
                _serverConfigRepo.Update(config);
                return new MyJsonResult(true, JsonRequestBehavior.DenyGet);
            }
            catch
            {
                Response.TrySkipIisCustomErrors = true;
                Response.StatusCode = 500;
                return new MyJsonResult(false, JsonRequestBehavior.DenyGet);
            }
        }

        public ActionResult Licenca()
        {
            var precolicenca = _serverConfigRepo.GetLicencePrice();
            var model = new UpdateLicencaViewModel
            {
                PrecoLicenca = precolicenca
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Licenca(UpdateLicencaViewModel model)
        {
            if (ModelState.IsValid)
            {
                var config = _serverConfigRepo.GetAllConfig();
                config.PrecoLicenca = model.PrecoLicenca;
                _serverConfigRepo.Update(config);
            }
            return View(model);
        }
    }
}