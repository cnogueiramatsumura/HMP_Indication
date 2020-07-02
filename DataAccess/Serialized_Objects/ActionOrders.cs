using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Serialized_Objects
{
    public class ActionOrders
    {
        public int Id { get; set; }    
        public DateTimeOffset DataCadastro { get; set; }
        public Nullable<DateTimeOffset> DataEntrada { get; set; }
        public Nullable<DateTimeOffset> DataCancelamento { get; set; }
        public Nullable<DateTimeOffset> DataExecucao { get; set; }
        public decimal Quantidade { get; set; }
        public int chamada_Id { get; set; }
        public int Usuario_Id { get; set; }
        public int OrdemStatus_Id { get; set; }
        public string symbol { get; set; }
        public string baseAsset { get; set; }
        public string quoteAsset { get; set; }
        public decimal PrecoEntrada { get; set; }
        public decimal RangeEntrada { get; set; }
        public Nullable<decimal> PrecoVendaMercado { get; set; }
        public decimal PrecoGain { get; set; }
        public decimal PrecoLoss { get; set; }
        public Nullable<decimal> NewGain { get; set; }
        public Nullable<decimal> NewLoss { get; set; }
        public string Descricao { get; set; }
        public string Observacao { get; set; }
    }
}
