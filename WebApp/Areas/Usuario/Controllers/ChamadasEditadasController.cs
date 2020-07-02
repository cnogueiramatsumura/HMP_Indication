using DataAccess.Entidades;
using DataAccess.Interfaces;
using DataAccess.Serialized_Objects;
using DataAccess.ViewModels;
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
    public class ChamadasEditadasController : MyBaseController
    {
        public MyJsonResult aceitar(AceitarEdicaoViewModel model)
        {
            var token = Session["token"];
            HttpContent form = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("EdicaoId",model.EdicaoId.ToString()),
                new KeyValuePair<string,string>("ChamadaId",model.ChamadaId.ToString())
            });
            var res = ApiUsuario.AceitarEdicao(token.ToString(), form);
            if (res.IsSuccessStatusCode)
            {
                var ret = res.Content.ReadAsStringAsync().Result;
                var retObj = JsonConvert.DeserializeObject<SerializeEdicaoAceita>(ret);
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

        public MyJsonResult recusar(AceitarEdicaoViewModel model)
        {
            var token = Session["token"];
            HttpContent form = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("EdicaoId",model.EdicaoId.ToString()),
                 new KeyValuePair<string,string>("ChamadaID",model.ChamadaId.ToString())
            });
            var res = ApiUsuario.RecusarEdicao(token.ToString(), form);
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