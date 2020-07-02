using DataAccess.Entidades;
using DataAccess.Helpers;
using DataAccess.Interfaces;
using DataAccess.Repository;
using DataAccess.Serialized_Objects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WebApp.ActionFilters;
using WebApp.Helpers;

namespace WebApp.Areas.Usuario.Controllers
{    
   
    [UsuarioAuthorize]
    public class OrdemsController : MyBaseController
    {      
        public MyJsonResult GetOrdembyChamadaID(int chamadaId)
        {
            var token = Session["token"].ToString();
            var result = ApiUsuario.GetOrdemByChamadaID(token, chamadaId);
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var ret = result.Content.ReadAsStringAsync().Result;
                var ordem = JsonConvert.DeserializeObject<Ordem>(ret);
                return new MyJsonResult(ordem, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return new MyJsonResult(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]        
        public MyJsonResult CancelarEntrada(int OrderID)
        {
            var token = Session["token"].ToString();
            var res = ApiUsuario.CancelarEntrada(token, OrderID);
            if (res.StatusCode == HttpStatusCode.OK)
            {
                var ret = res.Content.ReadAsStringAsync().Result;
                var ordem = JsonConvert.DeserializeObject<Ordem>(ret);
                return new MyJsonResult(ordem, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var result = res.Content.ReadAsStringAsync().Result;
                Response.TrySkipIisCustomErrors = true;
                Response.StatusCode = (int)res.StatusCode;
                return new MyJsonResult(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]      
        public MyJsonResult VenderaMercado(int OrderID)
        {
            var token = Session["token"].ToString();
            var result = ApiUsuario.SairMercado(token, OrderID);
            if (result.StatusCode == HttpStatusCode.OK)
            {
                var ret = result.Content.ReadAsStringAsync().Result;
                var ordem = JsonConvert.DeserializeObject<Ordem>(ret);
                return new MyJsonResult(ordem, JsonRequestBehavior.AllowGet);
            }
            else
            {          
                Response.TrySkipIisCustomErrors = true;
                Response.StatusCode = (int)result.StatusCode;
                return new MyJsonResult(result.Content.ReadAsStringAsync().Result, JsonRequestBehavior.AllowGet);
            }
        }
    
        public MyJsonResult Ativas()
        {
            try
            {
                var token = Session["token"];
                var res = ApiUsuario.GetOrdemsAtivas(token.ToString());
                if (res.StatusCode == HttpStatusCode.OK)
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    var ret = JsonConvert.DeserializeObject<List<ActionOrders>>(result);
                    return new MyJsonResult(ret, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    Response.TrySkipIisCustomErrors = true;
                    Response.StatusCode = (int)res.StatusCode; ;
                    return new MyJsonResult(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Response.TrySkipIisCustomErrors = true;
                Response.StatusCode = 500;
                return new MyJsonResult(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
      
        public MyJsonResult Finalizadas()
        {
            try
            {
                var token = Session["token"];
                var res = ApiUsuario.GetOrdemsFinalizadas(token.ToString());
                if (res.StatusCode == HttpStatusCode.OK)
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    var ret = JsonConvert.DeserializeObject<List<ActionOrders>>(result);
                    return new MyJsonResult(ret, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    Response.TrySkipIisCustomErrors = true;
                    Response.StatusCode = (int)res.StatusCode; ;
                    return new MyJsonResult(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Response.TrySkipIisCustomErrors = true;
                Response.StatusCode = 500;
                return new MyJsonResult(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
      
        public MyJsonResult Canceladas()
        {
            try
            {
                var token = Session["token"];
                var res = ApiUsuario.GetOrdemsCanceladas(token.ToString());
                if (res.StatusCode == HttpStatusCode.OK)
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    var ret = JsonConvert.DeserializeObject<List<ActionOrders>>(result);
                    return new MyJsonResult(ret, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    Response.TrySkipIisCustomErrors = true;
                    Response.StatusCode = (int) res.StatusCode;
                    return new MyJsonResult(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Response.TrySkipIisCustomErrors = true;
                Response.StatusCode = 500;
                return new MyJsonResult(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
    }
}


