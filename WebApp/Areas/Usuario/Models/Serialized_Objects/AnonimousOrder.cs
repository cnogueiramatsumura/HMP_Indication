using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Usuario.Models.Serialized_Objects
{
    public class AnonimousOrder
    {
        public int Id { get; set; }
        public DateTimeOffset? DataCadastro { get; set; }
        public int TipoOrdem { get; set; }
        public int Status_id { get; set; }
        public int Chamada_Id { get; set; }
        public string Symbol { get; set; }
        public string Descricao { get; set; }
        public string Observacao { get; set; }
        public decimal PrecoEntrada { get; set; }
        public decimal RangeEntrada { get; set; }
        public decimal PrecoGain { get; set; }
        public decimal PrecoLoss { get; set; }
        public decimal Quantidade { get; set; }
    }
}