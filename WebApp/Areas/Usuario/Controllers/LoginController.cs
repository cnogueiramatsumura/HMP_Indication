using DataAccess.Interfaces;
using DataAccess.ViewModels;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebApp.ActionFilters;
using WebApp.Helpers;

namespace WebApp.Areas.Usuario.Controllers
{

    public class LoginController : MyBaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var res = ApiUsuario.GetToken(model.email, model.password);
                var result = res.Content.ReadAsStringAsync().Result;
                if (res.IsSuccessStatusCode)
                {
                    var token = Newtonsoft.Json.JsonConvert.DeserializeObject<string>(result);
                    Session.Add("token", token);
                    FormsAuthentication.SetAuthCookie(model.email, false);
                    var MyIdentity = new GenericIdentity(model.email);
                    var roles = new string[] { "Users" };
                    var MyPrincipal = new GenericPrincipal(MyIdentity, roles);
                    Thread.CurrentPrincipal = MyPrincipal;
                    return RedirectToAction("index", "Dashboard");
                }
                else
                {
                    ModelState.AddModelError("error", result);
                    return View(model);
                }
            }
            return View(model);
        }  

        public ActionResult Logoff()
        {
            var token = Session["token"];
            if (token != null)
            {
                var res = ApiUsuario.Logoff(token.ToString());
            }

            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }

    }
}