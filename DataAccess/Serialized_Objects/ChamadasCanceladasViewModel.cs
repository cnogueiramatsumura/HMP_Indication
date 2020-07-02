using DataAccess.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Serialized_Objects
{
    public class ChamadasCanceladasViewModel
    {
        public int id { get; set; }
        public int Chamada_Id { get; set; }
        public DateTimeOffset DataCancelamento { get; set; }
        public decimal PrecoEntrada { get; set; }      
        public decimal RangeEntrada { get; set; }      
        public decimal PrecoGain { get; set; }   
        public decimal PrecoLoss { get; set; }
        public string Observacao { get; set; }
        public DateTimeOffset DataCadastro { get; set; }
        public int OrdemId { get; set; }
        public decimal Quantidade { get; set; }
        public string Symbol { get; set; }
        public string baseAsset { get; set; }
        public string quoteAsset { get; set; }
    }
}
