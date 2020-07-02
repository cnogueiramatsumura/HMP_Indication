using DataAccess.Context;
using DataAccess.Entidades;
using DataAccess.Helpers;
using DataAccess.Interfaces;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using WebAPI.Helpers;
using WebAPI.SignalR;

namespace WebAPI.Controllers
{
    public class TestController : ApiController
    {
        private readonly IUsuarioRepository _UserRepo;
        private readonly IChamadasRepository _ChamadasRepo;
        private readonly IOrdemRepository _OrdemRepo;
        private IHubContext _signalContext = GlobalHost.ConnectionManager.GetHubContext<SignalChamadas>();

        public TestController(IUsuarioRepository UsuarioRepository, IChamadasRepository ChamadasRepository, IHubContext HubContext, IOrdemRepository ordemRepository)
        {
            _UserRepo = UsuarioRepository;
            _ChamadasRepo = ChamadasRepository;
            _OrdemRepo = ordemRepository;
            //_signalContext = HubContext;
        }


        [HttpGet]
        [Route("api/test/test")]
        public string test()
        {
           return "string de test";
        }


        [HttpGet]
        [Route("api/test/ws")]
        public HttpResponseMessage WS()
        {
            var ret = new HttpResponseMessage();
            try
            {
                var _wsMonitor = WSMonitor.Instancia;
                string retstring = "";
                foreach (var item in _wsMonitor._ListWS)
                {
                    retstring += "<p>Usuario => " + item.User.Nome + " => id => " + item.User.Id + " key => " + (item.User.BinanceAPIKey != null ? item.User.BinanceAPIKey.Substring(0, 5) : "") + " connection Status => " + (item.ws != null ? item.ws.ReadyState.ToString() : "empty ws") + "</p>";
                }
                ret.Content = new StringContent(retstring, System.Text.Encoding.UTF8, "text/html");
                return ret;
            }
            catch (Exception ex)
            {
                ret.Content = new StringContent(ex.Message, System.Text.Encoding.UTF8, "text/html");
                return ret;
            }
        }

        [HttpGet]
        [Route("api/test/Ativo")]
        public HttpResponseMessage Ativo()
        {
            var ret = new HttpResponseMessage();
            try
            {
                var _market_Monitor = Market_Monitor.Instancia;
                string retstring = "";
                foreach (var item in _market_Monitor._ListAtivos)
                {
                    retstring += "<p>" + item.Symbol + " => id: " + item.chamada.Id + " => entrada: " + item.chamada.PrecoEntrada + " => Gain: " + item.GainMonitor + " => Loss: " + item.LossMonitor + " => status: " + item.chamada.ChamadaStatus_Id + "</p>";
                }
                ret.Content = new StringContent(retstring, Encoding.UTF8, "text/html");
                return ret;
            }
            catch (Exception ex)
            {
                ret.Content = new StringContent(ex.Message, Encoding.UTF8, "text/html");
                return ret;
            }
        }
    }

}
