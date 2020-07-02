using DataAccess.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    public interface IRecuperarSenha : IRepositoryBase<RecuperarSenha>
    {
        RecuperarSenha GetByTokenGuid(string token);
    }
}
