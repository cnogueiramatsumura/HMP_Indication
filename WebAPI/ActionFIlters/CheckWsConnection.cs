using DataAccess.Interfaces;
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
    public class CheckWsConnection : Attribute, IActionFilter
    {
        public bool AllowMultiple { get; }

        public Task<HttpResponseMessage> ExecuteActionFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            var userId = int.Parse(Helper.GetJWTPayloadValue(actionContext.Request, "id"));
            using (var _UserRepo = new UsuarioRepository())
            {
                var user = _UserRepo.GetById(userId);
                var monitor = WSMonitor.Instancia;
                monitor.AddMonitor(user);
            }
            return continuation();
        }
    }
}