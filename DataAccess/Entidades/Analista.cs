using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DataAccess.Entidades
{
    public class Analista
    {
        public int Id { get; set; }

        public string Email { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public DateTimeOffset DataCadastro { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        public string Foto { get; set; }
        public bool Ativo { get; set; }

        public virtual ICollection<Chamada> chamadas { get; set; }
    }
}
