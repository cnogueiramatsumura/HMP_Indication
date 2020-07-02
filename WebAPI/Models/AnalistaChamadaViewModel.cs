using DataAccess.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAPI.Models
{
    public class AnalistaChamadaViewModel
    {
        public int Analista_Id { get; set; }
        public int Id { get; set; }
        public DateTime DataCadastro { get; set; }
        [Required]
        public string SymbolDescription { get; set; }
        [Required]
        public string SymbolName { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:F8}", ApplyFormatInEditMode = true)]
        public string PrecoEntrada { get; set; }
        [Required]
        public string RangeEntrada { get; set; }
        [Required]
        public string PrecoGain { get; set; }
        [Required]
        public string PrecoLoss { get; set; }
        public string Observacao { get; set; }
        public Nullable<int> PercentualIndicado { get; set; }
        public int chamadaStatus_Id { get; set; }
        public Symbol Symbol { get; set; }
    }
}