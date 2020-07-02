using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entidades
{
    public class CancelamentoRecusado
    {
        public int Id { get; set; }
        public DateTimeOffset DataCancelamento { get; set; }
        public int CancelamentoChamada_Id { get; set; }
        public int Usuario_Id { get; set; }

    
        public virtual CancelamentoChamada CancelamentoChamada { get; set; }    
        public virtual Usuario Usuario { get; set; }
    }
}
