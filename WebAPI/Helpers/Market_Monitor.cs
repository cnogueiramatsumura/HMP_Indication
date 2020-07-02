using DataAccess.Entidades;
using DataAccess.Interfaces;
using DataAccess.Repository;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using WebSocketSharp;

namespace WebAPI.Helpers
{
    public class Market_Monitor
    {
        public static Market_Monitor _instancia;
        public List<AtivoMonitorado> _ListAtivos = new List<AtivoMonitorado>();     
       
        //Singleton Pattern
        public static Market_Monitor Instancia
        {
            get { return _instancia ?? (_instancia = new Market_Monitor()); }
        }

        public void AddMonitor(Chamada chamada, string symbol)
        {
            var AtMonitorado = getAtivoMonitorado(chamada.Id);
            if (AtMonitorado == null)
            {
                var ativo = new AtivoMonitorado(chamada, symbol);
                _ListAtivos.Add(ativo);
            }
            else if (!AtMonitorado.isConnected())
            {
                AtMonitorado.Reconect();
            }
        }

        public AtivoMonitorado getAtivoMonitorado(int chamadaID)
        {
            return _ListAtivos.Where(x => x.chamada.Id == chamadaID).FirstOrDefault();
        }

        public void RemoveMonitor(AtivoMonitorado ativoMonitorado)
        {         
            this._ListAtivos.Remove(ativoMonitorado);
        }

        public void RemoveMonitor(int ChamadaId)
        {
            var AtMonitorado = getAtivoMonitorado(ChamadaId);
            if (AtMonitorado != null)
            {
                RemoveMonitor(AtMonitorado);
            }
        }

        public void LimparMonitoramento()
        {
            foreach (var item in this._ListAtivos)
            {
                item.Dispose();
            }
            this._ListAtivos.Clear();
        }

        public void EditarMonitoramento(int chamadaId, decimal PrecoGain, decimal PrecoLoss)
        {
            var ativo = getAtivoMonitorado(chamadaId);
            ativo.GainMonitor = PrecoGain;
            ativo.LossMonitor = PrecoLoss;
        }

        public Market_Monitor()
        {
            GC.SuppressFinalize(this);
        }
    }
}