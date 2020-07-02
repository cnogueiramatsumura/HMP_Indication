using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entidades
{
    public class ChamadasRecusadas
    {
        public int Id { get; set; }
        public int Usuario_ID { get; set; }
        public int Chamada_ID { get; set; }
        public DateTimeOffset HoraRecusada { get; set; }

     
        public virtual Chamada Chamada { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
