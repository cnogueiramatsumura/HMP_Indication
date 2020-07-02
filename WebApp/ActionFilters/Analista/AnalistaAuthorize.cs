using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebApp.Helpers;

namespace WebApp.ActionFilters.Analista
{
    public class AnalistaAuthorize : AuthorizeAttribute
    {        
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var myroles = new MyRoleProvider().GetRolesForUser(filterContext.HttpContext.User.Identity.Name);
            // If they are authorized, handle accordingly
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated || !myroles.Contains("Analista"))
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { area = "Analista", controller = "Login", action = "index" }));
            }     
        }
    }
}