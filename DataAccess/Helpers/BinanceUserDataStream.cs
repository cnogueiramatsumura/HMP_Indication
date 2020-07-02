using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Helpers
{
    public class BinanceUserDataStream
    {
        public static HttpResponseMessage GetListenKey(string apiKey, string apiSecret)
        {
            var url = "https://api.binance.com/api/v3/userDataStream";
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
            //var timestamp = GetServerTime();
            var param = "recvWindow=60000&timestamp=" + timestamp;
            var hmacsignature = BinanceHelper.getSignature(param, apiSecret);
            using (HttpClient _HttpClient = new HttpClient())
            {
                _HttpClient.DefaultRequestHeaders.Add("X-MBX-APIKEY", apiKey);
                var resultado = _HttpClient.PostAsync(url,null).Result;
                return resultado;
            }
        }

        public static HttpResponseMessage CloseStream(string apiKey, string apiSecret)
        {
            var res = GetListenKey(apiKey, apiSecret);
            var listenkey = (string)JsonConvert.DeserializeObject<dynamic>(res.Content.ReadAsStringAsync().Result).listenKey;         

            var url = "https://api.binance.com/api/v3/userDataStream?";
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();           
            var querystring = "listenKey=" + listenkey;         
            using (HttpClient _HttpClient = new HttpClient())
            {
                _HttpClient.DefaultRequestHeaders.Add("X-MBX-APIKEY", apiKey);
                var resultado = _HttpClient.DeleteAsync(url + querystring).Result;
                return resultado;
            }
        }

        public static HttpResponseMessage KeepAlive(string apiKey, string apiSecret)
        {
            var res = GetListenKey(apiKey, apiSecret);
            var listenkey = (string)JsonConvert.DeserializeObject<dynamic>(res.Content.ReadAsStringAsync().Result).listenKey;
            var url = "https://api.binance.com/api/v3/userDataStream?";
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
            HttpContent content = new FormUrlEncodedContent(new []
            {
                new KeyValuePair<string,string>("listenKey",listenkey)
            });
            using (HttpClient _HttpClient = new HttpClient())
            {
                _HttpClient.DefaultRequestHeaders.Add("X-MBX-APIKEY", apiKey);
                var resultado = _HttpClient.PutAsync(url, content).Result;
                return resultado;
            }
        }
    }
}
