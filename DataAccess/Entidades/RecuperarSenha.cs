using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entidades
{
    public class RecuperarSenha
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTimeOffset DataCadastro { get; set; }
        public int Usuario_Id { get; set; }
        public bool Utilizado { get; set; }

        public virtual Usuario Usuario { get; set; }
    }
}
