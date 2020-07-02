using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entidades
{
    public class PagamentoLicencaStatus
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
   
        public virtual ICollection<PagamentoLicenca> PagamentoLicenca { get; set; }
    }
}
