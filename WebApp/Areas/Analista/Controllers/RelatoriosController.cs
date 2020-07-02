using DataAccess.Entidades;
using DataAccess.Interfaces;
using DataAccess.Serialized_Objects;
using DataAccess.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace WebApp.Areas.Analista.Controllers
{
    [Authorize]
    public class RelatoriosController : Controller
    {
        private readonly IChamadasRepository _ChamadasRepo;
        private readonly IAnalistaRepository _AnalistaRepo;
        public RelatoriosController(IChamadasRepository ChamadasRepository, IAnalistaRepository AnalistaRepository)
        {
            _ChamadasRepo = ChamadasRepository;
            _AnalistaRepo = AnalistaRepository;
        }

        public ActionResult Geral(DateTime? dataInicio, DateTime? DataFim, int? periodo)
        {
            var AnalistaID = _AnalistaRepo.GetByEmail(User.Identity.Name).Id;   
            var datahoje = DateTime.Today.AddDays(1);
            DateTime startdate;
            DateTime enddate;
            var res = new List<RelatorioGeral>();
            if (dataInicio != null && DataFim != null)
            {
                startdate = dataInicio.GetValueOrDefault();
                enddate = DataFim.GetValueOrDefault();
                res = _ChamadasRepo.RelatorioGeralAnalista(startdate, enddate, AnalistaID);
            }
            else if (periodo == null)
            {
                startdate = datahoje.AddDays(-7);
                res = _ChamadasRepo.RelatorioGeralAnalista(startdate, datahoje, AnalistaID);
            }
            else if (periodo != null)
            {
                startdate = datahoje.AddDays((int)periodo * -1);
                res = _ChamadasRepo.RelatorioGeralAnalista(startdate, datahoje, AnalistaID);
            }
            var viewModel = new RelatorioGeralViewModel
            {
                Gain = res.Where(x => x.ResultadoChamada_Id == 1).ToList(),
                Loss = res.Where(x => x.ResultadoChamada_Id == 2).ToList(),
                SaldoGanho = SomaSaldoGeral(res)
            };

            ViewBag.dropperiodos = DropDownPeriodos(periodo);
            return View(viewModel);
        }

        private decimal SomaSaldoGeral(List<RelatorioGeral> chamadas)
        {
            decimal saldoGanho = 0;
            foreach (var item in chamadas)
            {
                if (item.ResultadoChamada_Id == 1)
                {
                    saldoGanho += (decimal)(item.NewGain != null ? item.NewGain - item.PrecoEntrada : item.PrecoGain - item.PrecoEntrada);
                }
                else if (item.ResultadoChamada_Id == 2)
                {
                    saldoGanho += (decimal)(item.NewLoss != null ? item.NewLoss - item.PrecoEntrada : item.PrecoLoss - item.PrecoEntrada);
                }
            }
            return saldoGanho;
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