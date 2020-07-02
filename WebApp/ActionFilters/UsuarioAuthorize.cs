using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using WebApp.Helpers;

namespace WebApp.ActionFilters
{
    public class UsuarioAuthorize : AuthorizeAttribute
    {      
        public override void OnAuthorization(AuthorizationContext filterContext)
        {            
            var myroles = new MyRoleProvider().GetRolesForUser(filterContext.HttpContext.User.Identity.Name);
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated || !myroles.Contains("Users"))
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
                    filterContext.HttpContext.Response.StatusDescription = "Tempo de Authenticaçao expirada";
                    filterContext.HttpContext.Response.SuppressFormsAuthenticationRedirect = true;
                    filterContext.Result = new JsonResult
                    {
                        Data = new
                        {
                            Error = "Not Authorized",
                            LogOnUrl = "/usuario/login"
                        },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
                else
                {
                    FormsAuthentication.SignOut();
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { area = "Usuario", controller = "Login", action = "index" }));
                }
            }
            else
            {
                var token = filterContext.HttpContext.Session["token"];
                if (token == null)
                {
                    FormsAuthentication.SignOut();
                    if (filterContext.HttpContext.Request.IsAjaxRequest())
                    {
                        filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
                        filterContext.HttpContext.Response.StatusDescription = "Tempo de Authenticaçao expirada";
                        filterContext.HttpContext.Response.SuppressFormsAuthenticationRedirect = true;
                        filterContext.Result = new JsonResult
                        {
                            Data = new
                            {
                                Error = "Not Authorized",
                                LogOnUrl = "/usuario/login"
                            },
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet
                        };
                    }
                    else
                    {
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { area = "Usuario", controller = "Login", action = "index" }));
                    }
                }
                else if (token != null)
                {
                    var expDate = int.Parse(Helper.GetJWTPayloadValue(token.ToString(), "exp"));
                    int unixTimestampNow = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    if (expDate < unixTimestampNow)
                    {
                        FormsAuthentication.SignOut();
                        if (filterContext.HttpContext.Request.IsAjaxRequest())
                        {
                            filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
                            filterContext.HttpContext.Response.StatusDescription = "Tempo de Authenticaçao expirada";
                            filterContext.HttpContext.Response.SuppressFormsAuthenticationRedirect = true;
                            filterContext.Result = new JsonResult
                            {
                                Data = new
                                {
                                    Error = "Not Authorized",
                                    LogOnUrl = "/usuario/login"
                                },
                                JsonRequestBehavior = JsonRequestBehavior.AllowGet
                            };
                        }
                        else
                        {
                            FormsAuthentication.SignOut();
                            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { area = "Usuario", controller = "Login", action = "index" }));
                        }
                    }
                }
            }
        }
    }
}