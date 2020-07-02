using DataAccess.Entidades;
using DataAccess.Interfaces;
using DataAccess.ViewModels.Analista;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web;
using System.Web.Http;
using WebApi.OutputCache.V2;
using WebAPI.Helpers;

namespace WebAPI.Controllers
{
    public class ServerConfigController : ApiController
    {
        private readonly IServerConfigRepository _ServerConfigRepo;
        public ServerConfigController(IServerConfigRepository ServerConfigRepository)
        {
            _ServerConfigRepo = ServerConfigRepository;
        }

        [CacheOutput(ServerTimeSpan = 3600)]
        public HttpResponseMessage Get()
        {
            try
            {
                var ServerConfig = _ServerConfigRepo.GetAllConfig();
                return Request.CreateResponse(HttpStatusCode.OK, ServerConfig);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public HttpResponseMessage EmailTrocaSMTP(UpdateSmtpViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var envioEmail = new EnvioEmail(_ServerConfigRepo);
                    var domainName = Request.RequestUri.Scheme + "://" + Request.RequestUri.Host;
                    envioEmail.EmailTrocaSMTP(domainName, model.SmtpAdress, model.SmtpPort, model.SmtpUsername, model.SmtpPassword);
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    var errorObj = ModelStateErrors.DisplayModelStateError(ModelState);
                    return Request.CreateResponse(HttpStatusCode.NotAcceptable, errorObj, "text/plain");
                }
            }
            catch (SmtpException ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Não foi possível authenticar este email, verifique sua senha e as configurações.");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}