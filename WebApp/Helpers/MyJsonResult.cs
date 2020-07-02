using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Helpers
{
    public class MyJsonResult : JsonResult
    {
        private static readonly JsonSerializerSettings settings = new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,        
            ContractResolver = new CamelCasePropertyNamesContractResolver()
            
        };

        public MyJsonResult(object data)
        {
            Data = data;
            this.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            settings.Converters.Add(new IsoDateTimeConverter());
        }

        public MyJsonResult(object data, JsonRequestBehavior behavior)
        {
            Data = data;
            this.JsonRequestBehavior = behavior;
            settings.Converters.Add(new IsoDateTimeConverter());
        }

        public override void ExecuteResult(ControllerContext context)
        {

            if (this.JsonRequestBehavior == JsonRequestBehavior.DenyGet && string.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("GET request not allowed");
            }
            var response = context.HttpContext.Response;
            response.ContentType = "application/json";
            if (ContentEncoding != null)
            {
                response.ContentEncoding = ContentEncoding;
            }

            if (Data != null)
            {
                response.Write(JsonConvert.SerializeObject(Data, settings));
            }
        }
    }
}