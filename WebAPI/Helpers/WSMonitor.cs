using DataAccess.Entidades;
using DataAccess.Helpers;
using DataAccess.Interfaces;
using DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace WebAPI.Helpers
{
    public class WSMonitor
    {
        public static WSMonitor _instancia;
        public List<WSMonitorada> _ListWS = new List<WSMonitorada>();
        private ServerConfig serverConfig = new ServerConfig();

        public WSMonitor(IServerConfigRepository _serverconfigrepo)
        {
            serverConfig = _serverconfigrepo.GetAllConfig();
        }

        //Singleton Pattern
        public static WSMonitor Instancia
        {
            get { return _instancia ?? (_instancia = new WSMonitor(new ServerConfigRepository())); }
        }

        public void AddMonitor(Usuario user)
        {
            var wsmonitorada = GetMonitor(user.Id);
            if (wsmonitorada == null)
            {
                var ativo = new WSMonitorada(user, serverConfig);
                _ListWS.Add(ativo);
            }
            else if (!wsmonitorada.isConnected())
            {
                if (wsmonitorada.ws != null)
                {
                    wsmonitorada.Reconect();
                }
                else
                {
                    RemoveMonitor(wsmonitorada);
                    var NewMonitor = new WSMonitorada(user, serverConfig);
                    _ListWS.Add(NewMonitor);
                }
            }
        }

        public WSMonitorada GetMonitor(int UserID)
        {
            var monitor = this._ListWS.Where(x => x.User.Id == UserID).FirstOrDefault();
            return monitor;
        }

        public void RemoveMonitor(WSMonitorada wsMonitorada)
        {
            wsMonitorada.Dispose();
            this._ListWS.Remove(wsMonitorada);
        }

        public void RemoveMonitor(int UserID)
        {
            var ordemMonitorada = GetMonitor(UserID);
            if (ordemMonitorada != null)
            {
                RemoveMonitor(ordemMonitorada);
            }
        }

        public void LimparMonitoramento()
        {
            foreach (var item in this._ListWS)
            {
                item.Dispose();
            }
            this._ListWS.Clear();
        }     

        public WSMonitor()
        {
            GC.SuppressFinalize(this);
        }
    }
}