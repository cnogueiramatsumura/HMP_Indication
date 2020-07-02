using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entidades
{
    public class Ordem
    {
        public int Id { get; set; }    
        public DateTimeOffset DataCadastro { get; set; }
        public decimal Quantidade { get; set; }
        //Codigo da Ordem gerada no Exchange
        public string OrderID { get; set; }
        //Codigo da Ordem Oco responsavel pela ordem LimitOrder_ID & StopOrder_ID
        //Ordem Oco gera duas ordens
        public string OcoOrderListId { get; set; }
        public string LimitOrder_ID { get; set; }
        public string StopOrder_ID { get; set; }
        public Nullable<DateTimeOffset> DataEntrada { get; set; }
        public Nullable<DateTimeOffset> DataExecucao { get; set; }
        public Nullable<DateTimeOffset> DataCancelamento { get; set; }
        public int Chamada_Id { get; set; }
        public int Usuario_Id { get; set; }
        public int OrdemStatus_Id { get; set; }
        public Nullable<int> BinanceStatus_Id { get; set; }
        public int TipoOrdem_Id { get; set; }
        public Nullable<int> MainOrderID { get; set; }
        [DisplayFormat(DataFormatString = "{0:F8}", ApplyFormatInEditMode = true)]
        public Nullable<decimal> PrecoVendaMercado { get; set; }
        public Nullable<int> MotivoCancelamento_ID{ get; set; }
        


        public virtual Chamada Chamada { get; set; }     
        public virtual Usuario Usuario { get; set; }
        public virtual OrdemStatus OrdemStatus { get; set; }     
        public virtual BinanceStatus BinanceStatus { get; set; }     
        public virtual TipoOrdem TipoOrdem { get; set; }       
        public virtual ICollection<OrdemComission> OrderComissions { get; set; }     
        public virtual MotivoCancelamento MotivoCancelamento { get; set; }
    }
}
