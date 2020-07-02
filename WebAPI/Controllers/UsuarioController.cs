using DataAccess.Entidades;
using DataAccess.Helpers;
using DataAccess.Interfaces;
using DataAccess.Serialized_Objects;
using DataAccess.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Web.Http;
using WebAPI.ActionFIlters;
using WebAPI.Helpers;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class UsuarioController : ApiController
    {
        private readonly IUsuarioRepository _UserRepo;
        private readonly IConfirmEmailRepository _confirEmailRepo;
        private readonly IRecuperarSenha _RecuperarSenhaRepo;
        private readonly IServerConfigRepository _ServerConfigRepo;
        public UsuarioController(IUsuarioRepository userRepo, IConfirmEmailRepository confirEmailRepo, IRecuperarSenha RecuperarSenhaRepo, IServerConfigRepository _ServerConfigRepository)
        {
            _UserRepo = userRepo;
            _confirEmailRepo = confirEmailRepo;
            _RecuperarSenhaRepo = RecuperarSenhaRepo;
            _ServerConfigRepo = _ServerConfigRepository;
        }

        [Authorize]
        public HttpResponseMessage Get()
        {
            try
            {
                var userId = int.Parse(Helper.GetJWTPayloadValue(Request, "id"));
                var user = _UserRepo.GetById(userId);
                return Request.CreateResponse(HttpStatusCode.OK, user);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        public HttpResponseMessage Post(RegisterViewModel model)
        {
            //nao permitir usuarios com email repetidos nao esta validando criar validaçao
            try
            {
                if (model == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotAcceptable, "Campos Inválidos", "text/plain");
                }
                if (ModelState.IsValid)
                {
                    var Usuario = new Usuario
                    {
                        Email = model.email,
                        Nome = model.nome,
                        Sobrenome = model.sobrenome,
                        DataCadastro = DateTime.UtcNow,
                        //Password = Helper.EncryptSha512(model.password),
                        Password = Hashing.HashPassword(model.password),
                        DataVencimentoLicenca = DateTime.UtcNow.AddMonths(3)
                    };
                    _UserRepo.Add(Usuario);
                    var ConfirmEmail = new ConfirmEmail
                    {
                        Usuario_Id = Usuario.Id,
                        Token = Guid.NewGuid().ToString()
                    };
                    _confirEmailRepo.Add(ConfirmEmail);
                    var domainName = Request.RequestUri.Scheme + "://" + Request.RequestUri.Authority;

                    var envioEmail = new EnvioEmail(_ServerConfigRepo);
                    envioEmail.EmailCadastro(domainName, ConfirmEmail.Token, Usuario.Email);
                    return Request.CreateResponse(HttpStatusCode.OK, ConfirmEmail);
                }
                var errorObj = ModelStateErrors.DisplayModelStateError(ModelState);
                return Request.CreateResponse(HttpStatusCode.NotAcceptable, errorObj, "text/plain");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [HttpPost]
        [Route("api/usuario/RecuperarSenha")]  
        public HttpResponseMessage RecuperarSenha(RecuperarSenhaViewModel model)
        {
            try
            {
                if (model == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotAcceptable, "Campos Inválidos", "text/plain");
                }
                if (ModelState.IsValid)
                {
                    var user = _UserRepo.GetByEmail(model.Email);
                    if (user == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.NotAcceptable, "Email Inexistente", "text/plain");
                    }
                    else
                    {
                        var recSenha = new RecuperarSenha
                        {
                            Token = Guid.NewGuid().ToString(),
                            DataCadastro = DateTime.UtcNow,
                            Usuario_Id = user.Id,
                            Utilizado = false
                        };
                        _RecuperarSenhaRepo.Add(recSenha);
                        var domainName = Request.RequestUri.Scheme + "://" + Request.RequestUri.Authority;

                        var envioEmail = new EnvioEmail(_ServerConfigRepo);
                        envioEmail.EmailRecuperarSenha(domainName, recSenha.Token, user.Email);
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                }
                var errorObj = ModelStateErrors.DisplayModelStateError(ModelState);
                return Request.CreateResponse(HttpStatusCode.NotAcceptable, errorObj, "text/plain");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [HttpGet]
        [Route("api/usuario/GerarNovaSenha")]       
        public HttpResponseMessage GerarNovaSenha(string token)
        {
            try
            {
                if (token == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotAcceptable, "Campos Inválidos", "text/plain");
                }
                else
                {
                    var recSenha = _RecuperarSenhaRepo.GetByTokenGuid(token);
                    if (recSenha == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.NotAcceptable, "Token Inválido", "text/plain");
                    }
                    else
                    {
                        if (recSenha.Utilizado == false)
                        {
                            var novasSenha = Helper.GenerateRandomOcoOrderID(12);
                            var hashpassword = Hashing.HashPassword(novasSenha);
                            var user = _UserRepo.GetById(recSenha.Usuario_Id);
                            user.Password = hashpassword;
                            _UserRepo.Update(user);
                            recSenha.Utilizado = true;
                            _RecuperarSenhaRepo.Update(recSenha);
                            var envioEmail = new EnvioEmail(_ServerConfigRepo);
                            envioEmail.EmailEnviarNovaSenha(novasSenha, user.Email);
                            var response = Request.CreateResponse(HttpStatusCode.Moved);
                            var domainName = Request.RequestUri.Scheme + "://" + Request.RequestUri.Host;
                            response.Headers.Location = new Uri(domainName + "/usuario/Login");
                            return response;
                        }
                        else
                        {
                            var response = Request.CreateResponse(HttpStatusCode.Moved);
                            var domainName = Request.RequestUri.Scheme + "://" + Request.RequestUri.Host;
                            response.Headers.Location = new Uri(domainName + "/usuario/register/TokenExpirado");
                            return response;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [HttpGet]
        [Route("api/usuario/saldobtc")]
        [Authorize]
        public HttpResponseMessage GetSaldoBTC()
        {
            try
            {
                var userId = int.Parse(Helper.GetJWTPayloadValue(Request, "id"));
                var user = _UserRepo.GetById(userId);
                var BinanceResult = BinanceRestApi.GetAccountInformation(user.BinanceAPIKey, user.BinanceAPISecret);
                if (BinanceResult.IsSuccessStatusCode)
                {
                    var result = BinanceResult.Content.ReadAsStringAsync().Result;
                    var AccounInformation = JsonConvert.DeserializeObject<Account_Information>(result);
                    var saldo = AccounInformation.balances.Where(x => x.asset == "BTC").Select(x => x.free).FirstOrDefault();
                    return Request.CreateResponse(HttpStatusCode.OK, saldo);
                }
                var BinanceerrorObj = Helper.GetBinanceErrorObj(BinanceResult);
                return Request.CreateResponse(HttpStatusCode.BadRequest, BinanceerrorObj);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("api/usuario/MudarSenha")]
        [Authorize]
        public HttpResponseMessage MudarSenha([FromBody]AlterarSenhaViewModel model)
        {
            try
            {
                if (model == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotAcceptable, "Campos Inválidos", "text/plain");
                }
                if (ModelState.IsValid)
                {
                    var userid = int.Parse(Helper.GetJWTPayloadValue(Request, "id"));                
                    var user = _UserRepo.GetById(userid);
                    if (Hashing.ValidatePassword(model.SenhaAntiga,user.Password))
                    {
                        user.Password = Hashing.HashPassword(model.NovaSenha);
                        _UserRepo.Update(user);
                        return Request.CreateResponse(HttpStatusCode.OK, "Senha Alterada com Successo", "text/plain");
                    }
                    return Request.CreateResponse(HttpStatusCode.NotAcceptable, "Senha Antiga Inválida", "text/plain");
                }
                var errorObj = ModelStateErrors.DisplayModelStateError(ModelState);
                return Request.CreateResponse(HttpStatusCode.NotAcceptable, errorObj, "text/plain");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("api/usuario/SubscriptionOneSignalWeb")]
        [Authorize]
        public HttpResponseMessage SubscriptionOneSignalWeb([FromBody]string onesignalid)
        {
            try
            {
                var userid = int.Parse(Helper.GetJWTPayloadValue(Request, "id"));
                var user = _UserRepo.GetById(userid);
                user.OneSignalIDWeb = onesignalid;
                _UserRepo.Update(user);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("api/usuario/UnSubscriptionOneSignalWeb")]
        [Authorize]
        public HttpResponseMessage UnSubscriptionOneSignalWeb([FromBody]string onesignalid)
        {
            try
            {
                var userid = int.Parse(Helper.GetJWTPayloadValue(Request, "id"));
                var user = _UserRepo.GetById(userid);
                user.OneSignalIDWeb = null;
                _UserRepo.Update(user);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Metodo Somente para ser consumido pelo mobile
        /// utizado para salvar o campo do onesignalID android
        /// </summary>
        /// <param name="onesignalid"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/usuario/SubscriptionOneSignalApp")]
        [Authorize]
        public HttpResponseMessage SubscriptionOneSignalApp([FromBody]string onesignalid)
        {
            try
            {
                var userid = int.Parse(Helper.GetJWTPayloadValue(Request, "id"));
                var user = _UserRepo.GetById(userid);
                user.OneSignalIDApp = onesignalid;
                _UserRepo.Update(user);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("api/usuario/UnSubscriptionOneSignalApp")]
        [Authorize]
        public HttpResponseMessage UnSubscriptionOneSignalApp([FromBody]string onesignalid)
        {
            try
            {
                var userid = int.Parse(Helper.GetJWTPayloadValue(Request, "id"));
                var user = _UserRepo.GetById(userid);
                user.OneSignalIDApp = onesignalid;
                _UserRepo.Update(user);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}