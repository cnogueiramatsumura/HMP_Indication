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
using System.Web;
using System.Web.Http;
using WebAPI.Helpers;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Authorize]
    public class BinanceController : ApiController
    {
        private readonly IUsuarioRepository _UserRepo;
        public BinanceController(IUsuarioRepository UserRepository)
        {
            _UserRepo = UserRepository;
        }
        /// <summary>
        /// Atualiza as Chaves da api binance 
        /// </summary>
        /// <param name="ViewModel"></param>
        /// <returns></returns>
        public HttpResponseMessage Post([FromBody]SetKeysViewModel ViewModel)
        {
            try
            {
                if (ViewModel == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotAcceptable, "Campos Inválidos", "text/plain");
                }
                if (ModelState.IsValid)
                {
                    var userId = int.Parse(Helper.GetJWTPayloadValue(Request, "id"));
                    var user = _UserRepo.GetById(userId);
                    var BinanceResult = BinanceRestApi.GetAccountInformation(ViewModel.binanceKey, ViewModel.binanceSecret);
                    if (BinanceResult.IsSuccessStatusCode)
                    {
                        user.BinanceAPIKey = ViewModel.binanceKey;
                        user.BinanceAPISecret = ViewModel.binanceSecret;
                        user.IsValidBinanceKeys = true;
                        _UserRepo.Update(user);
                        var monitor = WSMonitor.Instancia;
                        monitor.RemoveMonitor(userId);
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else
                    {
                        user.IsValidBinanceKeys = false;
                        _UserRepo.Update(user);
                        var BinanceerrorObj = Helper.GetBinanceErrorObj(BinanceResult);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, BinanceerrorObj);
                    }
                }
                var modelstateError = ModelStateErrors.DisplayModelStateError(ModelState);
                return Request.CreateResponse(HttpStatusCode.NotAcceptable, modelstateError, "text/plain");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        /// <summary>
        /// Compra BNB a preço de Mercado
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/binance/ComprarBNB")]
        public HttpResponseMessage ComprarBNB(ComprarBNBViewModel ViewModel)
        {
            try
            {
                if (ViewModel == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotAcceptable, "Campos Inválidos", "text/plain");
                }
                if (ModelState.IsValid)
                {
                    var userId = int.Parse(Helper.GetJWTPayloadValue(Request, "id"));
                    var user = _UserRepo.GetById(userId);
                    var BinanceResult = BinanceRestApi.ComprarBNB(user.BinanceAPIKey, user.BinanceAPISecret, ViewModel.qtd);
                    if (BinanceResult.IsSuccessStatusCode)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else
                    {
                        var BinanceerrorObj = Helper.GetBinanceErrorObj(BinanceResult);
                        Logs.LogOrdem(user.Id + " (" + user.Email + ") => Type (Compar BNB Erro)" + " code => " + BinanceerrorObj.code + " motivo => " + BinanceerrorObj.msg);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, BinanceerrorObj);
                    }
                }
                var modelstateError = ModelStateErrors.DisplayModelStateError(ModelState);
                return Request.CreateResponse(HttpStatusCode.NotAcceptable, modelstateError, "text/plain");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        /// <summary>
        /// Testa Pra saber se as chaves da api são validas
        /// </summary>
        /// <param name="ViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/binance/TestKeys")]
        public HttpResponseMessage TestKeys(SetKeysViewModel ViewModel)
        {
            try
            {
                if (ViewModel == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotAcceptable, "Campos Inválidos", "text/plain");
                }
                if (ModelState.IsValid)
                {
                    var BinanceResult = BinanceRestApi.GetAccountInformation(ViewModel.binanceKey, ViewModel.binanceSecret);
                    if (BinanceResult.IsSuccessStatusCode)
                    {

                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else
                    {
                        var BinanceerrorObj = Helper.GetBinanceErrorObj(BinanceResult);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, BinanceerrorObj);
                    }
                }
                var modelstateError = ModelStateErrors.DisplayModelStateError(ModelState);
                return Request.CreateResponse(HttpStatusCode.NotAcceptable, modelstateError, "text/plain");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}