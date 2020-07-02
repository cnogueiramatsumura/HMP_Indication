using DataAccess.Entidades;
using DataAccess.Helpers;
using DataAccess.Interfaces;
using DataAccess.Serialized_Objects;
using DataAccess.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WebApp.ActionFilters;
using WebApp.Areas.Analista.Models;
using WebApp.Areas.Usuario.Models.Serialized_Objects;
using WebApp.Helpers;


namespace WebApp.Areas.Usuario.Controllers
{

    [UsuarioAuthorize]
    public class ChamadaController : MyBaseController
    {        
        private readonly IChamadasRepository _ChamadasRepo;
        public ChamadaController(IChamadasRepository ChamadasRepository)
        {          
            _ChamadasRepo = ChamadasRepository;
        }

        [HttpPost]
        public MyJsonResult recusarchamada(int id)
        {
            try
            {
                var token = Session["token"];
                var res = ApiUsuario.RecusarChamada(token.ToString(), id);
                if (res.StatusCode == HttpStatusCode.OK)
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    var ret = JsonConvert.DeserializeObject<ChamadasRecusadas>(result);
                    return new MyJsonResult(ret, JsonRequestBehavior.DenyGet);
                }
                else
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    Response.TrySkipIisCustomErrors = true;
                    Response.StatusCode = (int) res.StatusCode;
                    return new MyJsonResult(result, JsonRequestBehavior.DenyGet);
                }
            }
            catch (Exception ex)
            {
                Response.TrySkipIisCustomErrors = true;
                Response.StatusCode = 500;
                return new MyJsonResult(ex.Message, JsonRequestBehavior.DenyGet);
            }
        }
        [HttpPost]
        public MyJsonResult aceitarchamada(int id, decimal qtd)
        {
            try
            {
                var token = Session["token"];
                HttpContent form = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string,string>("id",id.ToString()),
                    new KeyValuePair<string,string>("qtd",qtd.ToString())
                });
                var res = ApiUsuario.Postchamada(token.ToString(), form);
                if (res.StatusCode == HttpStatusCode.OK)
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    var ret = JsonConvert.DeserializeObject<AnonimousOrder>(result);
                    return new MyJsonResult(ret, JsonRequestBehavior.DenyGet);
                }
                else
                {
                    //aterar o retorno aqui para uma msg de erro avaliar
                    var result = res.Content.ReadAsStringAsync().Result;
                    Response.TrySkipIisCustomErrors = true;
                    Response.StatusCode = (int)res.StatusCode;
                    return new MyJsonResult(result, JsonRequestBehavior.DenyGet);
                }
            }
            catch (Exception ex)
            {
                Response.TrySkipIisCustomErrors = true;
                Response.StatusCode = 500;
                return new MyJsonResult(new { error = ex.Message }, JsonRequestBehavior.DenyGet);
            }
        }

        public MyJsonResult validateLotSize(int id, decimal qtd)
        {
            var chamada = _ChamadasRepo.GetWith_Symbol_and_Filter(id);
            var obj = new FilterLotSize()
            {
                isValid = true,
                msg = ""
            };

            string FilterError;
            if (!BinanceHelper.IsValidateLotSizeFIlter(out FilterError, chamada, qtd))
            {
                obj.isValid = false;
                obj.msg = FilterError;
                return new MyJsonResult(obj, JsonRequestBehavior.AllowGet);
            }
            return new MyJsonResult(obj, JsonRequestBehavior.AllowGet);
        }

        public MyJsonResult Ativas()
        {
            try
            {
                var token = Session["token"];
                var res = ApiUsuario.GetchamadaAtivas(token.ToString());
                if (res.StatusCode == HttpStatusCode.OK)
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    var ret = JsonConvert.DeserializeObject<ChamadasAtivasViewModel>(result);
                    return new MyJsonResult(ret, JsonRequestBehavior.AllowGet);
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
        public MyJsonResult Encerradas()
        {
            try
            {
                var token = Session["token"];
                var res = ApiUsuario.GetchamadaEncerradas(token.ToString());
                if (res.StatusCode == HttpStatusCode.OK)
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    var ret = JsonConvert.DeserializeObject<List<Chamada>>(result);
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
        public MyJsonResult Recusadas()
        {
            try
            {
                var token = Session["token"];
                var res = ApiUsuario.GetchamadaRecusadas(token.ToString());
                if (res.StatusCode == HttpStatusCode.OK)
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    var ret = JsonConvert.DeserializeObject<List<Chamada>>(result);
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
    }

}