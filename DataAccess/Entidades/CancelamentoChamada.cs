using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entidades
{
    public class CancelamentoChamada
    {
        public int Id { get; set; }
        public int Chamada_Id { get; set; }
        public DateTimeOffset DataCancelamento { get; set; }

        public virtual Chamada Chamada { get; set; }     
        public virtual ICollection<CancelamentoRecusado> CancelamentoRecusado { get; set; }
    }
}
