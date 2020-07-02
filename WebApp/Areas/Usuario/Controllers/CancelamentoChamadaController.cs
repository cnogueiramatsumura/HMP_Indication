using DataAccess.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WebApp.ActionFilters;
using WebApp.Helpers;

namespace WebApp.Areas.Usuario.Controllers
{
    [UsuarioAuthorize]
    public class CancelamentoChamadaController : MyBaseController
    {        
        public MyJsonResult recusar(int CancelamentoChamada_ID)
        {
            var token = Session["token"];
            HttpContent form = new FormUrlEncodedContent(new[]
            {
                 new KeyValuePair<string,string>("CancelamentoChamada_ID", CancelamentoChamada_ID.ToString())
            });
            var res = ApiUsuario.CancelamentoRecusado(token.ToString(), form);
            if (res.IsSuccessStatusCode)
            {
                var ret = res.Content.ReadAsStringAsync().Result;
                var retObj = JsonConvert.DeserializeObject<dynamic>(ret);
                return new MyJsonResult(retObj, JsonRequestBehavior.DenyGet);
            }
            else
            {
                var result = res.Content.ReadAsStringAsync().Result;
                Response.TrySkipIisCustomErrors = true;
                Response.StatusCode = (int)res.StatusCode;
                return new MyJsonResult(result, JsonRequestBehavior.DenyGet);
            }
        }
    }
}