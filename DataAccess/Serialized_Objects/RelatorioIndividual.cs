using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Serialized_Objects
{
    public class RelatorioIndividual
    {
        public int Id { get; set; }
        public int OrdemStatus_Id { get; set; }
        public string symbol { get; set; }
        public string Descricao { get; set; }
        public string baseAsset { get; set; }
        public string quoteAsset { get; set; }
        public string NomeAnalista { get; set; }
        public DateTimeOffset DataCadastro { get; set; }    

        [DisplayFormat(DataFormatString = "{0:F8}", ApplyFormatInEditMode = true)]
        public Nullable<decimal> PrecoVendaMercado { get; set; }

        [DisplayFormat(DataFormatString = "{0:F8}", ApplyFormatInEditMode = true)]
        public decimal PrecoEntrada { get; set; }

        [DisplayFormat(DataFormatString = "{0:F8}", ApplyFormatInEditMode = true)]
        public decimal PrecoGain { get; set; }

        [DisplayFormat(DataFormatString = "{0:F8}", ApplyFormatInEditMode = true)]
        public decimal PrecoLoss { get; set; }

        [DisplayFormat(DataFormatString = "{0:F8}", ApplyFormatInEditMode = true)]
        public Nullable<decimal> NewGain { get; set; }

        [DisplayFormat(DataFormatString = "{0:F8}", ApplyFormatInEditMode = true)]
        public Nullable<decimal> NewLoss { get; set; }

    }
}
