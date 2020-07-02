using DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebAPI.Helpers;

namespace WebAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);          
            Helper.ReconectarAtivosDesconectados();
            Helper.ReconectarClientesComOrdensAbertas();
            Helper.CreateLogFolder();         
        }

        protected void Application_End()
        {
            //DataAccess.Helpers.Logs.LogIIS(DateTime.UtcNow + "Aqui reiniciou o Web Api pool");
            //DataAccess.Helpers.Logs.LogIIS("O codigo do Motivo => " + (int)System.Web.Hosting.HostingEnvironment.ShutdownReason);
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
        
        }
    }
}
