using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entidades
{
    public class ChamadaStatus
    {
        public ChamadaStatus()
        {
            Chamadas = new List<Chamada>();
        }

        public int Id { get; set; }
        public string Descricao { get; set; }
      
        public virtual ICollection<Chamada> Chamadas { get; set; }
    }
}
