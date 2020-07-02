using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebApp.ActionFilters;
using WebApp.Helpers;

namespace WebApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {            
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);         
        }

        protected void Application_End()
        {
            //DataAccess.Helpers.Logs.LogIIS(DateTime.UtcNow + "Aqui reiniciou o Web App pool");
            //DataAccess.Helpers.Logs.LogIIS("O codigo do Motivo => " + (int)System.Web.Hosting.HostingEnvironment.ShutdownReason);
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            //se nao for conexao https e nao for requisição local redireciona pra mesma pagina utilizando https
            if (!HttpContext.Current.Request.IsSecureConnection && !HttpContext.Current.Request.IsLocal)
            {
                Response.Redirect("https://" + Request.Url.Authority + HttpContext.Current.Request.RawUrl);
            }
        }
    }
}
