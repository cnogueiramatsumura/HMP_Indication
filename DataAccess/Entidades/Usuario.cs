using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DataAccess.Entidades
{
    public class Usuario
    {
        public Usuario()
        {
            EmailConfirmado = false;
        }

        public int Id { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmado { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public DateTimeOffset DataCadastro { get; set; }
        public DateTimeOffset DataVencimentoLicenca { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        public string Foto { get; set; }
        [JsonIgnore]
        public string OneSignalIDWeb { get; set; }
        [JsonIgnore]
        public string OneSignalIDApp { get; set; }
        [JsonIgnore]
        public int? AnalistaID { get; set; }
        [JsonIgnore]
        public string BinanceAPIKey { get; set; }
        [JsonIgnore]
        public string BinanceAPISecret { get; set; }
        public bool IsValidBinanceKeys { get; set; }




        //public virtual Analista analista { get; set; }      
        public virtual ICollection<Ordem> Ordems { get; set; }    
        public virtual ICollection<PagamentoLicenca> PagamentoLicenca { get; set; }       
        public virtual ICollection<ChamadasRecusadas> ChamadasRecusadas { get; set; } 
        public virtual ICollection<ConfirmEmail> ConfirmEmail { get; set; }   
        public virtual ICollection<EdicaoAceita> EdicoesAceitas { get; set; }   
        public virtual ICollection<CancelamentoRecusado> CancelamentoAceito { get; set; }
        public virtual ICollection<RecuperarSenha> RecuperarSenha { get; set; }
    }
}
