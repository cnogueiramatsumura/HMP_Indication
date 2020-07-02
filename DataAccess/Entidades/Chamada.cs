using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entidades
{
    public class Chamada
    {
        public Chamada()
        {
            Ordems = new List<Ordem>();
        }

        [JsonProperty(Order = 1)]
        public int Id { get; set; }
        public string SymbolDescription { get; set; }    
        public DateTimeOffset DataCadastro { get; set; }      
        [DisplayFormat(DataFormatString = "{0:F8}", ApplyFormatInEditMode = true)]
        public decimal PrecoMercadoHoraChamada { get; set; }
     
        [DisplayFormat(DataFormatString = "{0:F8}", ApplyFormatInEditMode = true)]
        public decimal PrecoEntrada { get; set; }
     
        [DisplayFormat(DataFormatString = "{0:F8}", ApplyFormatInEditMode = true)]
        public decimal RangeEntrada { get; set; }
        [DisplayFormat(DataFormatString = "{0:F8}", ApplyFormatInEditMode = true)]     
        public decimal PrecoGain { get; set; }
        [DisplayFormat(DataFormatString = "{0:F8}", ApplyFormatInEditMode = true)]       
        public decimal PrecoLoss { get; set; }      
        public int ChamadaStatus_Id { get; set; }    
        public int Symbol_id { get; set; }      
        public string Observacao { get; set; }
        public Nullable<int> PercentualIndicado { get; set; }       
        public Nullable<int> ResultadoChamada_Id { get; set; }
        public int? Analista_Id { get; set; }


        public virtual Symbol Symbol { get; set; }
        public virtual Analista Analista { get; set; }
        public virtual ChamadaStatus ChamadaStatus { get; set; }     
        public virtual ICollection<Ordem> Ordems { get; set; }      
        public virtual ICollection<ChamadasRecusadas> ChamadasRecusadas { get; set; }      
        public virtual ICollection<ChamadaEditada> ChamadaEditada { get; set; }       
        public virtual ResultadoChamada ResultadoChamada { get; set; }        
        public virtual ICollection<EdicaoAceita> EdicaoAceita { get; set; }     
        public virtual CancelamentoChamada CancelamentoChamada { get; set; }
    }
}
