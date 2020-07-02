using DataAccess.Entidades;
using DataAccess.Helpers;
using DataAccess.Interfaces;
using DataAccess.ViewModels;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Security;
using WebAPI.Helpers;
using WebAPI.Helpers.JWT;
using WebAPI.Models;
using WebAPI.SignalR;

namespace WebAPI.Controllers
{
    public class LoginController : ApiController
    {
        private readonly IUsuarioRepository _UserRepo;
        private readonly IConfirmEmailRepository _confirEmailRepo;
        private readonly IOrdemRepository _OrdemRepo;
        private readonly IHubContext _signalContext;
        private readonly IServerConfigRepository _ServerConfigRepo;

        public LoginController(IUsuarioRepository userRepo, IConfirmEmailRepository confirEmailRepo, IOrdemRepository OrdemRepo, IServerConfigRepository _ServerConfigRepository,IHubContext HubContext)
        {
            _UserRepo = userRepo;
            _confirEmailRepo = confirEmailRepo;
            _OrdemRepo = OrdemRepo;
            _ServerConfigRepo = _ServerConfigRepository;
            _signalContext = HubContext;
        }

        public HttpResponseMessage Post([FromBody]LoginViewModel ViewModel)
        {
            try
            {         
                if(ViewModel == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotAcceptable, "Campos Inválidos", "text/plain");
                }
                if (ModelState.IsValid)
                {
                    var user = _UserRepo.GetByEmail(ViewModel.email);
                    if (user == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.Unauthorized, "Usuario Inexistente", "text/plain");
                    }
                    else
                    {                       
                        if (Hashing.ValidatePassword(ViewModel.password, user.Password))
                        {
                            Helper.AtualizarOrdens(user);
                            AuthenticationModule authentication = new AuthenticationModule();
                            string token = authentication.CreateToken(user.Id, user.Email);
                            //adiciona monitoramento das das ordems e saldos do usuario
                            if (user.BinanceAPIKey != null && user.BinanceAPISecret != null && user.IsValidBinanceKeys)
                            {
                                WSMonitor monitor = WSMonitor.Instancia;
                                monitor.AddMonitor(user);
                                //monitor.RemoveDoubleConnection(user.Id);
                            }
                            return Request.CreateResponse(HttpStatusCode.OK, token);
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.Unauthorized, "Senha Inválida", "text/plain");
                        }
                    }
                }
                var errorObj = ModelStateErrors.DisplayModelStateError(ModelState);                
                return Request.CreateResponse(HttpStatusCode.NotAcceptable, errorObj, "text/plain");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [System.Web.Http.Authorize]
        [HttpDelete]
        public HttpResponseMessage Delete()
        {
            try
            {
                var userid = int.Parse(Helper.GetJWTPayloadValue(Request, "id"));
                var user = _UserRepo.GetById(userid);
                var ordensAbertas = _OrdemRepo.OrdemsAbertas(userid);
                if (ordensAbertas != null && ordensAbertas.Count == 0)
                {
                    var wsmonitor = WSMonitor.Instancia;
                    wsmonitor.RemoveMonitor(userid);
                    if (user.IsValidBinanceKeys)
                    {
                        BinanceUserDataStream.CloseStream(user.BinanceAPIKey, user.BinanceAPISecret);
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("api/Login/ConfirmaEmail/")]
        public HttpResponseMessage ConfirmaEmail(string token)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var confirmEmail = _confirEmailRepo.GetByToken(token);
                    if (confirmEmail != null)
                    {
                        var user = _UserRepo.GetById(confirmEmail.Usuario_Id);
                        user.EmailConfirmado = true;
                        _UserRepo.Update(user);                 
                        
                        _signalContext.Clients.User("anonimo").EmailConfirmado(token);

                        var response = Request.CreateResponse(HttpStatusCode.Moved);
                        var domainName = Request.RequestUri.Scheme + "://" + Request.RequestUri.Host;
                        response.Headers.Location = new Uri(domainName + "/usuario/Register/EmailConfirmado");
                        return response;
                    }
                }
                var errorObj = ModelStateErrors.DisplayModelStateError(ModelState);
                return Request.CreateResponse(HttpStatusCode.NotAcceptable, errorObj, "text/plain");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("api/Login/ReenviarEmail")]
        public HttpResponseMessage ReenviarEmail(string token)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var confirmEmail = _confirEmailRepo.GetByToken(token);
                    if (confirmEmail != null)
                    {
                        var domainName = Request.RequestUri.Scheme + "://" + Request.RequestUri.Authority;


                        var envioEmail = new EnvioEmail(_ServerConfigRepo);
                        var msgEmail = envioEmail.EmailCadastro(domainName, token, confirmEmail.Usuario.Email);
                        return Request.CreateResponse(HttpStatusCode.OK, msgEmail);
                    }
                }
                var errorObj = ModelStateErrors.DisplayModelStateError(ModelState);
                return Request.CreateResponse(HttpStatusCode.NotAcceptable, errorObj, "text/plain");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}