using Newtonsoft.Json;
using System.Collections.Generic;

namespace DataAccess.Entidades
{
    public class OrdemStatus
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
     
        public virtual ICollection<Ordem> Ordems { get; set; }
    }
}
