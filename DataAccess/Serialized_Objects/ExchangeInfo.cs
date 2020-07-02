using DataAccess.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataAccess.Serialized_Objects
{
    public class ExchangeInfo
    {
        public string timezone { get; set; }
        public string serverTime { get; set; }
        public List<rateLimits> rateLimits { get; set; }
        public exchangeFilters[] exchangeFilters { get; set; }
        public Symbol[] symbols { get; set; }
    }

    public class rateLimits
    {
        public string rateLimitType { get; set; }
        public string interval { get; set; }
        public int intervalNum { get; set; }
        public int limit { get; set; }
    }

    public class exchangeFilters
    {

    }
}