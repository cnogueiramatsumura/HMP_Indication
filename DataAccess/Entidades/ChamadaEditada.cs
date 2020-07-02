using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entidades
{
    public class ChamadaEditada
    {
        public int Id { get; set; }
        public DateTimeOffset DataEdicao { get; set; }
        [DisplayFormat(DataFormatString = "{0:F8}", ApplyFormatInEditMode = true)]
        public decimal NewGain { get; set; }
        [DisplayFormat(DataFormatString = "{0:F8}", ApplyFormatInEditMode = true)]
        public decimal NewLoss { get; set; }
        public int Chamada_Id { get; set; }

      
        public virtual Chamada Chamada { get; set; }       
        public virtual ICollection<EdicaoAceita> EdicoesAceitas { get; set; }
    }
}
