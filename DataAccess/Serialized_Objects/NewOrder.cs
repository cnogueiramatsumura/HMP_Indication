using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Serialized_Objects
{
    public class NewOrder
    {
        public string symbol { get; set; }
        public string orderId { get; set; }
        public int orderListId { get; set; }
        public string clientOrderId { get; set; }
        public string transactTime { get; set; }
        public decimal price { get; set; }
        public decimal origQty { get; set; }
        public decimal executedQty { get; set; }
        public decimal cummulativeQuoteQty { get; set; }
        public string status { get; set; }
        public string timeInForce { get; set; }
        public string type { get; set; }
        public string side { get; set; }
        public List<fills> fills { get; set; }
    }

    public class fills
    {
        public decimal price { get; set; }
        public decimal qty { get; set; }
        public decimal commission { get; set; }
        public string commissionAsset { get; set; }
    }
}
