using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Areas.Usuario.Controllers
{
    public class MyBaseController : Controller
    {
        protected override void OnException(ExceptionContext filterContext)
        {

            var controller = filterContext.Controller as Controller;
            controller.Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
            controller.Response.TrySkipIisCustomErrors = true;
            filterContext.ExceptionHandled = true;

            var controllerName = (string)filterContext.RouteData.Values["controller"];
            var actionName = (string)filterContext.RouteData.Values["action"];
            var exception = filterContext.Exception;
            //need a model to pass exception data to error view
            var model = new HandleErrorInfo(exception, controllerName, actionName);
            var view = new ViewResult();
            view.ViewName = "Error";
            view.ViewData = new ViewDataDictionary();
            view.ViewData.Model = model;
            //copy any view data from original control over to error view so they can be accessible in view.       
            var viewData = controller.ViewData;
            if (viewData != null && viewData.Count > 0)
            {
                viewData.ToList().ForEach(view.ViewData.Add);
            }
            //Obs:
            // filterContext.Result = view;  => Redireciona o Usuario para a pagina de erro 
            //view.ExecuteResult(filterContext); => Faz com que as duas paginas sejam renderizadas tanto a URL solicitada quanto a pagina de erro
            filterContext.Result = view;
            //view.ExecuteResult(filterContext);
        }
    }
}