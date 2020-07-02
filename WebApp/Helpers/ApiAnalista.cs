using DataAccess.Interfaces;
using DataAccess.Repository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace WebApp.Helpers
{
    public class ApiAnalista
    {
        public static string EndPoint;

        static ApiAnalista()
        {
            var _ServerConfigRepository = new ServerConfigRepository();
            EndPoint = _ServerConfigRepository.GetApiServer();
        }

        #region Chamadas
        public static HttpResponseMessage PostChamada(HttpContent formulario)
        {
            var URN = "/api/AnalistaChamada/";
            var url = EndPoint + URN;
            using (HttpClient _HttpClient = new HttpClient())
            {
                var resultado = _HttpClient.PostAsync(url, formulario).Result;
                return resultado;
            }
        }

        public static HttpResponseMessage EncerraChamadaAberta(int id)
        {
            var URN = "/api/AnalistaChamada/" + id;
            var url = EndPoint + URN;
            using (HttpClient _HttpClient = new HttpClient())
            {
                var resultado = _HttpClient.DeleteAsync(url).Result;
                return resultado;
            }
        }

        public static HttpResponseMessage EditarChamada(HttpContent formulario)
        {
            var URN = "/api/AnalistaChamada/EditarChamada";
            var url = EndPoint + URN;
            using (HttpClient _HttpClient = new HttpClient())
            {
                var resultado = _HttpClient.PostAsync(url, formulario).Result;
                return resultado;
            }
        }

        public static HttpResponseMessage EmailTrocaSMTP(HttpContent formulario)
        {
            var URN = "/api/ServerConfig/EmailTrocaSMTP";
            var url = EndPoint + URN;
            using (HttpClient _HttpClient = new HttpClient())
            {
                var resultado = _HttpClient.PostAsync(url, formulario).Result;
                return resultado;
            }
        }

        #endregion
    }
}