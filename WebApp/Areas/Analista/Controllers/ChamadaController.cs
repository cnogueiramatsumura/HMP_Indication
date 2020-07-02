using DataAccess.Entidades;
using DataAccess.Helpers;
using DataAccess.Interfaces;
using DataAccess.Repository;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;
using WebApp.ActionFilters.Analista;
using WebApp.Areas.Analista.Models;
using WebApp.Helpers;
namespace WebApp.Areas.Analista.Controllers
{
    [AnalistaAuthorize(Roles = "Analista")]
    public class ChamadaController : Controller
    {
        private readonly IChamadasRepository _IChamadasRepo;
        private readonly IChamadaEditadaRepository _IChamadaEditadaRepo;       
        private readonly IOrdemRepository _IOrdemRepo;
        private readonly IServerConfigRepository _ServerConfigRepo;
        private readonly IAnalistaRepository _AnalistaRepository;

        public ChamadaController(IChamadasRepository ChamadasRepository, IChamadaEditadaRepository ChamadaEditadaRepository, IUsuarioRepository UsuarioRepository, IOrdemRepository OrdemRepository, IServerConfigRepository _ServerConfigRepository, IAnalistaRepository AnalistaRepository)
        {
            _IChamadasRepo = ChamadasRepository;
            _IChamadaEditadaRepo = ChamadaEditadaRepository;          
            _IOrdemRepo = OrdemRepository;
            _ServerConfigRepo = _ServerConfigRepository;
            _AnalistaRepository = AnalistaRepository;           
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(ChamadaViewModel model)
        {
            if (ModelState.IsValid)
            {
                var analista = _AnalistaRepository.GetByEmail(User.Identity.Name);
                HttpContent form = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("SymbolDescription",model.SymbolDescription ),
                    new KeyValuePair<string, string>("SymbolName",model.SymbolName ),
                    new KeyValuePair<string, string>("PrecoEntrada",model.PrecoEntrada.ToString("F8")),
                    new KeyValuePair<string, string>("RangeEntrada",model.RangeEntrada.ToString("F8")),
                    new KeyValuePair<string, string>("PrecoGain", model.PrecoGain.ToString("F8")),
                    new KeyValuePair<string, string>("PrecoLoss",model.PrecoLoss.ToString("F8")),
                    new KeyValuePair<string, string>("Observacao", model.Observacao),
                    new KeyValuePair<string, string>("PercentualIndicado", model.PercentualIndicado),
                    new KeyValuePair<string, string>("Analista_Id", analista.Id.ToString()),
                });
                var res = ApiAnalista.PostChamada(form);
                if (res.IsSuccessStatusCode)
                {                  
                    return RedirectToAction("abertas");
                }
                var apierror = res.Content.ReadAsStringAsync().Result;
                ModelState.AddModelError("apierro", apierror);
            }
            return View(model);
        }

        public ActionResult Abertas()
        {
            var userid = _AnalistaRepository.GetByEmail(User.Identity.Name).Id;
            var config = _ServerConfigRepo.GetAllConfig();
            ViewBag.ApiDomainName = config.ApiServer;
            var OpenList = _IChamadasRepo.AnalistaCancelOrEdit(userid);
            return View(OpenList);           
        }

        [HttpPost]
        public MyJsonResult CancelarAbertas(int id)
        {
            var res = ApiAnalista.EncerraChamadaAberta(id);
            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                var chamada = JsonConvert.DeserializeObject<Chamada>(result);             
                return new MyJsonResult(result, JsonRequestBehavior.DenyGet);
            }
            else
            {
                Response.TrySkipIisCustomErrors = true;
                Response.StatusCode = (int)res.StatusCode;
                return new MyJsonResult(res, JsonRequestBehavior.DenyGet);
            }
        }

        public ActionResult EditarChamada(int chamadaId)
        {
            var chamada = _IChamadasRepo.GetWith_Symbol(chamadaId);
            if (chamada == null || chamada.ChamadaStatus_Id != 2)
            {
                return View("EdicaoFinalizada");
            }

            var ListaEdicoes = _IChamadaEditadaRepo.GetListEdit(chamadaId);
            if (ListaEdicoes.Count > 0)
            {
                var viewmodel = new EditChamadasViewModel { Chamada_Id = chamada.Id, symbol = chamada.Symbol.symbol, Entrada = chamada.PrecoEntrada, Gain = ListaEdicoes.LastOrDefault().NewGain, Loss = ListaEdicoes.LastOrDefault().NewLoss, ListaEdicoes = ListaEdicoes };
                return View(viewmodel);
            }
            else
            {
                var viewmodel = new EditChamadasViewModel { Chamada_Id = chamada.Id, symbol = chamada.Symbol.symbol, Entrada = chamada.PrecoEntrada, Gain = chamada.PrecoGain, Loss = chamada.PrecoLoss, ListaEdicoes = new List<ChamadaEditada>() };
                return View(viewmodel);
            }
        }

        [HttpPost]
        public ActionResult EditarChamada(EditChamadasViewModel model)
        {
            if (ModelState.IsValid)
            {
                HttpContent form = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("Chamada_Id",model.Chamada_Id.ToString()),
                    new KeyValuePair<string, string>("Entrada",model.Entrada.ToString("F8")),
                    new KeyValuePair<string, string>("NewGain",model.NewGain.ToString("F8")),
                    new KeyValuePair<string, string>("NewLoss",model.NewLoss.ToString("F8")),
                    new KeyValuePair<string, string>("symbol",model.symbol)
                });
                var res = ApiAnalista.EditarChamada(form);
                if (res.IsSuccessStatusCode)
                {                
                    return RedirectToAction("Abertas");
                }
            }
            var ListaEdicoes = _IChamadaEditadaRepo.GetListEdit(model.Chamada_Id);
            model.ListaEdicoes = ListaEdicoes;
            return View(model);
        }

        public ActionResult Posicionados(int chamadaId)
        {
            var chamada = _IChamadasRepo.RelatorioAceitaramAChamada(chamadaId);
            return View(chamada);
        }
    }
}