using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Serialized_Objects
{
    public class EdicoesAbertas
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public string baseAsset { get; set; }
        public string quoteAsset { get; set; }
        public DateTimeOffset DataEdicao { get; set; }   
        public int Chamada_Id { get; set; }
        public decimal PrecoEntrada { get; set; }
        public decimal RangeEntrada { get; set; }
        public decimal PrecoGain { get; set; }
        public decimal PrecoLoss { get; set; }
        public decimal NewGain { get; set; }
        public decimal NewLoss { get; set; }
    }
}
