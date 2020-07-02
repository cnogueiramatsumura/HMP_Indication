using DataAccess.Entidades;
using DataAccess.Helpers;
using DataAccess.Interfaces;
using DataAccess.Repository;
using DataAccess.Serialized_Objects;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Web;
using WebAPI.SignalR;
using WebSocketSharp;

namespace WebAPI.Helpers
{
    public class WSMonitorada : IDisposable
    {
        public WebSocket ws;
        IHubContext signalContext = GlobalHost.ConnectionManager.GetHubContext<SignalChamadas>();
        public Usuario User;
        private Timer timerKeepAlive = new Timer();
        private Timer closeconnection = new Timer();
        private bool disposedValue = false;     
        private OneSignalAPI OneSignalApi;

        static void PingRequests(object sender, ElapsedEventArgs e, Usuario user, WebSocket ws)
        {
            try
            {
                var connstatus = ws.Ping();
                if (!connstatus)
                {
                    ws.Connect();
                }
            }
            catch (Exception ex)
            {
            }
        }

        public WSMonitorada(Usuario user, ServerConfig config)
        {
            this.User = user;     
            OneSignalApi = new OneSignalAPI(config.OneSignalToken, config.AppServer, config.OneSignalAppId);

            if (!string.IsNullOrEmpty(user.BinanceAPIKey) && !string.IsNullOrEmpty(user.BinanceAPISecret))
            {
                timerKeepAlive.Interval = TimeSpan.FromMinutes(15).TotalMilliseconds;
                timerKeepAlive.Elapsed += (object source, ElapsedEventArgs e) =>
                {
                    BinanceUserDataStream.KeepAlive(user.BinanceAPIKey, user.BinanceAPISecret);
                };
                timerKeepAlive.Start();
                // a cada 3 horas verifica se o cara possui ordem aberta se nao possuir desconecta ele
                closeconnection.Interval = TimeSpan.FromHours(1).TotalMilliseconds;
                closeconnection.Elapsed += (object source, ElapsedEventArgs e) =>
                {
                    using (var _OrdemRepo = new OrdemRepository())
                    {
                        var ordensabertas = _OrdemRepo.OrdemsAbertas(User.Id);
                        if (ordensabertas.Count == 0)
                        {
                            this.Dispose(true);
                        }
                    }
                };
                closeconnection.Start();

                var result = BinanceUserDataStream.GetListenKey(user.BinanceAPIKey, user.BinanceAPISecret);
                if (result.IsSuccessStatusCode)
                {
                    var res = result.Content.ReadAsStringAsync().Result;
                    var listenKey = (string)JsonConvert.DeserializeObject<dynamic>(res).listenKey;
                    ws = new WebSocket("wss://stream.binance.com:9443/ws/" + listenKey);
                    //ws.Log.Level = LogLevel.Trace;
                    //ws.Log.File = "C:\\LogConexao\\" + user.nome + ".txt";

                    //Para envio de ping   
                    ws.EmitOnPing = true;

                    ws.OnOpen += (sender, e) =>
                    {

                    };

                    ws.OnMessage += (sender, e) =>
                    {
                        if (e.IsPing)
                        {
                            ws.Ping();
                            return;
                        }

                        var content = e.Data;
                        var ws_Payload = JsonConvert.DeserializeObject<dynamic>(content);

                        //if (ws_Payload.e != "outboundAccountInfo")
                        //{
                        //    Logs.LogOrdem(user.Id + " (" + user.Email + ") => " + DateTime.UtcNow + " => Type (" + ws_Payload.e + ") Payloadobjeto => " + ws_Payload);
                        //}

                        using (var _OrdemRepo = new OrdemRepository())
                        {
                            if ((string)ws_Payload.e == "executionReport")
                            {
                                //Logs.LogOrdem(user.Id + " (" + user.Email + ") => " + DateTime.UtcNow + " => Type (ExecutionReport)  => " + (string)ws_Payload.x + " => " + e.Data);

                                //Ordem Executada
                                if ((string)ws_Payload.x == "TRADE" && (string)ws_Payload.X == "FILLED")
                                {
                                    string ordemID = (string)ws_Payload.c;
                                    var ordem = _OrdemRepo.EntradaByBinanceOrderID(ordemID);
                                    //tentar passar o id pra inteiro significa q a ordem foi gerada no meu sistema, o sisteman binance gera uma string
                                    #region OrderEntrada
                                    if (ordem != null && ordem.TipoOrdem_Id == 1)
                                    {
                                        Logs.LogOrdem(user.Id + " (" + user.Email + ") => " + DateTime.UtcNow + " => Type (Ordem Entrada Executada) OrdemId => " + ordemID);
                                        using (var _OrderCommision = new OrdemComissionRepository())
                                        {
                                            var Comission = new OrdemComission
                                            {
                                                Order_Id = ordem.Id,
                                                ComissionAmount = (decimal)ws_Payload.n,
                                                ComissionAsset = (string)ws_Payload.N,
                                                QtdExecutada = (decimal)ws_Payload.z,
                                                ValorExecutado = (decimal)ws_Payload.p
                                            };
                                            _OrderCommision.Add(Comission);

                                            ordem.DataEntrada = DateTime.UtcNow;
                                            ordem.OrdemStatus_Id = 3;
                                            ordem.BinanceStatus_Id = 3;
                                            _OrdemRepo.Update(ordem);
                                            _OrdemRepo.AddReference(ordem, "OrdemStatus");
                                            signalContext.Clients.User(user.Id.ToString()).EntradaRealizada(ordem);
                                            string LimitOrderID, StopOrderID;
                                            do
                                            {
                                                LimitOrderID = Helper.GenerateRandomOcoOrderID(10);
                                            } while (_OrdemRepo.IsValidOrderID(LimitOrderID));
                                            do
                                            {
                                                StopOrderID = Helper.GenerateRandomOcoOrderID(10);
                                            } while (_OrdemRepo.IsValidOrderID(StopOrderID));
                                            var listAssets = _OrderCommision.GetOrderComissions(ordem.Id);
                                            decimal SaldoQuantidade = Helper.ArredondarQuantidadeVenda(listAssets, ordem);
                                            var limitLoss = Helper.OcoStopLimitWithPercent(ordem.Chamada.Symbol_id, ordem.Chamada.PrecoLoss, 0.5m);
                                            var ocoReturn = BinanceRestApi.SendSaidaOco(user.BinanceAPIKey.Trim(), user.BinanceAPISecret.Trim(), ordem.Chamada.Symbol.symbol, SaldoQuantidade, ordem.Chamada.PrecoGain, ordem.Chamada.PrecoLoss, limitLoss, LimitOrderID, StopOrderID);
                                            if (ocoReturn.IsSuccessStatusCode)
                                            {
                                                var ocoRes = ocoReturn.Content.ReadAsStringAsync().Result;
                                                var ocoObj = JsonConvert.DeserializeObject<dynamic>(ocoRes);
                                                var OcoOrder = new Ordem
                                                {
                                                    DataCadastro = DateTime.UtcNow,
                                                    Quantidade = SaldoQuantidade,
                                                    Chamada_Id = ordem.Chamada_Id,
                                                    Usuario_Id = user.Id,
                                                    OrdemStatus_Id = 3,
                                                    TipoOrdem_Id = 2,
                                                    BinanceStatus_Id = 1,
                                                    StopOrder_ID = StopOrderID,
                                                    LimitOrder_ID = LimitOrderID,
                                                    OcoOrderListId = (string)ocoObj.listClientOrderId,
                                                    MainOrderID = ordem.Id
                                                };
                                                _OrdemRepo.Add(OcoOrder);
                                                var resOco = ocoReturn.Content.ReadAsStringAsync().Result;
                                                Logs.LogOrdem(user.Id + " (" + user.Email + ") => " + DateTime.UtcNow + " =>  Type (Ordem OCO Criada sucesso) OcoOrderListID => " + OcoOrder.OcoOrderListId);
                                            }
                                            else
                                            {
                                                var BinanceerrorObj = Helper.GetBinanceErrorObj(ocoReturn);
                                                Logs.LogOrdem(user.Id + " (" + user.Email + ") => " + DateTime.UtcNow + " =>  Type (Erro ao Criar Ordem Oco) MainOrderID => " + ordem.Id + " => code => " + BinanceerrorObj.code + " => msg => " + BinanceerrorObj.msg + "/" + BinanceerrorObj.motivo);
                                            }
                                        }
                                    }
                                    #endregion
                                    #region VendaMercado
                                    else if (ordem != null && ordem.TipoOrdem_Id == 3)
                                    {
                                        var mainorder = _OrdemRepo.GetById((int)ordem.MainOrderID);
                                        mainorder.PrecoVendaMercado = (decimal)ws_Payload.L;
                                        mainorder.OrdemStatus_Id = 2;
                                        _OrdemRepo.Update(mainorder);
                                    }
                                    #endregion
                                    #region Order Oco
                                    else
                                    {
                                        //var OcoOrderId = (string)ws_Payload.c;
                                        ordem = _OrdemRepo.OcoOrderByBinanceOrderID(ordemID);
                                        if (ordem != null)
                                        {
                                            Logs.LogOrdem(user.Id + " (" + user.Email + ") => " + DateTime.UtcNow + " => Type (Ordem OCO Executada) Stop_Or_Limit_ID => " + ordemID);
                                            var valorExecutado = ws_Payload.p;
                                            ordem.BinanceStatus_Id = 3;
                                            //status da order main tem que ser igual status da ultima ordem filha
                                            var mainOrder = _OrdemRepo.GetWith_Chamada_and_Symbol((int)ordem.MainOrderID);
                                            mainOrder.DataExecucao = DateTime.UtcNow;
                                            using (var _EdicaoAceitaRepo = new EdicaoAceitaRepository())
                                            {
                                                var EdicaoAceita = _EdicaoAceitaRepo.AceitouEdicao(user.Id, mainOrder.Chamada_Id);
                                                if (EdicaoAceita == null)
                                                {
                                                    if (valorExecutado >= ordem.Chamada.PrecoGain)
                                                    {
                                                        ordem.OrdemStatus_Id = 5;
                                                        mainOrder.OrdemStatus_Id = 5;
                                                        _OrdemRepo.Update(mainOrder);
                                                        _OrdemRepo.Update(ordem);
                                                        signalContext.Clients.User(user.Id.ToString()).GainRealizado(mainOrder);
                                                        OneSignalApi.NotificarUsuario(user, mainOrder.Chamada.Symbol.symbol, NotificationType.Gain);
                                                    }
                                                    else if (valorExecutado <= ordem.Chamada.PrecoLoss)
                                                    {
                                                        ordem.OrdemStatus_Id = 6;
                                                        mainOrder.OrdemStatus_Id = 6;
                                                        _OrdemRepo.Update(mainOrder);
                                                        _OrdemRepo.Update(ordem);
                                                        signalContext.Clients.User(user.Id.ToString()).LossRealizado(mainOrder);
                                                        OneSignalApi.NotificarUsuario(user, mainOrder.Chamada.Symbol.symbol, NotificationType.Loss);
                                                    }
                                                }
                                                else
                                                {
                                                    if (valorExecutado >= EdicaoAceita.ChamadaEditada.NewGain)
                                                    {
                                                        ordem.OrdemStatus_Id = 5;
                                                        mainOrder.OrdemStatus_Id = 5;
                                                        _OrdemRepo.Update(mainOrder);
                                                        _OrdemRepo.Update(ordem);
                                                        signalContext.Clients.User(user.Id.ToString()).GainRealizado(mainOrder);
                                                        OneSignalApi.NotificarUsuario(user, mainOrder.Chamada.Symbol.symbol, NotificationType.Gain);
                                                    }
                                                    else if (valorExecutado <= EdicaoAceita.ChamadaEditada.NewLoss)
                                                    {
                                                        ordem.OrdemStatus_Id = 6;
                                                        mainOrder.OrdemStatus_Id = 6;
                                                        _OrdemRepo.Update(mainOrder);
                                                        _OrdemRepo.Update(ordem);
                                                        signalContext.Clients.User(user.Id.ToString()).LossRealizado(mainOrder);
                                                        OneSignalApi.NotificarUsuario(user, mainOrder.Chamada.Symbol.symbol, NotificationType.Loss);
                                                    }
                                                }
                                            }
                                            signalContext.Clients.User(user.Id.ToString()).RemoverEdicao(ordem.Chamada_Id);
                                        }
                                    }
                                    #endregion
                                }

                                //quando a ordem é quebrada em varios valores
                                else if ((string)ws_Payload.x == "TRADE" && (string)ws_Payload.X == "PARTIALLY_FILLED")
                                {
                                    string ordemID = (string)ws_Payload.c;
                                    var ordem = _OrdemRepo.EntradaByBinanceOrderID(ordemID);
                                    if (ordem != null && ordem.TipoOrdem_Id == 1)
                                    {
                                        using (var _OrderCommision = new OrdemComissionRepository())
                                        {
                                            var Comission = new OrdemComission
                                            {
                                                Order_Id = ordem.Id,
                                                ComissionAmount = (decimal)ws_Payload.n,
                                                ComissionAsset = (string)ws_Payload.N,
                                                QtdExecutada = (decimal)ws_Payload.z,
                                                ValorExecutado = (decimal)ws_Payload.p
                                            };
                                            _OrderCommision.Add(Comission);
                                        }
                                    }
                                }

                                //Ordem Expirada
                                else if ((string)ws_Payload.x == "EXPIRED")
                                {
                                    var orderId = (string)ws_Payload.c;
                                    var ordem = _OrdemRepo.EntradaByBinanceOrderID(orderId);
                                    if (ordem != null)
                                    {
                                        ordem.DataCancelamento = DateTime.UtcNow;
                                        ordem.BinanceStatus_Id = 7;
                                        ordem.OrdemStatus_Id = 7;
                                        _OrdemRepo.Update(ordem);
                                        signalContext.Clients.User(user.Id.ToString()).RejeitadaMercadoemFalta(ordem.Id);
                                    }
                                }

                                //Ordem Cancelada
                                else if ((string)ws_Payload.x == "CANCELED")
                                {
                                    var orderId = (string)ws_Payload.C;
                                    var mainOrder = _OrdemRepo.EntradaByBinanceOrderID(orderId);
                                    if (mainOrder != null && mainOrder.MotivoCancelamento_ID == null)
                                    {
                                        Logs.LogOrdem(user.Id + " (" + user.Email + ") => " + DateTime.UtcNow + " => Type (Ordem Entrada Cancelada) OrderID => " + orderId);
                                        mainOrder.DataCancelamento = DateTime.UtcNow;
                                        mainOrder.BinanceStatus_Id = 4;
                                        mainOrder.OrdemStatus_Id = 4;
                                        mainOrder.MotivoCancelamento_ID = 1;
                                        _OrdemRepo.Update(mainOrder);
                                        signalContext.Clients.User(user.Id.ToString()).OrdemCancelada(mainOrder.MainOrderID == null ? mainOrder.Id : mainOrder.MainOrderID);
                                    }
                                    else
                                    {
                                        Logs.LogOrdem(user.Id + " (" + user.Email + ") => " + DateTime.UtcNow + " => Type (Ordem OCO Cancelada) Stop_Or_Limit_ID => " + orderId);
                                        var Ocoordem = _OrdemRepo.OcoOrderByBinanceOrderID(orderId);
                                        //venda a mercado
                                        if (Ocoordem != null && Ocoordem.MotivoCancelamento_ID == 2)
                                        {
                                            //A principio nao fazer nada
                                        }
                                        //Edicao Aceita
                                        else if (Ocoordem != null && Ocoordem.MotivoCancelamento_ID == 3)
                                        {

                                            //mainOrder = _OrdemRepo.GetById((int)Ocoordem.MainOrderID);
                                            //if (mainOrder != null && mainOrder.OrdemStatus_Id != 4)
                                            //{
                                            //    signalContext.Clients.User(user.Id.ToString()).OrdemCancelada(Ocoordem.MainOrderID == null ? mainOrder.Id : Ocoordem.MainOrderID);
                                            //    mainOrder.OrdemStatus_Id = 4;
                                            //    mainOrder.BinanceStatus_Id = 4;
                                            //    mainOrder.DataCancelamento = DateTime.UtcNow;
                                            //    _OrdemRepo.Update(mainOrder);
                                            //}
                                        }
                                    }
                                }
                            }
                            //Atualiza saldo
                            if ((string)ws_Payload.e == "outboundAccountPosition")
                            {
                                try
                                {
                                    var listAssets = ws_Payload.B.ToObject<List<B>>();
                                    var castedList = (List<B>)listAssets;
                                    var saldoBtc = castedList.Where(x => x.a == "BTC").FirstOrDefault().f;
                                    signalContext.Clients.User(user.Id.ToString()).AtualizarSaldo(saldoBtc.ToString("N8"));
                                }
                                catch (Exception)
                                {

                                }
                            }
                        }
                    };

                    ws.OnError += (sender, e) =>
                    {
                        Logs.LogConexao(user.Id + " (" + user.Nome + ") => OnError Event => " + DateTime.UtcNow + " Msg Erro => " + e.Message + " => inner exception => " + e.Exception.Message + " => stack trace => " + e.Exception.StackTrace);
                    };

                    ws.OnClose += (sender, e) =>
                    {
                        try
                        {
                            Logs.LogConexao(user.Id + " (" + user.Nome + ") => OnClose Event => " + DateTime.UtcNow + " code => " + e.Code + " motivo => " + e.Reason);
                            var monitor = WSMonitor.Instancia;
                            monitor.RemoveMonitor(User.Id);                       
                        }
                        catch
                        {
                        }
                    };
                    ws.Connect();
                }
            }
        }

        public void Reconect()
        {
            if (this.ws != null && !isConnected())
            {
                this.ws.Connect();
            }
        }
        public void Disconect()
        {
            if (isConnected())
            {
                this.ws.Close();
            }
        }

        public bool isConnected()
        {
            return (ws != null && ws.ReadyState == WebSocketState.Open);
        }

        #region IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    closeconnection.Dispose();
                    timerKeepAlive.Dispose();
                    if (this.ws != null)
                    {
                        ws.Close();
                    }                 
                    ws = null;
                }           
                disposedValue = true;
            }
        }


        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
