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
    public class MarketValueController : MyBaseController
    {
        public MyJsonResult GetMarketPriece(string symbol)
        {
            try
            {
                var token = Session["token"].ToString();
                var res = ApiUsuario.GetMarketPriece(token,symbol.ToUpper());
                if (res.IsSuccessStatusCode)
                {
                    var result = res.Content.ReadAsStringAsync().Result;                  
                    return new MyJsonResult(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    Response.TrySkipIisCustomErrors = true;
                    Response.StatusCode = (int)res.StatusCode;
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
        public MyJsonResult GetBTCValue()
        {
            try
            {
                var token = Session["token"].ToString();
                var ret = ApiUsuario.GetBTCValue(token);
                if (ret.IsSuccessStatusCode)
                {
                    var result = ret.Content.ReadAsStringAsync().Result;
                    return new MyJsonResult(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var result = ret.Content.ReadAsStringAsync().Result;
                    Response.TrySkipIisCustomErrors = true;
                    Response.StatusCode = (int)ret.StatusCode;
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
        public MyJsonResult GetDollarValue()
        {
            try
            {
                var token = Session["token"].ToString();
                var ret = ApiUsuario.GetDollarValue(token);
                if (ret.IsSuccessStatusCode)
                {
                    var result = ret.Content.ReadAsStringAsync().Result;
                    return new MyJsonResult(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var result = ret.Content.ReadAsStringAsync().Result;
                    Response.TrySkipIisCustomErrors = true;
                    Response.StatusCode = (int)ret.StatusCode;
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