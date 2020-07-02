using DataAccess.Entidades;
using DataAccess.Serialized_Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataAccess.ViewModels
{
    public class RelatoriosViewModel
    {
        public List<RelatorioIndividual> Gain { get; set; }
        public List<RelatorioIndividual> Loss { get; set; }
        public List<RelatorioIndividual> Vendidas { get; set; }
        public decimal SaldoGanho { get; set; }
    }
}