using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Serialized_Objects
{
    public class BitpayPaymentResponse
    {
        public int orderId { get; set; }
        public string id { get; set; }   
        public string url { get; set; }     
        public string posData { get; set; }     
        public string status { get; set; }
        public decimal price{ get; set; }
        public string currency { get; set; }
        public long invoiceTime { get; set; }
        public long expirationTime { get; set; }
        public long currentTime { get; set; }
        public string amountPaid { get; set; }
        public string transactionCurrency{ get; set; }
    }
}
