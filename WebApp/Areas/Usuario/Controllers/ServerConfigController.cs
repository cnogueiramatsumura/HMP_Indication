using DataAccess.Entidades;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Helpers;

namespace WebApp.Areas.Usuario.Controllers
{
    public class ServerConfigController : Controller
    {
        public MyJsonResult Index()
        {
            try
            {
                var token = Session["token"];
                var res = ApiUsuario.ServerConfig(token.ToString());
                if (res.IsSuccessStatusCode)
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    var ret = JsonConvert.DeserializeObject<ServerConfig>(result);
                    return new MyJsonResult(ret, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    Response.TrySkipIisCustomErrors = true;
                    Response.StatusCode = (int)res.StatusCode;
                    return new MyJsonResult(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Response.TrySkipIisCustomErrors = true;
                Response.StatusCode = 500;
                return new MyJsonResult(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
    }
}