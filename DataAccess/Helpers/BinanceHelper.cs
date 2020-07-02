using DataAccess.Entidades;
using DataAccess.Serialized_Objects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace DataAccess.Helpers
{
    public class BinanceHelper
    {
        public static SymbolTicker getMarketValue(string Symbol)
        {
            var url = "https://api.binance.com/api/v3/ticker/price?";
            var querystring = "symbol=" + Symbol;
            using (HttpClient _HttpClient = new HttpClient())
            {
                var resultado = _HttpClient.GetAsync(url + querystring).Result;
                var content = resultado.Content.ReadAsStringAsync().Result;
                var symbolTicker = JsonConvert.DeserializeObject<SymbolTicker>(content);
                return symbolTicker;
            }
        }

        public static SymbolTicker getBTCMarketValue()
        {
            var url = "https://api.binance.com/api/v3/ticker/price?";
            var querystring = "symbol=BTCUSDT";
            using (HttpClient _HttpClient = new HttpClient())
            {
                var resultado = _HttpClient.GetAsync(url + querystring).Result;
                var content = resultado.Content.ReadAsStringAsync().Result;
                var symbolTicker = JsonConvert.DeserializeObject<SymbolTicker>(content);
                return symbolTicker;
            }
        }

        public static Currency getDollarValue()
        {           
            var url = "https://www.binance.com/exchange-api/v1/public/asset-service/product/currency";
            using (HttpClient _HttpClient = new HttpClient())
            {
                var resultado = _HttpClient.GetAsync(url).Result;
                var content = resultado.Content.ReadAsStringAsync().Result;
                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<Currency>(content);
                return obj; 
            }
        }

        public static string getSignature(string queryString, string apiSecret)
        {
            //apiSecret
            ASCIIEncoding enc = new ASCIIEncoding();
            var key = enc.GetBytes(apiSecret);
            //parametro do hmac é a chave do usuarioa da binance
            using (var Hmac = new HMACSHA256(key))
            {
                byte[] messageBytes = enc.GetBytes(queryString);
                var computedHash = Hmac.ComputeHash(messageBytes);
                var result = BitConverter.ToString(computedHash).Replace("-", "");
                return result;
            }
        }

        //public async static Task<symbols> getExchangeSymbol(string symbol)
        //{
        //    var url = "https://api.binance.com/api/v1/exchangeInfo";
        //    using (HttpClient _HttpClient = new HttpClient())
        //    {
        //        var resultado = _HttpClient.GetAsync(url).Result;
        //        var content = await resultado.Content.ReadAsStringAsync();
        //        var exchangeInfo = JsonConvert.DeserializeObject<ExchangeInfo>(content);
        //        var exchangeSymbol = exchangeInfo.symbols.Where(x => x.symbol == symbol).FirstOrDefault();
        //        return exchangeSymbol;
        //    }
        //}

        //public static filters getExchangeFilter(symbols symbol, FilterType filterType)
        //{
        //    return symbol.filters.Where(x => x.filterType == filterType.ToString()).FirstOrDefault();
        //}

        // Tipos de filtros a serem adicionados para seleção



        public static string getEntradaOrderType(Chamada chamada)
        {
            if (chamada.PrecoEntrada <= chamada.PrecoMercadoHoraChamada)
            {
                return "TAKE_PROFIT_LIMIT";
            }
            else
            {
                return "STOP_LOSS_LIMIT";
            }
        }

        public static bool IsValidateLotSizeFIlter(out string filterEror, Chamada chamada, decimal qtd)
        {
            var LotSizeFIlter = chamada.Symbol.filters.Where(x => x.filterType == "LOT_SIZE").FirstOrDefault();
            var res = ((qtd - LotSizeFIlter.minQty) % LotSizeFIlter.stepSize == 0) ? true : false;
            filterEror = "";
            if (res == false)
            {
                if (LotSizeFIlter.stepSize == 1)
                {
                    filterEror = "A Quantidade precisa ser um numero inteiro ";
                }
                else
                {
                    filterEror = "A Quantidade precisa ser um numero multiplo de " + LotSizeFIlter.stepSize.ToString().TrimEnd('0');
                }
            }
            return res;
        }


        public static bool ValidateFilterPrice(filters filtro, decimal quantidade)
        {
            var digitleng = Math.Round(filtro.tickSize * 100000000).ToString().Length;
            var arrayqtd = quantidade.ToString().Split('.');
            var digitoqtd = arrayqtd.Length > 1 ? arrayqtd[1].Length : 0;
            if ((digitleng == 1 && digitoqtd > 0) || (digitoqtd > digitleng))
            {
                return false;
            }
            return true;
        }

    }
}