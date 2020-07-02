using DataAccess.Serialized_Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Usuario.Models.DashBoard
{
    public class LimitesViewModel
    {
        public List<balances> balances { get; set; }
        public List<SymbolTicker> symbolTickers { get; set; }
    }
}