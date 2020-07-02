using DataAccess.Entidades;
using System.Collections.Generic;

namespace DataAccess.Interfaces
{
    public interface IUsuarioRepository : IRepositoryBase<Usuario>
    {
        Usuario GetByEmail(string email);
        List<Usuario> OneSignalIds();
    }
}