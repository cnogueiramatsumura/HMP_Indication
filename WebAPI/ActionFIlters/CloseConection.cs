using DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using WebAPI.Helpers;

namespace WebAPI.ActionFIlters
{
    public class CloseConection : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            Task.Delay(1000);
            var userId = int.Parse(Helper.GetJWTPayloadValue(actionExecutedContext.Request, "id"));
            using (var _OrdemRepo = new OrdemRepository())
            {
                //var user = _UserRepo.GetById(userId);
                var ordensAbertas = _OrdemRepo.OrdemsAbertas(userId);
                if (ordensAbertas.Count == 0)
                {
                    var monitor = WSMonitor.Instancia;
                    monitor.RemoveMonitor(userId);
                }
            }
        }
    }
}