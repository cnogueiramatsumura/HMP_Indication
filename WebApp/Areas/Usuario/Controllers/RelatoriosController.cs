using DataAccess.Entidades;
using DataAccess.Helpers;
using DataAccess.Interfaces;
using DataAccess.Serialized_Objects;
using DataAccess.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApp.ActionFilters;

namespace WebApp.Areas.Usuario.Controllers
{
    [UsuarioAuthorize]
    public class RelatoriosController : MyBaseController
    {
        [LoadViewBags]       
        public ActionResult Individual(DateTime? dataInicio, DateTime? DataFim, int? periodo)
        {
            var token = Session["token"].ToString();
            //pega data de hoje no horario meia noite e adiciona 1 dia para finalizar o dia de hoje
            var datahoje = DateTime.Today.AddDays(1);
            DateTime startdate;
            DateTime enddate;
            RelatoriosViewModel ordens;
            System.Net.Http.HttpResponseMessage res = new System.Net.Http.HttpResponseMessage();
            if (dataInicio != null && DataFim != null)
            {
                startdate = dataInicio.GetValueOrDefault();
                enddate = DataFim.GetValueOrDefault();
                res = Helpers.ApiUsuario.GetRelatorioIndividual(token, startdate, enddate);
            }
            else if (periodo == null)
            {
                startdate = datahoje.AddDays(-7);
                res = Helpers.ApiUsuario.GetRelatorioIndividual(token, startdate, datahoje);
            }
            else if (periodo != null)
            {
                startdate = datahoje.AddDays((int)periodo * -1);
                res = Helpers.ApiUsuario.GetRelatorioIndividual(token, startdate, datahoje);
            }
            var content = res.Content.ReadAsStringAsync().Result;
            ordens = JsonConvert.DeserializeObject<RelatoriosViewModel>(content);
            ViewBag.dropperiodos = DropDownPeriodos(periodo);
            return View(ordens);
        }

        [LoadViewBags]
        public ActionResult Geral(DateTime? dataInicio, DateTime? DataFim, int? periodo)
        {
            var token = Session["token"].ToString();
            var datahoje = DateTime.Today.AddDays(1);
            DateTime startdate;
            DateTime enddate;
            RelatorioGeralViewModel viewmodel;
            System.Net.Http.HttpResponseMessage res = new System.Net.Http.HttpResponseMessage();
            if (dataInicio != null && DataFim != null)
            {
                startdate = dataInicio.GetValueOrDefault();
                enddate = DataFim.GetValueOrDefault();
                res = Helpers.ApiUsuario.GetRelatorioGeral(token, startdate, enddate);
            }
            else if (periodo == null)
            {
                startdate = datahoje.AddDays(-7);
                res = Helpers.ApiUsuario.GetRelatorioGeral(token, startdate, datahoje);
            }
            else if (periodo != null)
            {
                startdate = datahoje.AddDays((int)periodo * -1);
                res = Helpers.ApiUsuario.GetRelatorioGeral(token, startdate, datahoje);
            }
            var content = res.Content.ReadAsStringAsync().Result;
            viewmodel = JsonConvert.DeserializeObject<RelatorioGeralViewModel>(content);
            ViewBag.dropperiodos = DropDownPeriodos(periodo);
            return View(viewmodel);
        }

        private SelectList DropDownPeriodos(int? periodo)
        {
            var listitens = new List<SelectListItem>
            {
                new SelectListItem{Text= "7 dias", Value = "7" },
                new SelectListItem{Text= "15 dias", Value = "15" },
                new SelectListItem{Text= "30 dias", Value = "30"}
            };
            var SelectList = periodo == null ? new SelectList(listitens, "Value", "Text") : new SelectList(listitens, "Value", "Text", periodo + " dias");
            return SelectList;
        }
    }
}