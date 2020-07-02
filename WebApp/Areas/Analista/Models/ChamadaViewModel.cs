using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.CustomDataAnnotations;

namespace WebApp.Areas.Analista.Models
{
    public class ChamadaViewModel
    {
        [Required]
        [DisplayName("Nome da Moeda")]  
        public string SymbolDescription { get; set; }
        [Remote("checksymbolName", "Symbols", ErrorMessage = "Symbol Inválido")]
        [Required]
        [DisplayName("Symbol")]
        public string SymbolName { get; set; }
        [Required]    
        public decimal ValorMercado { get; set; }
        [Required]
        [DisplayName("Preço de Entrada")]
        [ValidadeEntrada]
        public decimal PrecoEntrada { get; set; }
        [Required]
        [ValidateRangeEntrada]
        [DisplayName("Limite de Preço")]
        public decimal RangeEntrada { get; set; }
        [Required]
        [ValidateGain]
        [DisplayName("Preço do Gain")]
        public decimal PrecoGain { get; set; }
        [Required]
        [ValidadeLoss]
        [DisplayName("Preço do Loss")]
        public decimal PrecoLoss { get; set; }
        [DisplayName("Observação")]
        public string Observacao { get; set; }
        [DisplayName("Indicaçao de Investimento")]
        public string PercentualIndicado { get; set; }     
    }
}