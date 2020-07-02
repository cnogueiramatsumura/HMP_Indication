using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class ConfirmoController : Controller
    {

        public string test()
        {
            var baseAddress = new Uri("https://confirmo.net/");
            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {
                httpClient.DefaultRequestHeaders.Add("Bearer", "QtQwYjJRLQMTTBtvKEPbhqJgYaYx7qQKU9z8QAhwF49irPa4l0AEDV8Oqm2xmxjk");
                using (var content = new StringContent("{  \"product\": {    \"description\": \"small silver computer\",    \"name\": \"Computer\"  },  \"invoice\": {    \"amount\": \"5.00\",    \"currencyFrom\": \"BRL\",    \"currencyTo\": \"BTC\"  },  \"settlement\": {    \"currency\": \"BRL\"  },  \"notifyEmail\": \"ccidhighwind@yahoo.com.br\",  \"notifyUrl\": \"\",  \"returnUrl\": \"\",  \"reference\": \"referenciajapa\"}", System.Text.Encoding.Default, "application/json"))
                {
                    using (var response = httpClient.PostAsync("api/v3/invoices", content).Result)
                    {
                        var responseData = response.Content.ReadAsStringAsync().Result;
                        return responseData;
                    }
                }
            }
        }
    }
}