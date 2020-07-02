using DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebApp.Areas.Analista.Models;
using WebApp.Helpers;

namespace WebApp.Areas.Analista.Controllers
{
    public class LoginController : Controller
    {
        private readonly IAnalistaRepository _AnaRepo;
        public LoginController(IAnalistaRepository AnalistaRepository)
        {
            _AnaRepo = AnalistaRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var Analista = _AnaRepo.GetByEmail(model.email);
                if (Analista == null)
                {
                    ModelState.AddModelError("error", "Usuario Inexistente");
                }
                else
                {
                    if (Hashing.ValidatePassword(model.password, Analista.Password))
                    {
                        FormsAuthentication.SetAuthCookie(model.email, false);
                        var MyIdentity = new GenericIdentity(model.email);
                        var roles = new string[] { "Analista" };
                        var MyPrincipal = new GenericPrincipal(MyIdentity, roles);
                        Thread.CurrentPrincipal = MyPrincipal;
                        return RedirectToAction("index", "dashboard", new { area = "Analista" });
                    }
                    ModelState.AddModelError("error", "Senha Inválida");
                }
            }
            return View(model);
        }

        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index","Login");
        }
    }
}