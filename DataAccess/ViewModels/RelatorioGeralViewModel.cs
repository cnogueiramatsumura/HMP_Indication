using DataAccess.Entidades;
using DataAccess.Serialized_Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataAccess.ViewModels
{
    public class RelatorioGeralViewModel
    {
        public List<RelatorioGeral> Gain{ get; set; }
        public List<RelatorioGeral> Loss { get; set; }
        public decimal SaldoGanho { get; set; }
    }   
}