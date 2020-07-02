using DataAccess.Entidades;
using DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class RecuperarSenhaRepository : RepositoryBase<RecuperarSenha>, IRecuperarSenha
    {
        public RecuperarSenha GetByTokenGuid(string token)
        {
            var validtime = DateTime.UtcNow.AddMinutes(-15);
            var res = (from p in db.recuperarSenha where p.Token == token && p.DataCadastro > validtime orderby p.Id descending select p).FirstOrDefault();
            return res;
        }
    }
}
