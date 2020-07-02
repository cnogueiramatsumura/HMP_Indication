using DataAccess.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.OutputCache.V2;

namespace WebAPI.Controllers
{
    [Authorize]
    public class MarketValueController : ApiController
    {       
        [HttpGet]
        [Route("api/marketvalue/GetMarketPriece")]
        public HttpResponseMessage GetMarketPriece(string symbol)
        {
            try
            {
                var priece = BinanceHelper.getMarketValue(symbol.ToUpper()).price;
                return Request.CreateResponse(HttpStatusCode.OK, priece);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [CacheOutput(ServerTimeSpan = 30)]
        [HttpGet]
        [Route("api/marketvalue/GetBTCValue")]
        public HttpResponseMessage GetBTCValue()
        {
            try
            {
                var priece = BinanceHelper.getBTCMarketValue().price;
                return Request.CreateResponse(HttpStatusCode.OK, decimal.Round(priece,2,MidpointRounding.AwayFromZero));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [CacheOutput(ServerTimeSpan = 300)]
        [HttpGet]
        [Route("api/marketvalue/GetDollarValue")]
        public HttpResponseMessage GetDollarValue()
        {
            try
            {
                var currencies = BinanceHelper.getDollarValue();
                var BRL = currencies.data.Where(x => x.pair == "BRL_USD").FirstOrDefault();
                return Request.CreateResponse(HttpStatusCode.OK, decimal.Round(BRL.rate, 2, MidpointRounding.AwayFromZero));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
