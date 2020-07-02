using DataAccess.Entidades;
using DataAccess.Helpers;
using DataAccess.Interfaces;
using DataAccess.Serialized_Objects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;
using WebAPI.ActionFIlters;
using WebAPI.Helpers;
using WebAPI.Models;
using WebAPI.SignalR;
using System.Net;
using DataAccess.ViewModels;

namespace WebAPI.Controllers
{
    [Authorize]
    // [CheckWsConnection]

    public class ChamadaController : ApiController
    {
        private readonly IUsuarioRepository _UserRepo;
        private readonly IOrdemRepository _OrdemRepo;
        private readonly IChamadasRepository _ChamadasRepo;
        private readonly IChamadasRecusadasRepository _ChamadasRecusadasRepo;
        private readonly IChamadaEditadaRepository _ChamadaEditadaRepository;
        private readonly ICancelamentoChamadaRepository _cancelamentoChamadaRepository;
        private readonly Microsoft.AspNet.SignalR.IHubContext _signalContext;

        public ChamadaController(IUsuarioRepository UserRepository, IOrdemRepository OrdemRepository, IChamadasRepository ChamadasRepository, IChamadasRecusadasRepository ChamadasRecusadasRepository, IChamadaEditadaRepository ChamadasEditadasRepository,
            ICancelamentoChamadaRepository cancelamentoChamadaRepository, Microsoft.AspNet.SignalR.IHubContext hubcontext)
        {
            _UserRepo = UserRepository;
            _OrdemRepo = OrdemRepository;
            _ChamadasRepo = ChamadasRepository;
            _ChamadasRecusadasRepo = ChamadasRecusadasRepository;
            _ChamadaEditadaRepository = ChamadasEditadasRepository;
            _cancelamentoChamadaRepository = cancelamentoChamadaRepository;
            _signalContext = hubcontext;
        }

        public HttpResponseMessage Get(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var chamada = _ChamadasRepo.GetById(id);
                    return Request.CreateResponse(HttpStatusCode.OK, chamada);
                }
                var modelstateError = ModelStateErrors.DisplayModelStateError(ModelState);
                return Request.CreateResponse(HttpStatusCode.NotAcceptable, modelstateError, "text/plain");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userId = int.Parse(Helper.GetJWTPayloadValue(Request, "id"));
                    var user = _UserRepo.GetById(userId);
                    var chamadarecusada = new ChamadasRecusadas()
                    {
                        HoraRecusada = DateTime.UtcNow,
                        Chamada_ID = id,
                        Usuario_ID = userId
                    };
                    _ChamadasRecusadasRepo.Add(chamadarecusada);
                    _signalContext.Clients.User(userId.ToString()).EncerrarChamada(id);
                    return Request.CreateResponse(HttpStatusCode.OK, chamadarecusada);
                }
                var modelstateError = ModelStateErrors.DisplayModelStateError(ModelState);
                return Request.CreateResponse(HttpStatusCode.NotAcceptable, modelstateError, "text/plain");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpPost]
        [CheckWsConnection]
        public HttpResponseMessage Post([FromBody]CreateChamadaViewModel ViewModel)
        {
            try
            {
                if (ViewModel == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotAcceptable, "Campos Inválidos", "text/plain");
                }
                if (ModelState.IsValid)
                {
                    #region Binance
                    var userId = int.Parse(Helper.GetJWTPayloadValue(Request, "id"));
                    var user = _UserRepo.GetById(userId);
                    var chamada = _ChamadasRepo.GetWith_Symbol_and_Filter(ViewModel.id);
                    var ordemType = BinanceHelper.getEntradaOrderType(chamada);
                    string BinanceOrderID = "";
                    if (!BinanceHelper.ValidateFilterPrice(chamada.Symbol.filters.Where(x => x.filterType == "PRICE_FILTER").FirstOrDefault(), ViewModel.qtd))
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new BinanceErrors { code = 0, motivo = "Quantidade Inválida" });
                    }

                    do
                    {
                        BinanceOrderID = Helper.GenerateRandomOcoOrderID(10);
                    } while (_OrdemRepo.IsValidOrderID(BinanceOrderID));

                    HttpResponseMessage BinanceResult = BinanceRestApi.SendOrdemEntrada(user.BinanceAPIKey, user.BinanceAPISecret, ordemType, chamada.Symbol.symbol, ViewModel.qtd, chamada.PrecoEntrada, chamada.RangeEntrada, BinanceOrderID);
                    #endregion
                    if (BinanceResult.IsSuccessStatusCode)
                    {
                        var ordem = new Ordem()
                        {
                            DataCadastro = DateTime.UtcNow,
                            DataExecucao = null,
                            Quantidade = (decimal)ViewModel.qtd,
                            Chamada_Id = ViewModel.id,
                            Usuario_Id = user.Id,
                            OrdemStatus_Id = 1,
                            TipoOrdem_Id = 1,
                            BinanceStatus_Id = 1,
                            OrderID = BinanceOrderID
                        };
                        Logs.LogOrdem(user.Id + " (" + user.Email + ") => " + DateTime.UtcNow + " => Type (Aceitar Chamada Sucesso) ChamadaID => " + chamada.Id);
                        _OrdemRepo.Add(ordem);
                        var objanonimo = new
                        {
                            Id = ordem.Id,
                            TipoOrdem = 1,
                            Status_id = ordem.OrdemStatus_Id,
                            DataCadastro = ordem.DataCadastro,
                            Quantidade = ordem.Quantidade.ToString("N8"),
                            Chamada_Id = ordem.Chamada_Id,
                            Symbol = chamada.Symbol.symbol,
                            Descricao = "Aguardando Entrada",
                            PrecoEntrada = chamada.PrecoEntrada.ToString("N8"),
                            PrecoGain = chamada.PrecoGain.ToString("N8"),
                            PrecoLoss = chamada.PrecoLoss.ToString("N8"),
                            RangeEntrada = chamada.RangeEntrada.ToString("N8"),
                            observacao = chamada.Observacao
                        };
                        //necessario porque o kra pode estar logado no app e na web ao mesmo tempo e tentar enviar nos dois
                        _signalContext.Clients.User(userId.ToString()).RemoverChamada(chamada.Id);
                        _signalContext.Clients.User("admin").AddPosicionado(chamada.Id);
                        return Request.CreateResponse(HttpStatusCode.OK, objanonimo);
                    }
                    else
                    {
                        var BinanceerrorObj = Helper.GetBinanceErrorObj(BinanceResult);
                        Logs.LogOrdem(user.Id + " (" + user.Email + ") => Type (Aceitar Chamada Erro) ChamadaID => " + chamada.Id + " code => " + BinanceerrorObj.code + " motivo => " + BinanceerrorObj.msg);
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

        [HttpGet]
        [Route("api/Chamada/Ativas")]
        public HttpResponseMessage GetAtivas()
        {
            try
            {
                var userId = int.Parse(Helper.GetJWTPayloadValue(Request, "id"));
                var ret = new ChamadasAtivasViewModel
                {
                    Ativas = _ChamadasRepo.GetAllOpen(userId),
                    ChamadaEditadas = _ChamadaEditadaRepository.GetAllOpen(userId),
                    CancelamentoChamadas = _cancelamentoChamadaRepository.GetAllOpen(userId)
                };
                return Request.CreateResponse(HttpStatusCode.OK, ret);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("api/Chamada/Encerradas")]
        public HttpResponseMessage GetEncerradas()
        {
            try
            {
                var userId = int.Parse(Helper.GetJWTPayloadValue(Request, "id"));
                var ret = _ChamadasRepo.GetAllClosed(userId);
                return Request.CreateResponse(HttpStatusCode.OK, ret);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("api/Chamada/Recusadas")]
        public HttpResponseMessage GetRecusadas()
        {
            try
            {
                var userId = int.Parse(Helper.GetJWTPayloadValue(Request, "id"));
                var ret = _ChamadasRepo.GetAllRefused(userId);
                return Request.CreateResponse(HttpStatusCode.OK, ret);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}