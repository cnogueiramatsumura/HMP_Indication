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
using System.Threading.Tasks;
using System.Timers;
using System.Web;
using System.Web.Mvc;
using WebAPI.SignalR;
using WebSocketSharp;

namespace WebAPI.Helpers
{
    public class AtivoMonitorado
    {
        public string Symbol { get; set; }
        public Chamada chamada { get; set; }
        public WebSocket ws;
        IHubContext signalContext = GlobalHost.ConnectionManager.GetHubContext<SignalChamadas>();
        private bool disposedValue = false;
        public decimal GainMonitor;
        public decimal LossMonitor;


        public AtivoMonitorado(Chamada Chamada, string symbol)
        {
            Symbol = symbol;
            this.chamada = Chamada;

            GainMonitor = chamada.PrecoGain;
            LossMonitor = chamada.PrecoLoss;

            ws = new WebSocket("wss://stream.binance.com:9443/stream?streams=" + symbol.ToLower() + "@trade");
            //ws.Log.Level = LogLevel.Trace;
            //ws.Log.File = "changetofilepath";
            ws.OnOpen += (sender, e) =>
            {
                Logs.LogConexaoAtivos(chamada.Id + " => " + DateTime.UtcNow + " => type ( Start Monitor )");
            };

            //Para envio de ping
            ws.EmitOnPing = true;
            ws.OnMessage += (sender, e) =>
            {
                try
                {
                    if (e.IsPing)
                    {
                        ws.Ping();
                        return;
                    }
                    var content = e.Data;
                    var ws_trade = JsonConvert.DeserializeObject<WS_Trade>(content);
                    var marketvalue = ws_trade.data.p;

                    if (chamada.ChamadaStatus_Id == 1)
                    {
                        //verifica se a ordem foi criada com valor acima do preco de mercado ||  se a ordem foi criada com valor abaixo do preco de mercado
                        if ((chamada.PrecoMercadoHoraChamada > chamada.PrecoEntrada && marketvalue <= chamada.PrecoEntrada) || (chamada.PrecoMercadoHoraChamada < chamada.PrecoEntrada && marketvalue >= chamada.PrecoEntrada))
                        {
                            using (var _ChamadasRepo = new ChamadasRepository())
                            {
                                chamada.ChamadaStatus_Id = 2;
                                _ChamadasRepo.Update(chamada);
                                var userIds = _ChamadasRepo.NaoRecusaram_e_nao_Aceitaram_Chamada(chamada.Id);
                                //remove card de chamadas da tela dos clientes   
                                signalContext.Clients.Users(userIds).EncerrarChamada(chamada.Id);
                                signalContext.Clients.User("admin").MudarParaEdicao(chamada.Id);
                                Logs.LogConexaoAtivos(chamada.Id + " => " + DateTime.UtcNow + " => type ( Preco Entrada Alcançado ) => " + marketvalue);
                            }
                        }
                    }
                    else if (chamada.ChamadaStatus_Id == 2)
                    {
                        if (marketvalue > GainMonitor)
                        {
                            using (var _ChamadasRepo = new ChamadasRepository())
                            {
                                chamada.ChamadaStatus_Id = 4;
                                chamada.ResultadoChamada_Id = 1;
                                _ChamadasRepo.Update(chamada);
                                //signalContext.Clients.User("admin").RemoverEdicao(chamada.Id);
                                signalContext.Clients.All.RemoverEdicao(chamada.Id);
                                Logs.LogConexaoAtivos(chamada.Id + " => " + DateTime.UtcNow + " => type ( Preco Gain Alcançado) => " + marketvalue);
                                this.Dispose(true);
                            }
                        }
                        else if (marketvalue < LossMonitor)
                        {
                            using (var _ChamadasRepo = new ChamadasRepository())
                            {
                                chamada.ChamadaStatus_Id = 4;
                                chamada.ResultadoChamada_Id = 2;
                                _ChamadasRepo.Update(chamada);
                                //signalContext.Clients.User("admin").RemoverEdicao(chamada.Id);
                                signalContext.Clients.All.RemoverEdicao(chamada.Id);
                                Logs.LogConexaoAtivos(chamada.Id + " => " + DateTime.UtcNow + " => type ( Preco Loss Alcançado) => " + marketvalue);
                                this.Dispose(true);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logs.LogConexaoAtivos("Error on Try Catch => " + ex.Message + " => inner exception => " + ex.InnerException.Message + " => stack trace => " + ex.StackTrace);
                }
            };

            ws.OnClose += (sender, e) =>
            {
                Logs.LogConexaoAtivos(chamada.Id + " => OnClose Event => " + DateTime.UtcNow + " code => " + e.Code + " motivo => " + e.Reason);
                var monitor = Market_Monitor.Instancia;
                monitor.RemoveMonitor(chamada.Id);
            };

            ws.OnError += (sender, e) =>
            {
                Logs.LogConexaoAtivos(chamada.Id + " => OnError Event => " + DateTime.UtcNow + " Msg Erro => " + e.Message + " => inner exception => " + e.Exception.Message + " => stack trace => " + e.Exception.StackTrace);
            };
            ws.Connect();
        }

        public void Reconect()
        {
            if (!isConnected())
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
                    if (isConnected())
                    {
                        ws.Close();
                        ws = null;
                    }
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