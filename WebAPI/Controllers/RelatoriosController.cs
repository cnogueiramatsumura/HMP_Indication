using DataAccess.Entidades;
using DataAccess.Interfaces;
using DataAccess.Serialized_Objects;
using DataAccess.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Helpers;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    //[Authorize]
    public class RelatoriosController : ApiController
    {
    
        private readonly IOrdemRepository _OrdemRepo;
        private readonly IChamadasRepository _ChamadasRepoRepo;

        public RelatoriosController(IOrdemRepository OrdemRepository, IChamadasRepository ChamadasRepository)
        {           
            _OrdemRepo = OrdemRepository;
            _ChamadasRepoRepo = ChamadasRepository;
        }

        [HttpGet]
        [Route("api/relatorios/individual")]
        [Authorize]
        public HttpResponseMessage Individual(DateTime dataInicio, DateTime dataFim)
        {
            try
            {
                var userId = int.Parse(Helper.GetJWTPayloadValue(Request, "id"));
                var Ordems = _OrdemRepo.RelatorioIndividual(userId, dataInicio, dataFim);
                var viewModel = new RelatoriosViewModel
                {
                    Gain = Ordems.Where(x => x.OrdemStatus_Id == 5).ToList(),
                    Loss = Ordems.Where(x => x.OrdemStatus_Id == 6).ToList(),
                    Vendidas = Ordems.Where(x => x.OrdemStatus_Id == 2).ToList(),
                    SaldoGanho = SomaSaldoIndividual(Ordems)
                };
                return Request.CreateResponse(HttpStatusCode.OK, viewModel);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("api/relatorios/Geral")]
        [Authorize]
        public HttpResponseMessage Geral(DateTime dataInicio, DateTime dataFim)
        {
            try
            {             
                var chamadas = _ChamadasRepoRepo.RelatorioGeral(dataInicio, dataFim);
                var viewModel = new RelatorioGeralViewModel
                {
                    Gain = chamadas.Where(x => x.ResultadoChamada_Id == 1).ToList(),
                    Loss = chamadas.Where(x => x.ResultadoChamada_Id == 2).ToList(),
                    SaldoGanho = SomaSaldoGeral(chamadas)                    
                };
                return Request.CreateResponse(HttpStatusCode.OK, viewModel);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        private decimal SomaSaldoIndividual(List<RelatorioIndividual> Ordems)
        {
            decimal saldoGanho = 0;
            foreach (var item in Ordems)
            {
                if(item.OrdemStatus_Id == 5)
                {
                    saldoGanho += (decimal)(item.NewGain != null ?  item.NewGain - item.PrecoEntrada : item.PrecoGain - item.PrecoEntrada);
                }
                else if(item.OrdemStatus_Id == 6)
                {
                    saldoGanho += (decimal)(item.NewLoss != null ? item.NewLoss - item.PrecoEntrada : item.PrecoLoss - item.PrecoEntrada);
                }
                else if(item.OrdemStatus_Id == 2)
                {
                    saldoGanho += (decimal)item.PrecoVendaMercado - item.PrecoEntrada;
                }             
            }
            return saldoGanho;
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
    }
}
