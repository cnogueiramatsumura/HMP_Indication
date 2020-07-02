using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataAccess.Serialized_Objects
{
    public class BinanceErrors
    {
        public int code { get; set; }
        public string msg { get; set; }
        public string motivo { get; set; }
    }
}