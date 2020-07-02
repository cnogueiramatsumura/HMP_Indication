using DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Helpers;

namespace WebApp.ActionFilters
{
    public class LoadViewBags : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            using (var _UserRepo = new UsuarioRepository())
            {

                var Roles = new MyRoleProvider().GetRolesForUser(filterContext.HttpContext.User.Identity.Name);
                if (Roles.Contains("Users"))
                {
                    var user = _UserRepo.GetByEmail(filterContext.HttpContext.User.Identity.Name);
                    filterContext.Controller.ViewBag.datavencimento = user.DataVencimentoLicenca.ToString("dd/MM/yyyy");
                    filterContext.Controller.ViewBag.username = user.Nome;
                }
            }
        }
    }
}