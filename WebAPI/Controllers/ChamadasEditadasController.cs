using DataAccess.Entidades;
using DataAccess.Helpers;
using DataAccess.Interfaces;
using DataAccess.Serialized_Objects;
using DataAccess.ViewModels;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebAPI.ActionFIlters;
using WebAPI.Helpers;
using WebAPI.Models;
using WebAPI.SignalR;

namespace WebAPI.Controllers
{
    [System.Web.Http.Authorize]
    //[CheckWsConnection]
    public class ChamadasEditadasController : ApiController
    {
        private readonly IUsuarioRepository _UserRepo;
        private readonly IOrdemRepository _OrdemRepo;
        private readonly IEdicaoAceitaRepository _EdicaoAceitaRepo;
        private readonly IChamadaEditadaRepository _ChamadaEditadaRepo;

        public ChamadasEditadasController(IUsuarioRepository UserRepository, IOrdemRepository OrdemRepository, IEdicaoAceitaRepository EdicaoAceitaRepository, IChamadaEditadaRepository ChamadaEditadaRepository)
        {
            _UserRepo = UserRepository;
            _OrdemRepo = OrdemRepository;
            _EdicaoAceitaRepo = EdicaoAceitaRepository;
            _ChamadaEditadaRepo = ChamadaEditadaRepository;
        }

        [HttpPost]
        [Route("api/ChamadasEditadas/aceitar")]
        public HttpResponseMessage aceitar([FromBody]AceitarEdicaoViewModel model)
        {
            try
            {
                if (model == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotAcceptable, "Campos Inválidos", "text/plain");
                }
                if (ModelState.IsValid)
                {
                    var userId = int.Parse(Helper.GetJWTPayloadValue(Request, "id"));
                    var chamadaEditada = _ChamadaEditadaRepo.GetById(model.EdicaoId);
                    var user = _UserRepo.GetById(userId);
                    var ordem = _OrdemRepo.GetOcoOrder(userId, model.ChamadaId);
                    ordem.MotivoCancelamento_ID = 3;
                    _OrdemRepo.Update(ordem);

                    //caso tenha cancelado e dado erro pra criar outra oco
                    if (ordem.OrdemStatus_Id == 4 && ordem.BinanceStatus_Id == 4 && ordem.MotivoCancelamento_ID == 3)
                    {
                        string LimitOrderID, StopOrderID;
                        do
                        {
                            LimitOrderID = Helper.GenerateRandomOcoOrderID(10);
                        } while (_OrdemRepo.IsValidOrderID(LimitOrderID));
                        do
                        {
                            StopOrderID = Helper.GenerateRandomOcoOrderID(10);
                        } while (_OrdemRepo.IsValidOrderID(StopOrderID));
                        var limitLoss = Helper.OcoStopLimitWithPercent(ordem.Chamada.Symbol_id, chamadaEditada.NewLoss, 0.5m);
                        //cria uma nova ordem
                        var ocoReturn = BinanceRestApi.SendSaidaOco(user.BinanceAPIKey, user.BinanceAPISecret, ordem.Chamada.Symbol.symbol, ordem.Quantidade, chamadaEditada.NewGain, chamadaEditada.NewLoss, limitLoss, LimitOrderID, StopOrderID);
                        if (ocoReturn.IsSuccessStatusCode)
                        {
                            var edicaoAceita = new EdicaoAceita
                            {
                                Usuario_Id = userId,
                                TipoEdicao_ID = 1,
                                DataCadastro = DateTime.UtcNow,
                                ChamadaEditada_ID = model.EdicaoId,
                                Chamada_ID = model.ChamadaId
                            };
                            _EdicaoAceitaRepo.Add(edicaoAceita);

                            var ocoRes = ocoReturn.Content.ReadAsStringAsync().Result;
                            var ocoObj = JsonConvert.DeserializeObject<dynamic>(ocoRes);
                            var OcoOrder = new Ordem
                            {
                                DataCadastro = DateTime.UtcNow,
                                DataExecucao = null,
                                Quantidade = ordem.Quantidade,
                                Chamada_Id = ordem.Chamada_Id,
                                Usuario_Id = user.Id,
                                OrdemStatus_Id = 3,
                                TipoOrdem_Id = 2,
                                BinanceStatus_Id = 1,
                                StopOrder_ID = StopOrderID,
                                LimitOrder_ID = LimitOrderID,
                                OcoOrderListId = (string)ocoObj.listClientOrderId,
                                MainOrderID = ordem.MainOrderID
                            };
                            _OrdemRepo.Add(OcoOrder);

                            var retunrobj = new { PrecoEntrada = ordem.Chamada.PrecoEntrada, NewGain = chamadaEditada.NewGain, NewLoss = chamadaEditada.NewLoss, chamadaId = chamadaEditada.Chamada_Id };
                            Logs.LogOrdem(user.Id + " (" + user.Email + ") => Type (Criar Ordem Editada Sucesso) MainOrderID => " + OcoOrder.MainOrderID);
                            return Request.CreateResponse(HttpStatusCode.OK, retunrobj);
                        }
                        else
                        {
                            var BinanceErrorObj = Helper.GetBinanceErrorObj(ocoReturn);
                            Logs.LogOrdem(user.Id + " (" + user.Email + ") => Type (Erro ao Criar Ordem Editada) OcoOrderListID => " + ordem.OcoOrderListId + " code => " + BinanceErrorObj.code + " motivo => " + BinanceErrorObj.msg);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, BinanceErrorObj);
                        }
                    }

                    var res = BinanceRestApi.CancelarOco(user.BinanceAPIKey, user.BinanceAPISecret, ordem.Chamada.Symbol.symbol, ordem.OcoOrderListId);
                    if (res.IsSuccessStatusCode)
                    {
                        ordem.OrdemStatus_Id = 4;
                        ordem.BinanceStatus_Id = 4;
                        ordem.DataCancelamento = DateTime.UtcNow;
                        _OrdemRepo.Update(ordem);
                        string LimitOrderID, StopOrderID;
                        do
                        {
                            LimitOrderID = Helper.GenerateRandomOcoOrderID(10);
                        } while (_OrdemRepo.IsValidOrderID(LimitOrderID));
                        do
                        {
                            StopOrderID = Helper.GenerateRandomOcoOrderID(10);
                        } while (_OrdemRepo.IsValidOrderID(StopOrderID));
                        var limitLoss = Helper.OcoStopLimitWithPercent(ordem.Chamada.Symbol_id, chamadaEditada.NewLoss, 0.5m);
                        //cria uma nova ordem
                        var ocoReturn = BinanceRestApi.SendSaidaOco(user.BinanceAPIKey, user.BinanceAPISecret, ordem.Chamada.Symbol.symbol, ordem.Quantidade, chamadaEditada.NewGain, chamadaEditada.NewLoss, limitLoss, LimitOrderID, StopOrderID);
                        if (ocoReturn.IsSuccessStatusCode)
                        {
                            var edicaoAceita = new EdicaoAceita
                            {
                                Usuario_Id = userId,
                                TipoEdicao_ID = 1,
                                DataCadastro = DateTime.UtcNow,
                                ChamadaEditada_ID = model.EdicaoId,
                                Chamada_ID = model.ChamadaId
                            };
                            _EdicaoAceitaRepo.Add(edicaoAceita);

                            var ocoRes = ocoReturn.Content.ReadAsStringAsync().Result;
                            var ocoObj = JsonConvert.DeserializeObject<dynamic>(ocoRes);
                            var OcoOrder = new Ordem
                            {
                                DataCadastro = DateTime.UtcNow,
                                DataExecucao = null,
                                Quantidade = ordem.Quantidade,
                                Chamada_Id = ordem.Chamada_Id,
                                Usuario_Id = user.Id,
                                OrdemStatus_Id = 3,
                                TipoOrdem_Id = 2,
                                BinanceStatus_Id = 1,
                                StopOrder_ID = StopOrderID,
                                LimitOrder_ID = LimitOrderID,
                                OcoOrderListId = (string)ocoObj.listClientOrderId,
                                MainOrderID = ordem.MainOrderID
                            };
                            _OrdemRepo.Add(OcoOrder);

                            var retunrobj = new { PrecoEntrada = ordem.Chamada.PrecoEntrada, NewGain = chamadaEditada.NewGain, NewLoss = chamadaEditada.NewLoss, chamadaId = chamadaEditada.Chamada_Id };
                            Logs.LogOrdem(user.Id + " (" + user.Email + ") => Type (Criar Ordem Editada Sucesso) => OrderId => " + OcoOrder.Id + " =>  MainOrderID => " + OcoOrder.MainOrderID);
                            return Request.CreateResponse(HttpStatusCode.OK, retunrobj);
                        }
                        else
                        {
                            var BinanceErrorObj = Helper.GetBinanceErrorObj(ocoReturn);
                            Logs.LogOrdem(user.Id + " (" + user.Email + ") => Type (Erro ao Criar Ordem Editada) OcoOrderListID => " + ordem.OcoOrderListId + " code => " + BinanceErrorObj.code + " motivo => " + BinanceErrorObj.msg);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, BinanceErrorObj);
                        }
                    }
                    else
                    {
                        var BinanceErrorObj = Helper.GetBinanceErrorObj(res);
                        Logs.LogOrdem(user.Id + " (" + user.Email + ") => Type (Erro ao Cancelar Ordem Para Aceitar Edição) OcoOrderListID => " + ordem.OcoOrderListId + " code => " + BinanceErrorObj.code + " motivo => " + BinanceErrorObj.msg);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, BinanceErrorObj);
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

        [HttpPost]
        [Route("api/ChamadasEditadas/recusar")]
        public HttpResponseMessage recusar([FromBody]AceitarEdicaoViewModel model)
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
                    var chamadaEditada = new EdicaoAceita
                    {
                        Usuario_Id = user.Id,
                        TipoEdicao_ID = 2,
                        DataCadastro = DateTime.UtcNow,
                        ChamadaEditada_ID = model.EdicaoId,
                        Chamada_ID = model.ChamadaId
                    };
                    _EdicaoAceitaRepo.Add(chamadaEditada);
                    return Request.CreateResponse(HttpStatusCode.OK, chamadaEditada);
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