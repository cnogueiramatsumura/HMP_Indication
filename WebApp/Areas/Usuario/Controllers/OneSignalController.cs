using DataAccess.Entidades;
using DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WebApp.ActionFilters;
using WebApp.Helpers;

namespace WebApp.Areas.Usuario.Controllers
{
    [UsuarioAuthorize]
    public class OneSignalController : MyBaseController
    {

        [HttpPost]
        public JsonResult Subscrible(string onesignalId)
        {
            try
            {
                var token = Session["token"];
                HttpContent form = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string,string>("",onesignalId)
                });
                var res = ApiUsuario.SubscriptionOneSignal(form, token.ToString());
                if (res.StatusCode == HttpStatusCode.OK)
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {                    
                    Response.TrySkipIisCustomErrors = true;
                    Response.StatusCode = (int)res.StatusCode;
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }        
        }

        [HttpPost]
        public JsonResult UnSubscrible(string onesignalId)
        {
            try
            {
                var token = Session["token"];
                HttpContent form = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string,string>("",onesignalId)
                });
                var res = ApiUsuario.UnSubscriptionOneSignal(form, token.ToString());
                if (res.StatusCode == HttpStatusCode.OK)
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Response.TrySkipIisCustomErrors = true;
                    Response.StatusCode = (int)res.StatusCode;
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
    }
}