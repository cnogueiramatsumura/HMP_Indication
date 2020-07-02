using System.Web.Mvc;

namespace WebApp.Areas.Analista
{
    public class AnalistaAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Analista";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Analista_default",
                "Analista/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
                
            );
        }
    }
}