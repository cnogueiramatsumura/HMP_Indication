using DataAccess.Repository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace WebApp.Helpers
{
    public class ApiUsuario
    {
        public static string EndPoint;
        static ApiUsuario()
        {
            var _ServerConfigRepository = new ServerConfigRepository();
            EndPoint = _ServerConfigRepository.GetApiServer();
        }

        #region Usuario
        /// <summary>
        /// Cadastrar um Usuario
        /// </summary>
        /// <param name="formulario"></param>
        /// <returns></returns>
        public static HttpResponseMessage PostRegister(HttpContent formulario)
        {
            var URN = "/api/Usuario/";
            var url = EndPoint + URN;
            var resultado = Post(url, formulario);
            return resultado;
        }
        public static HttpResponseMessage SubscriptionOneSignal(HttpContent formulario, string token)
        {
            var URN = "/api/usuario/SubscriptionOneSignalWeb";
            var url = EndPoint + URN;
            var resultado = PostWithToken(url, formulario, token);
            return resultado;
        }
        public static HttpResponseMessage UnSubscriptionOneSignal(HttpContent formulario, string token)
        {
            var URN = "/api/Usuario/UnSubscriptionOneSignalWeb";
            var url = EndPoint + URN;
            var resultado = PostWithToken(url, formulario, token);
            return resultado;
        }
        public static HttpResponseMessage MudarSenha(HttpContent formulario, string token)
        {
            var URN = "/api/Usuario/MudarSenha";
            var url = EndPoint + URN;
            var resultado = PostWithToken(url, formulario, token);
            return resultado;
        }
        public static HttpResponseMessage RecuperarSenha(HttpContent formulario)
        {
            var URN = "/api/Usuario/RecuperarSenha";
            var url = EndPoint + URN;
            var resultado = Post(url, formulario);
            return resultado;
        }
        #endregion

        #region Login
        /// <summary>
        /// Pega chave da API
        /// </summary>
        /// <param name="email">email</param>
        /// <param name="password">password</param>
        /// <returns></returns>
        public static HttpResponseMessage GetToken(string email, string password)
        {
            var url = EndPoint + "/api/login";
            HttpContent form = new FormUrlEncodedContent(new[]
             {
                new KeyValuePair<string, string>("email", email),
                new KeyValuePair<string, string>("password",password)
             });
            var response = Post(url, form);
            return response;
        }
        /// <summary>
        /// Deslogar do Sistema
        /// </summary>
        /// <param name="token">jwt Token</param>
        /// <returns></returns>
        public static HttpResponseMessage Logoff(string token)
        {
            var url = EndPoint + "/api/login";
            var resultado = DeleteWithToken(url, token);
            return resultado;
        }
        #endregion

        #region Binance
        /// <summary>
        /// Seta as Chaves da APi Binance
        /// </summary>
        /// <param name="formulario"></param>
        /// <param name="token">jwt Token</param>
        /// <returns></returns>
        public static HttpResponseMessage BinanceKeys(HttpContent formulario, string token)
        {
            var URN = "/api/binance/";
            var url = EndPoint + URN;
            var resultado = PostWithToken(url, formulario, token);
            return resultado;
        }
        /// <summary>
        /// Comprar BNB com valor a mercado
        /// </summary>
        /// <param name="formulario"></param>
        /// <param name="token">jwt Token</param>
        /// <returns></returns>
        public static HttpResponseMessage ComprarBNB(HttpContent formulario, string token)
        {
            var URN = "/api/binance/ComprarBNB";
            var url = EndPoint + URN;
            var resultado = PostWithToken(url, formulario, token);
            return resultado;
        }
        /// <summary>
        /// Testa Para saber se as chaves da binance sao validas
        /// </summary>
        /// <param name="formulario"></param>
        /// <param name="token">jwt token</param>
        /// <returns></returns>
        public static HttpResponseMessage BinanceTestKeys(HttpContent formulario, string token)
        {
            var URN = "/api/binance/TestKeys/";
            var url = EndPoint + URN;
            var resultado = PostWithToken(url, formulario, token);
            return resultado;
        }
        #endregion

        #region Chamadas
        /// <summary>
        /// Seleciona Todas as Chamadas
        /// </summary>
        /// <param name="token">jwt Token</param>
        /// <returns></returns>
        public static HttpResponseMessage Getchamada(string token)
        {
            var URN = "/api/chamada/";
            var url = EndPoint + URN;
            var resultado = Get(url, token);
            return resultado;
        }
        /// <summary>
        /// Seleciona Uma chamada pelo ID
        /// </summary>
        /// <param name="token">jwt Token</param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static HttpResponseMessage Getchamada(string token, int id)
        {
            var URN = "/api/chamada/" + id;
            var url = EndPoint + URN;

            var resultado = Get(url, token);
            return resultado;
        }
        /// <summary>
        /// Seleciona Todas as Chamadas que estiverem em Aberto
        /// </summary>
        /// <param name="token">jwt Token</param>
        /// <returns></returns>
        public static HttpResponseMessage GetchamadaAtivas(string token)
        {
            var URN = "/api/chamada/Ativas";
            var url = EndPoint + URN;

            var resultado = Get(url, token);
            return resultado;
        }
        /// <summary>
        /// Seleciona Todas as Chamadas que ja foram Encerradas
        /// </summary>
        /// <param name="token">jwt Token</param>
        /// <returns></returns>
        public static HttpResponseMessage GetchamadaEncerradas(string token)
        {
            var URN = "/api/chamada/Encerradas";
            var url = EndPoint + URN;

            var resultado = Get(url, token);
            return resultado;
        }
        /// <summary>
        /// Seleciona Todas as Chamadas que foram recusadas
        /// </summary>
        /// <param name="token">jwt Token</param>
        /// <returns></returns>
        public static HttpResponseMessage GetchamadaRecusadas(string token)
        {
            var URN = "/api/chamada/Recusadas";
            var url = EndPoint + URN;

            var resultado = Get(url, token);
            return resultado;
        }
        /// <summary>
        /// Aceita uma chamada posicionando um Ordem 
        /// </summary>
        /// <param name="token">jwt Token</param>
        /// <param name="formulario"></param>
        /// <returns></returns>
        public static HttpResponseMessage Postchamada(string token, HttpContent formulario)
        {
            var URN = "/api/chamada/";
            var url = EndPoint + URN;
            var resultado = PostWithToken(url, formulario, token);
            return resultado;
        }
        /// <summary>
        /// Recusa uma chamada
        /// </summary>
        /// <param name="token">jwt Token</param>
        /// <param name="id">id chamada</param>
        /// <returns></returns>
        public static HttpResponseMessage RecusarChamada(string token, int id)
        {
            var URN = "/api/chamada/" + id;
            var url = EndPoint + URN;
            var resultado = DeleteWithToken(url, token);
            return resultado;
        }
        #endregion

        #region Ordems
        /// <summary>
        /// Seleciona Todas as Ordens
        /// </summary>
        /// <param name="token">jwt Token</param>
        /// <returns></returns>
        public static HttpResponseMessage GetOrdem(string token)
        {
            var URN = "/api/ordem/";
            var url = EndPoint + URN;
            var resultado = Get(url, token);
            return resultado;
        }
        /// <summary>
        /// Seleciona Uma ordem pelo ID
        /// </summary>
        /// <param name="token">jwt Token</param>
        /// <param name="id">id da ordem</param>
        /// <returns></returns>
        public static HttpResponseMessage GetOrdem(string token, int id)
        {
            var URN = "/api/ordem/" + id;
            var url = EndPoint + URN;
            var resultado = Get(url, token);
            return resultado;
        }
        /// <summary>
        /// Seleciona a main Ordem que esta associada a Chamada
        /// </summary>
        /// <param name="token">jwt Token</param>
        /// <param name="ChamadaId">Id chamada</param>
        /// <returns></returns>
        public static HttpResponseMessage GetOrdemByChamadaID(string token, int ChamadaId)
        {
            var URN = "/api/ordem/GetbyChamadaid/" + ChamadaId;
            var url = EndPoint + URN;
            var resultado = Get(url, token);
            return resultado;
        }
        /// <summary>
        /// Seleciona Todas as Ordems que estao Posicionadas
        /// </summary>
        /// <param name="token">jwt Token</param>
        /// <returns></returns>
        public static HttpResponseMessage GetOrdemsAtivas(string token)
        {
            var URN = "/api/ordem/Ativas";
            var url = EndPoint + URN;

            var resultado = Get(url, token);
            return resultado;
        }
        /// <summary>
        /// Seleciona Todas as Ordems Finalizadas
        /// </summary>
        /// <param name="token">jwt Token</param>
        /// <returns></returns>
        public static HttpResponseMessage GetOrdemsFinalizadas(string token)
        {
            var URN = "/api/ordem/Finalizadas";
            var url = EndPoint + URN;

            var resultado = Get(url, token);
            return resultado;
        }
        /// <summary>
        /// Seleciona Todas as Ordens que foram Canceladas
        /// </summary>
        /// <param name="token">jwt Token</param>
        /// <returns></returns>
        public static HttpResponseMessage GetOrdemsCanceladas(string token)
        {
            var URN = "/api/ordem/Canceladas";
            var url = EndPoint + URN;

            var resultado = Get(url, token);
            return resultado;
        }
        /// <summary>
        /// Cancela Uma Ordem que esteja em aberto 
        /// </summary>
        /// <param name="token">jwt Token</param>
        /// <param name="id">id da Ordem</param>
        /// <returns></returns>
        public static HttpResponseMessage CancelarEntrada(string token, int id)
        {
            var URN = "/api/ordem/" + id;
            var url = EndPoint + URN;
            var resultado = DeleteWithToken(url, token);
            return resultado;
        }
        /// <summary>
        /// Vende Uma orden que esteja posicionada a valor de Mercado
        /// </summary>
        /// <param name="token">jwt Token</param>
        /// <param name="id">Id da Ordem</param>
        /// <returns></returns>
        public static HttpResponseMessage SairMercado(string token, int id)
        {
            var URN = "/api/ordem/VenderMercado/" + id;
            var url = EndPoint + URN;
            var resultado = DeleteWithToken(url, token);
            return resultado;
        }
        #endregion

        #region ChamadasEditas
        /// <summary>
        /// Aceita Ediçao
        /// </summary>
        /// <param name="token">jwt Token</param>
        /// <param name="formulario"></param>
        /// <returns></returns>
        public static HttpResponseMessage AceitarEdicao(string token, HttpContent formulario)
        {
            var URN = "/api/ChamadasEditadas/aceitar";
            var url = EndPoint + URN;
            var resultado = PostWithToken(url, formulario, token);
            return resultado;
        }
        /// <summary>
        /// Recusa uma Edicao
        /// </summary>
        /// <param name="token">jwt Token</param>
        /// <param name="formulario"></param>
        /// <returns></returns>
        public static HttpResponseMessage RecusarEdicao(string token, HttpContent formulario)
        {
            var URN = "/api/ChamadasEditadas/recusar";
            var url = EndPoint + URN;
            var resultado = PostWithToken(url, formulario, token);
            return resultado;
        }
        #endregion

        #region CancelarmentoRecusado
        /// <summary>
        /// Aceita Ediçao
        /// </summary>
        /// <param name="token">jwt Token</param>
        /// <param name="formulario"></param>
        /// <returns></returns>
        public static HttpResponseMessage CancelamentoRecusado(string token, HttpContent formulario)
        {
            var URN = "/api/CancelamentoChamada/recusar";
            var url = EndPoint + URN;
            var resultado = PostWithToken(url, formulario, token);
            return resultado;
        }
        #endregion

        #region ServerConfig
        /// <summary>
        /// Server Configuration
        /// </summary>
        /// <param name="token">jwt Token</param>      
        /// <returns></returns>
        public static HttpResponseMessage ServerConfig(string token)
        {
            var URN = "/api/serverconfig";
            var url = EndPoint + URN;
            var resultado = Get(url, token);
            return resultado;
        }
        #endregion

        #region MarketValues
        public static HttpResponseMessage GetMarketPriece(string token, string symbol)
        {
            var URN = "/api/marketvalue/GetMarketPriece?symbol=" + symbol;
            var url = EndPoint + URN;
            var resultado = Get(url, token);
            return resultado;
        }

        public static HttpResponseMessage GetBTCValue(string token)
        {
            var URN = "/api/marketvalue/GetBTCValue";
            var url = EndPoint + URN;
            var resultado = Get(url, token);
            return resultado;
        }

        public static HttpResponseMessage GetDollarValue(string token)
        {
            var URN = "/api/marketvalue/GetDollarValue";
            var url = EndPoint + URN;
            var resultado = Get(url, token);
            return resultado;
        }
        #endregion

        #region Relatorios
        public static HttpResponseMessage GetRelatorioIndividual(string token, DateTime datainicio, DateTime datafim)
        {
            var URN = "/api/relatorios/individual?dataInicio=" + datainicio.ToString() + "&dataFim=" + datafim.ToString();
            var url = EndPoint + URN;
            var resultado = Get(url, token);
            return resultado;
        }
        public static HttpResponseMessage GetRelatorioGeral(string token, DateTime datainicio, DateTime datafim)
        {
            var URN = "/api/relatorios/Geral?dataInicio=" + datainicio.ToString() + "&dataFim=" + datafim.ToString();
            var url = EndPoint + URN;
            var resultado = Get(url, token);
            return resultado;
        }
        #endregion



        #region methodos auxiliares
        private static HttpResponseMessage Get(string url, string token)
        {
            using (HttpClient _HttpClient = new HttpClient())
            {
                _HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                var resultado = _HttpClient.GetAsync(url).Result;
                return resultado;
            }
        }

        private static HttpResponseMessage Post(string url, HttpContent formulario)
        {
            using (HttpClient _HttpClient = new HttpClient())
            {
                var resultado = _HttpClient.PostAsync(url, formulario).Result;
                return resultado;
            }
        }

        private static HttpResponseMessage PostWithToken(string url, HttpContent formulario, string token)
        {
            using (HttpClient _HttpClient = new HttpClient())
            {
                _HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded");
                _HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                var resultado = _HttpClient.PostAsync(url, formulario).Result;
                return resultado;
            }
        }

        private static HttpResponseMessage DeleteWithToken(string url, string token)
        {
            using (HttpClient _HttpClient = new HttpClient())
            {
                _HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                var resultado = _HttpClient.DeleteAsync(url).Result;
                return resultado;
            }
        }
        #endregion
    }
}