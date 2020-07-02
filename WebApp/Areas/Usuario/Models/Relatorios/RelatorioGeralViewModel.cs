using DataAccess.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Usuario.Models.Relatorios
{
    public class RelatorioGeralViewModel
    {
        public List<Chamada> Chamadas { get; set; }
        public decimal SaldoGanho { get; set; }
        [DataType(DataType.Date)]
        public DateTime dataIncio { get; set; }
        [DataType(DataType.Date)]
        public DateTime dataFim { get; set; }
    }
}