using DataAccess.Entidades;
using DataAccess.Helpers;
using DataAccess.Interfaces;
using DataAccess.Serialized_Objects;
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

namespace WebAPI.Controllers
{
    [Authorize]
    //[CheckWsConnection]
    public class OrdemController : ApiController
    {
        private readonly IUsuarioRepository _UserRepo;
        private readonly IOrdemRepository _OrdemRepo;
        private readonly IOrdemComissionRepository _OrdemComissionRepo;

        public OrdemController(IUsuarioRepository UserRepository, IOrdemRepository OrdemRepository, IChamadasRepository ChamadasRepository, IOrdemComissionRepository OrdemComissionRepository, Microsoft.AspNet.SignalR.IHubContext hubcontext)
        {
            _UserRepo = UserRepository;
            _OrdemRepo = OrdemRepository;
            _OrdemComissionRepo = OrdemComissionRepository;
        }

        public Ordem Get(int id)
        {
            return _OrdemRepo.GetById(id);
        }

        [HttpGet]
        [Route("api/Ordem/GetbyChamadaID/{id}")]
        public HttpResponseMessage GetbyChamadaID(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userid = int.Parse(Helper.GetJWTPayloadValue(Request, "id"));
                    var ordem = _OrdemRepo.SelecionarbyChamadaID(id, userid);
                    return Request.CreateResponse(HttpStatusCode.OK, ordem);
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
        /// Cancelar a Entrada
        /// </summary>
        /// <param name="id">OrdemId</param>
        /// <returns></returns>
        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userId = int.Parse(Helper.GetJWTPayloadValue(Request, "id"));
                    var user = _UserRepo.GetById(userId);
                    var ordem = _OrdemRepo.GetWith_Chamada_and_Symbol(id);

                    var res = BinanceRestApi.CancelarEntrada(user.BinanceAPIKey, user.BinanceAPISecret, ordem.Chamada.Symbol.symbol, ordem.OrderID);
                    if (res.IsSuccessStatusCode)
                    {
                        ordem.BinanceStatus_Id = 4;
                        ordem.OrdemStatus_Id = 4;
                        ordem.DataCancelamento = DateTime.UtcNow;
                        ordem.MotivoCancelamento_ID = 1;
                        _OrdemRepo.Update(ordem);
                        return Request.CreateResponse(HttpStatusCode.OK, ordem);
                    }
                    else
                    {
                        var BinanceErrorObj = Helper.GetBinanceErrorObj(res);
                        Logs.LogOrdem(user.Id + " (" + user.Email + ") => " + DateTime.UtcNow + " => Type (Erro ao Cancelar Ordem de Entrada) OrdemId => " + id + " code => " + BinanceErrorObj.code + " motivo => " + BinanceErrorObj.msg);
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

        [HttpDelete]
        [Route("api/Ordem/VenderMercado/{id}")]
        [CloseConection]
        public HttpResponseMessage VenderMercado(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userId = int.Parse(Helper.GetJWTPayloadValue(Request, "id"));
                    var user = _UserRepo.GetById(userId);
                    //alterar pra receber como parametro chamada id
                    var OriginalOrder = _OrdemRepo.GetById(id);
                    var ordem = _OrdemRepo.GetOcoOrder(userId, OriginalOrder.Chamada_Id);
                    ordem.MotivoCancelamento_ID = 2;
                    _OrdemRepo.Update(ordem);


                    //caso tenha cancelado e tenha dado erro pra criar venda a mercado
                    if (ordem.OrdemStatus_Id == 4 && ordem.BinanceStatus_Id == 4 && ordem.MotivoCancelamento_ID == 2)
                    {
                        string BinanceOrderID;
                        do
                        {
                            BinanceOrderID = Helper.GenerateRandomOcoOrderID(10);
                        } while (_OrdemRepo.IsValidOrderID(BinanceOrderID));
                        var listAssets = _OrdemComissionRepo.GetOrderComissions(id);
                        decimal SaldoQuantidade = Helper.ArredondarQuantidadeVenda(listAssets, OriginalOrder);
                        var resMarket = BinanceRestApi.VenderMercado(user.BinanceAPIKey, user.BinanceAPISecret, ordem.Chamada.Symbol.symbol, SaldoQuantidade, BinanceOrderID);
                        if (resMarket.IsSuccessStatusCode)
                        {
                            var BinanceContent = resMarket.Content.ReadAsStringAsync().Result;
                            var BinanceResult = JsonConvert.DeserializeObject<NewOrder>(BinanceContent);
                            var newOrder = new Ordem
                            {
                                BinanceStatus_Id = 3,
                                OrdemStatus_Id = 2,
                                OrderID = BinanceOrderID,
                                Chamada_Id = OriginalOrder.Chamada_Id,
                                DataCadastro = DateTime.UtcNow,
                                TipoOrdem_Id = 3,
                                Quantidade = ordem.Quantidade,
                                Usuario_Id = userId,
                                MainOrderID = OriginalOrder.Id,
                                PrecoVendaMercado = BinanceResult.fills.FirstOrDefault().price
                            };
                            _OrdemRepo.Add(newOrder);
                            Logs.LogOrdem(user.Id + " (" + user.Email + ") => " + DateTime.UtcNow + " => Type (Vender a Mercado Sucesso) MainOrderId =>  " + OriginalOrder.Id);
                            OriginalOrder.OrdemStatus_Id = 2;
                            OriginalOrder.BinanceStatus_Id = 3;
                            OriginalOrder.PrecoVendaMercado = BinanceResult.fills.FirstOrDefault().price;
                            _OrdemRepo.Update(OriginalOrder);
                            return Request.CreateResponse(HttpStatusCode.OK, OriginalOrder);
                        }
                        else
                        {
                            var BinanceerrorObj = Helper.GetBinanceErrorObj(resMarket);
                            Logs.LogOrdem(user.Id + " (" + user.Email + ") => " + DateTime.UtcNow + " => Type (Vender a Mercado Erro) MainOrderId => " + OriginalOrder.Id + " code => " + BinanceerrorObj.code + " motivo => " + BinanceerrorObj.msg);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, BinanceerrorObj);
                        }
                    }

                    var res = BinanceRestApi.CancelarOco(user.BinanceAPIKey, user.BinanceAPISecret, ordem.Chamada.Symbol.symbol, ordem.OcoOrderListId);
                    if (res.IsSuccessStatusCode)
                    {
                        ordem.BinanceStatus_Id = 4;
                        ordem.OrdemStatus_Id = 4;
                        ordem.DataCancelamento = DateTime.UtcNow;
                        _OrdemRepo.Update(ordem);

                        string BinanceOrderID;
                        do
                        {
                            BinanceOrderID = Helper.GenerateRandomOcoOrderID(10);
                        } while (_OrdemRepo.IsValidOrderID(BinanceOrderID));
                        var listAssets = _OrdemComissionRepo.GetOrderComissions(id);
                        decimal SaldoQuantidade = Helper.ArredondarQuantidadeVenda(listAssets, OriginalOrder);
                        var resMarket = BinanceRestApi.VenderMercado(user.BinanceAPIKey, user.BinanceAPISecret, ordem.Chamada.Symbol.symbol, SaldoQuantidade, BinanceOrderID);
                        if (resMarket.IsSuccessStatusCode)
                        {
                            var BinanceContent = resMarket.Content.ReadAsStringAsync().Result;
                            var BinanceResult = JsonConvert.DeserializeObject<NewOrder>(BinanceContent);
                            var newOrder = new Ordem
                            {
                                BinanceStatus_Id = 3,
                                OrdemStatus_Id = 2,
                                OrderID = BinanceOrderID,
                                Chamada_Id = OriginalOrder.Chamada_Id,
                                DataCadastro = DateTime.UtcNow,
                                TipoOrdem_Id = 3,
                                Quantidade = ordem.Quantidade,
                                Usuario_Id = userId,
                                MainOrderID = OriginalOrder.Id,
                                PrecoVendaMercado = BinanceResult.fills.FirstOrDefault().price
                            };
                            _OrdemRepo.Add(newOrder);
                            Logs.LogOrdem(user.Id + " (" + user.Email + ") => " + DateTime.UtcNow + " => Type (Vender a Mercado Sucesso) MainOrderId =>  " + OriginalOrder.Id);
                            OriginalOrder.OrdemStatus_Id = 2;
                            OriginalOrder.BinanceStatus_Id = 3;
                            OriginalOrder.PrecoVendaMercado = BinanceResult.fills.FirstOrDefault().price;
                            _OrdemRepo.Update(OriginalOrder);
                            return Request.CreateResponse(HttpStatusCode.OK, OriginalOrder);
                        }
                        else
                        {
                            var BinanceerrorObj = Helper.GetBinanceErrorObj(resMarket);
                            Logs.LogOrdem(user.Id + " (" + user.Email + ") => " + DateTime.UtcNow + " => Type (Vender a Mercado Erro) MainOrderId => " + OriginalOrder.Id + " code => " + BinanceerrorObj.code + " motivo => " + BinanceerrorObj.msg);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, BinanceerrorObj);
                        }
                    }
                    else
                    {
                        var BinanceErrorObj = Helper.GetBinanceErrorObj(res);
                        Logs.LogOrdem(user.Id + " (" + user.Email + ") => " + DateTime.UtcNow + " => Type (Erro ao Cancelar Ordem Oco Para Venda a Mercado) OcoOrderListID => " + ordem.OcoOrderListId + " code => " + BinanceErrorObj.code + " motivo => " + BinanceErrorObj.msg);
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

        [HttpGet]
        [Route("api/Ordem/Ativas")]
        public HttpResponseMessage GetAtivas()
        {
            try
            {
                var userId = int.Parse(Helper.GetJWTPayloadValue(Request, "id"));
                var Ordems = _OrdemRepo.SelecionarPosicionadas(userId);
                return Request.CreateResponse(HttpStatusCode.OK, Ordems);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("api/Ordem/Finalizadas")]
        public HttpResponseMessage GetFinalizadas()
        {
            try
            {
                var userId = int.Parse(Helper.GetJWTPayloadValue(Request, "id"));
                var Ordems = _OrdemRepo.SelecionarFinalizadas(userId);
                return Request.CreateResponse(HttpStatusCode.OK, Ordems);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("api/Ordem/Canceladas")]
        public HttpResponseMessage GetCanceladas()
        {
            try
            {
                var userId = int.Parse(Helper.GetJWTPayloadValue(Request, "id"));
                var Ordems = _OrdemRepo.SelecionarCanceladas(userId);
                return Request.CreateResponse(HttpStatusCode.OK, Ordems);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}