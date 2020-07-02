using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entidades
{
    public class EdicaoAceita
    {
        public int Id { get; set; }
        public int ChamadaEditada_ID { get; set; }
        public int Usuario_Id { get; set; }
        public int TipoEdicao_ID { get; set; }
        public DateTimeOffset DataCadastro { get; set; }
        public int Chamada_ID { get; set; }

     
        public virtual TipoEdicaoAceita TipoEdicao { get; set; }       
        public virtual Usuario Usuario { get; set; }     
        public virtual ChamadaEditada ChamadaEditada { get; set; }        
        public virtual Chamada Chamada { get; set; }
    }
}
