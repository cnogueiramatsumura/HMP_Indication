using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataAccess.Serialized_Objects
{
    public class WS_Trade
    {
        public string stream { get; set; }
        public data data { get; set; }
    }

    public class data
    {
        public string E { get; set; } //Event Type
        public string e { get; set; } //Event Type
        public string s { get; set; } //Symbol
        public decimal p { get; set; } //priece
    }

}