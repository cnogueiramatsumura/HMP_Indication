using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Serialized_Objects
{
    public class BitpaiInvoiceResponse
    {
        public string facade { get; set; }
        public retobj data { get; set; }
    }

    public class retobj
    {       
        public string Id { get; set; }
        public string currency { get; set; }
        public string Url { get; set; }
        public string Status { get; set; }
        public string amountPaid { get; set; }
        public long InvoiceTime { get; set; }
        public long ExpirationTime { get; set; }
        public long CurrentTime { get; set; }
        public int TargetConfirmations { get; set; }
        public string guid { get; set; }
        public string TransactionCurrency { get; set; }
        public string displayAmountPaid { get; set; }
        public decimal price{ get; set; }

    }
}
