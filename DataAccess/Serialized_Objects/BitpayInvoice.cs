using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Serialized_Objects
{
   public class BitpayInvoice
    {
        public int orderId { get; set; }
        public string guid { get; set; }
        public string token { get; set; }
        public decimal price { get; set; }
        public string currency { get; set; }
        public string redirectUrl { get; set; }
        public string notificationURL { get; set; }
        public string posData { get; set; }
        public string notify { get; set; }

        public BitpayInvoice()
        {
            currency = "BRL";
        }
    }
}
