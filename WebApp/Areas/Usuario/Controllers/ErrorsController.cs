using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.ActionFilters;

namespace WebApp.Areas.Usuario.Controllers
{
    public class ErrorsController : MyBaseController
    {       
        [LoadViewBags]
        public ActionResult BinanceApiError()
        {
            return View();
        }
    }
}