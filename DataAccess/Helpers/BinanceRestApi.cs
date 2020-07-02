using Newtonsoft.Json;
using System;
using System.Net.Http;

namespace DataAccess.Helpers
{
    public class BinanceRestApi
    {
        public static HttpResponseMessage GetExchangeInfo()
        {
            var url = "https://api.binance.com/api/v1/exchangeInfo";
            using (HttpClient _HttpClient = new HttpClient())
            {
                var resultado = _HttpClient.GetAsync(url).Result;
                return resultado;
            }
        }

        public static HttpResponseMessage SymbolsPriece()
        {
            var url = "https://api.binance.com/api/v3/ticker/price";
            using (HttpClient _HttpClient = new HttpClient())
            {
                var resultado = _HttpClient.GetAsync(url).Result;
                return resultado;
            }
        }

        public static HttpResponseMessage GetAccountInformation(string apiKey, string apiSecret)
        {
            var url = "https://api.binance.com/api/v3/account?";
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
            var querystring = "timestamp=" + timestamp;
            var hmacsignature = BinanceHelper.getSignature(querystring, apiSecret);

            using (HttpClient _HttpClient = new HttpClient())
            {
                _HttpClient.DefaultRequestHeaders.Add("X-MBX-APIKEY", apiKey);
                var resultado = _HttpClient.GetAsync(url + querystring + "&signature=" + hmacsignature).Result;
                return resultado;
            }
        }

        public static HttpResponseMessage SendOrdemEntrada(string apiKey, string apiSecret, string OrderType, string symbol, decimal quantidade, decimal Priece, decimal LimitPriece, string OrdemId)
        {
            var url = "https://api.binance.com/api/v3/order?";
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
            var querystring = "symbol=" + symbol.ToUpper() + "&side=BUY&type=" + OrderType + "&quantity=" + quantidade.ToString() + "&recvWindow=60000&timestamp=" + timestamp + "&newClientOrderId=" + OrdemId + "&newOrderRespType=RESULT&timeInForce=FOK&price=" + LimitPriece.ToString() + "&stopPrice=" + Priece.ToString();
            var hmacsignature = BinanceHelper.getSignature(querystring, apiSecret);          
            using (HttpClient _HttpClient = new HttpClient())
            {
                _HttpClient.DefaultRequestHeaders.Add("X-MBX-APIKEY", apiKey);
                var resultado = _HttpClient.PostAsync(url + querystring + "&signature=" + hmacsignature, null).Result;
                return resultado;
            }
        }

        public static HttpResponseMessage CancelarEntrada(string apiKey, string apiSecret, string symbol, string orderId)
        {
            var url = "https://api.binance.com/api/v3/order?";
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
            var querystring = "symbol=" + symbol.ToUpper() + "&recvWindow=60000&timestamp=" + timestamp + "&origClientOrderId=" + orderId;
            var hmacsignature = BinanceHelper.getSignature(querystring, apiSecret);

            using (HttpClient _HttpClient = new HttpClient())
            {
                _HttpClient.DefaultRequestHeaders.Add("X-MBX-APIKEY", apiKey);
                var resultado = _HttpClient.DeleteAsync(url + querystring + "&signature=" + hmacsignature).Result;
                return resultado;
            }
        }

        public static HttpResponseMessage CancelarOco(string apiKey, string apiSecret, string symbol, string orderListId)
        {
            var url = "https://api.binance.com/api/v3/orderList?";
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
            var querystring = "symbol=" + symbol.ToUpper() + "&recvWindow=60000&timestamp=" + timestamp + "&listClientOrderId=" + orderListId;
            var hmacsignature = BinanceHelper.getSignature(querystring, apiSecret);

            using (HttpClient _HttpClient = new HttpClient())
            {
                _HttpClient.DefaultRequestHeaders.Add("X-MBX-APIKEY", apiKey);
                var resultado = _HttpClient.DeleteAsync(url + querystring + "&signature=" + hmacsignature).Result;
                return resultado;
            }
        }

        public static HttpResponseMessage VenderMercado(string apiKey, string apiSecret, string symbol, decimal quantidade, string OrdemId)
        {
            var url = "https://api.binance.com/api/v3/order?";
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
            var querystring = "symbol=" + symbol.ToUpper() + "&side=SELL&type=MARKET&quantity=" + quantidade.ToString() + "&recvWindow=60000&timestamp=" + timestamp + "&newClientOrderId=" + OrdemId;
            var hmacsignature = BinanceHelper.getSignature(querystring, apiSecret);

            using (HttpClient _HttpClient = new HttpClient())
            {
                _HttpClient.DefaultRequestHeaders.Add("X-MBX-APIKEY", apiKey);
                var resultado = _HttpClient.PostAsync(url + querystring + "&signature=" + hmacsignature, null).Result;
                return resultado;
            }
        }
        public static HttpResponseMessage SendSaidaOco(string apiKey, string apiSecret, string symbol, decimal quantidade, decimal Gain, decimal Loss, decimal LimitLoss, string LimitOrdemId, string StopOrderID)
        {
            var url = "https://api.binance.com/api/v3/order/oco?";
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
            var querystring = "symbol=" + symbol.ToUpper() + "&side=SELL&quantity=" + quantidade.ToString() + "&stopClientOrderId=" + StopOrderID + "&limitClientOrderId=" + LimitOrdemId + "&recvWindow=60000&timestamp=" + timestamp + "&stopLimitTimeInForce=GTC&price=" + Gain.ToString() + "&stopPrice=" + Loss.ToString() + "&stopLimitPrice=" + LimitLoss.ToString();
            var hmacsignature = BinanceHelper.getSignature(querystring, apiSecret);         
            using (HttpClient _HttpClient = new HttpClient())
            {
                _HttpClient.DefaultRequestHeaders.Add("X-MBX-APIKEY", apiKey);
                var resultado = _HttpClient.PostAsync(url + querystring + "&signature=" + hmacsignature, null).Result;
                return resultado;
            }
        }

        public static HttpResponseMessage ComprarBNB(string apiKey, string apiSecret, decimal quantidade)
        {
            var url = "https://api.binance.com/api/v3/order?";
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
            var querystring = "symbol=BNBBTC&side=BUY&type=MARKET&quantity=" + quantidade.ToString() + "&recvWindow=60000&timestamp=" + timestamp;
            var hmacsignature = BinanceHelper.getSignature(querystring, apiSecret);
            using (HttpClient _HttpClient = new HttpClient())
            {
                _HttpClient.DefaultRequestHeaders.Add("X-MBX-APIKEY", apiKey);
                var resultado = _HttpClient.PostAsync(url + querystring + "&signature=" + hmacsignature, null).Result;
                return resultado;
            }
        }


        public static HttpResponseMessage RecentTraders(string apiKey, string apiSecret, string symbol)
        {
            var url = "https://api.binance.com/api/v3/allOrders?";
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
            var querystring = "symbol=" + symbol.ToUpper() + "&timestamp=" + timestamp; ;
            var hmacsignature = BinanceHelper.getSignature(querystring, apiSecret);

            using (HttpClient _HttpClient = new HttpClient())
            {
                _HttpClient.DefaultRequestHeaders.Add("X-MBX-APIKEY", apiKey);
                var resultado = _HttpClient.GetAsync(url + querystring + "&signature=" + hmacsignature).Result;
                return resultado;
            }
        }   
    }
}