using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Serialized_Objects
{
    public class Currency
    {
        public int code { get; set; }
        public string message { get; set; }
        public string messageDetail { get; set; }
        public List<currencyData> data { get; set; }
    }

    public class currencyData
    {
        public string pair { get; set; }
        public decimal rate { get; set; }
        public string symbol { get; set; }
    }
}
