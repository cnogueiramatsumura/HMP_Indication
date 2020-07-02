using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Serialized_Objects
{
    public class outboundAccountPosition
    {
        public string e { get; set; }
        public long E { get; set; }
        public long u { get; set; }
        public List<B> B { get; set; }
    }

    public class B
    {
        public string a { get; set; }
        public decimal f { get; set; }
        public decimal l { get; set; }
    }
}
