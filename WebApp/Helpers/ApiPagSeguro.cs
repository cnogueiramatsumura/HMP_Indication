using DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using DataAccess.Entidades;
using Newtonsoft.Json;
using System.Text;

namespace WebApp.Helpers
{
    public class ApiPagSeguro
    {
        private readonly string _baseUri;
        private readonly string token;
        private readonly string redirectUrl;
        private readonly string notificationURL;
        public string pagseguroUrl;

        public ApiPagSeguro(string ApiToken, string WebAppDomain, bool useSandbox = true)
        {
            token = useSandbox ? "62D9286AE7FF4F6A8DBC3227F0DD9827" : ApiToken;
            _baseUri = useSandbox ? "https://ws.sandbox.pagseguro.uol.com.br/" : "https://ws.pagseguro.uol.com.br/";
            redirectUrl = useSandbox ? "localhost/usuario/pagamentos/check" : WebAppDomain + "/usuario/pagamentos/check";
            notificationURL = WebAppDomain + "/usuario/pagamentos/pagseguroSuccesso";
            pagseguroUrl = useSandbox ? "http://sandbox.pagseguro.uol.com.br/v2/checkout/payment.html?code=" : "http://pagseguro.uol.com.br/v2/checkout/payment.html?code=";
        }

        public HttpResponseMessage GeneratePayment(int PagamentoLicencaId, decimal Preco)
        {
            var response = CreateOrder(PagamentoLicencaId, Preco);
            return response;
        }

        /// <summary>
        /// Create an order https://developer.coingate.com/docs/create-order
        /// </summary>
        /// <param name="dto">The order object</param>
        /// <param name="resourcePath"></param>
        /// <returns></returns>
        public HttpResponseMessage CreateOrder(int PagamentoId, decimal Preco, string resourcePath = "v2/checkout/")
        {
            //email é o do dono do pagseguro
            var body = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string,string>("email","cnogueiramatsumura@yahoo.com.br"),
                new KeyValuePair<string,string>("token",token),
                new KeyValuePair<string,string>("currency","BRL"),
                new KeyValuePair<string,string>("reference",PagamentoId.ToString()),
                new KeyValuePair<string,string>("itemId1","1"),
                new KeyValuePair<string,string>("itemDescription1","Licença CriptoStorm"),
                new KeyValuePair<string,string>("itemAmount1",Preco.ToString()),
                new KeyValuePair<string,string>("itemQuantity1","1"),
                new KeyValuePair<string,string>("redirectURL",redirectUrl),
                new KeyValuePair<string,string>("notificationURL",notificationURL)
            });

            using (HttpClient _HttpClient = new HttpClient())
            {
                _HttpClient.Timeout = new TimeSpan(0, 0, 10);
                var resultado = _HttpClient.PostAsync(_baseUri + resourcePath, body).Result;
                return resultado;
            }
        }

        public HttpResponseMessage CheckPayment(string notificationCode)
        {
            //email e token sao do dono do pagseguro
            string resourcePath = "v3/transactions/notifications/" + notificationCode + "?email=cnogueiramatsumura@yahoo.com.br&token=" + token;
            using (HttpClient _HttpClient = new HttpClient())
            {
                _HttpClient.Timeout = new TimeSpan(0, 0, 10);
                var resultado = _HttpClient.GetAsync(_baseUri + resourcePath).Result;
                return resultado;
            }
        }
    }

    public class PagSeguroOrdem
    {
        public string currency { get; set; }
        public string email { get; set; }
        public string token { get; set; }

    }

    public class checkout
    {
        public string code { get; set; }
        public DateTime date { get; set; }
    }

    public class transaction
    {
        public DateTime date { get; set; }
        public string code { get; set; }
        public int reference { get; set; }
        public int status { get; set; }
        public decimal netAmount { get; set; }
    }

}