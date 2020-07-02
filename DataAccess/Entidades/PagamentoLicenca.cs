using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entidades
{
    public class PagamentoLicenca
    {
        public int Id { get; set; }
        public DateTimeOffset DataCriacaoInvoice { get; set; }
        public Nullable<DateTimeOffset> DataPagamento { get; set; }
        public int MetodoPagamentoId { get; set; }
        public int PagamentoLicencaStatusId { get; set; }
        public decimal ValoraReceber { get; set; }
        public decimal ValorPago { get; set; }
        public int Usuario_Id { get; set; }
        public string CodigoPagSeguro { get; set; }
        public string CodigoBitPay { get; set; }
        public string TokenPagamento { get; set; }
        public string Qtd_BTC_Pago { get; set; }

     
        public virtual Usuario Usuario { get; set; }   
        public virtual MetodoPagamento MetodoPagamento{ get; set; }    
        public virtual PagamentoLicencaStatus PagamentoLicencaStatus { get; set; }
    }
}
