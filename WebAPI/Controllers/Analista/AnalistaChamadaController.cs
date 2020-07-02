using DataAccess.Entidades;
using DataAccess.Helpers;
using DataAccess.Interfaces;
using DataAccess.Repository;
using DataAccess.Serialized_Objects;
using Microsoft.AspNet.SignalR;
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
using WebAPI.SignalR;

namespace WebAPI.Controllers.Analista
{
    public class AnalistaChamadaController : ApiController
    {
        private readonly ISymbolRepository _ISymbolRepo;
        private readonly IChamadasRepository _ChamadasRepo;
        private readonly IChamadaEditadaRepository _IChamadaEditadaRepo;
        private readonly IOrdemRepository _IOrdemRepo;
        private readonly ICancelamentoChamadaRepository _cancelamentoChamadaRepository;
        private readonly IUsuarioRepository _IUserioRepo;
        private readonly IServerConfigRepository _ServerConfigRepo;
        private IHubContext _signalContext;
        private OneSignalAPI OneSignalAPI;

        public AnalistaChamadaController(IServerConfigRepository _ServerConfigRepository, IUsuarioRepository UsuarioRepository, ISymbolRepository SymbolRepository, IChamadasRepository ChamadasRepository, IChamadaEditadaRepository ChamadaEditadaRepository, IOrdemRepository OrdemRepository, ICancelamentoChamadaRepository cancelamentoChamadaRepository, IHubContext HubContext)
        {
            _IUserioRepo = UsuarioRepository;
            _ISymbolRepo = SymbolRepository;
            _ChamadasRepo = ChamadasRepository;
            _IChamadaEditadaRepo = ChamadaEditadaRepository;
            _cancelamentoChamadaRepository = cancelamentoChamadaRepository;
            _ServerConfigRepo = _ServerConfigRepository;
            _IOrdemRepo = OrdemRepository;
            _signalContext = HubContext;

            var config = _ServerConfigRepo.GetAllConfig();
            OneSignalAPI = new OneSignalAPI(config.OneSignalToken, config.AppServer, config.OneSignalAppId);
        }

        [HttpPost]
        public HttpResponseMessage Post([FromBody]AnalistaChamadaViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var PrecoMercado = BinanceHelper.getMarketValue(model.SymbolName.ToUpper()).price;
                    var symbol = _ISymbolRepo.GetBySymbol(model.SymbolName);
                    var chamada = new Chamada()
                    {
                        DataCadastro = DateTime.UtcNow,
                        PrecoEntrada = decimal.Parse(model.PrecoEntrada),
                        RangeEntrada = decimal.Parse(model.RangeEntrada),
                        PrecoGain = decimal.Parse(model.PrecoGain),
                        PrecoLoss = decimal.Parse(model.PrecoLoss),
                        Observacao = model.Observacao,
                        ChamadaStatus_Id = 1,
                        PrecoMercadoHoraChamada = PrecoMercado,
                        Symbol_id = symbol.Id,
                        PercentualIndicado = model.PercentualIndicado,
                        SymbolDescription = model.SymbolDescription.ToUpper(),
                        Analista_Id = model.Analista_Id
                    };
                    _ChamadasRepo.Add(chamada);
                    _ChamadasRepo.Detach(chamada);
                    //obs: nao pode remover a reference passas os filtros pro app
                    var obj = _ChamadasRepo.GetWith_Symbol_and_Filter(chamada.Id);
                    Market_Monitor monitor = Market_Monitor._instancia;
                    monitor.AddMonitor(chamada, model.SymbolName.ToLower());
                    //notifico todos os clientes conectados via signalR                 
                    _signalContext.Clients.All.AdicionarChamada(obj);

                    var listclients = _IUserioRepo.OneSignalIds();
                    OneSignalAPI.NotificarWeb(listclients, model.SymbolName, NotificationType.Entrada);
                    OneSignalAPI.NotificarApp(listclients, model.SymbolName, NotificationType.Entrada);

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                var errorObj = ModelStateErrors.DisplayModelStateError(ModelState);
                return Request.CreateResponse(HttpStatusCode.NotAcceptable, errorObj, "text/plain");
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
                    var chamada = _ChamadasRepo.GetWith_Symbol(id);
                    chamada.ChamadaStatus_Id = 3;
                    _ChamadasRepo.Update(chamada);

                    var cancelamentochamada = new CancelamentoChamada
                    {
                        DataCancelamento = DateTime.UtcNow,
                        Chamada_Id = id
                    };
                    _cancelamentoChamadaRepository.Add(cancelamentochamada);
                    var monitor = Market_Monitor.Instancia;
                    monitor.RemoveMonitor(id);

                    var userIds = _ChamadasRepo.NaoRecusaram_e_nao_Aceitaram_Chamada(chamada.Id);
                    _signalContext.Clients.Users(userIds).EncerrarChamada(chamada.Id);

                    var UsuariosAguardandoEntrada = _ChamadasRepo.UsuariosAguardandoEntrada(id);
                    var AguardandoEntradaIds = UsuariosAguardandoEntrada.Select(x => x.Id.ToString()).ToList();
                    _signalContext.Clients.Users(AguardandoEntradaIds).CancelarEntrada(chamada.Symbol.symbol);
                                    

                    #region Cancelando Automaticamente
                    var ordensParaCancelar = _IOrdemRepo.PosicionadosPorChamada(id);
                    foreach (var ordem in ordensParaCancelar)
                    {
                        var ret = BinanceRestApi.CancelarEntrada(ordem.Usuario.BinanceAPIKey, ordem.Usuario.BinanceAPISecret, ordem.Chamada.Symbol.symbol, ordem.OrderID);
                        if (ret.IsSuccessStatusCode)
                        {
                            ordem.BinanceStatus_Id = 4;
                            ordem.OrdemStatus_Id = 4;
                            ordem.DataCancelamento = DateTime.UtcNow;
                            _signalContext.Clients.User(ordem.Usuario_Id.ToString()).OrdemCancelada(ordem.Id);
                            _IOrdemRepo.Update(ordem);
                        }
                    }
                    OneSignalAPI.NotificarWeb(UsuariosAguardandoEntrada, chamada.Symbol.symbol, NotificationType.CancelamentoEntrada);
                    OneSignalAPI.NotificarApp(UsuariosAguardandoEntrada, chamada.Symbol.symbol, NotificationType.CancelamentoEntrada);

                    #endregion
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

        [HttpPost]
        [Route("api/AnalistaChamada/EditarChamada")]
        public HttpResponseMessage EditarChamada([FromBody]EditChamadasViewModel viewmodel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var chamadaEditada = new ChamadaEditada
                    {
                        Chamada_Id = viewmodel.Chamada_Id,
                        DataEdicao = DateTime.UtcNow,
                        NewGain = viewmodel.NewGain,
                        NewLoss = viewmodel.NewLoss
                    };
                    _IChamadaEditadaRepo.Add(chamadaEditada);
                    var chamadaAntiga = _ChamadasRepo.GetWith_Symbol(viewmodel.Chamada_Id);
                    var retObj = new
                    {
                        id = chamadaEditada.Id,
                        chamada_Id = viewmodel.Chamada_Id,
                        dataEdicao = DateTime.UtcNow,
                        newGain = viewmodel.NewGain,
                        newLoss = viewmodel.NewLoss,
                        symbol = viewmodel.symbol,
                        chamada = chamadaAntiga
                    };

                    var UserIds = _IOrdemRepo.UsuariosPosicionados(viewmodel.Chamada_Id);
                    _signalContext.Clients.Users(UserIds).ChamadaEditada(retObj);
                    var monitor = Market_Monitor.Instancia;
                    monitor.EditarMonitoramento(viewmodel.Chamada_Id, viewmodel.NewGain, viewmodel.NewLoss);

                    //ids dos que possuem ordem posicionadas
                    var listClients = _IOrdemRepo.EditadasOneSignalIds(viewmodel.Chamada_Id);
                    OneSignalAPI.NotificarWeb(listClients, viewmodel.symbol, NotificationType.Edicao);
                    OneSignalAPI.NotificarApp(listClients, viewmodel.symbol, NotificationType.Edicao);

                    return Request.CreateResponse(HttpStatusCode.OK);
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