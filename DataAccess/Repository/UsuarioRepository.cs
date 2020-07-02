using DataAccess.Entidades;
using DataAccess.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.Repository
{
    public class UsuarioRepository : RepositoryBase<Usuario>, IUsuarioRepository
    {
        public Usuario GetByEmail(string email)
        {
            return db.usuario.Where(x => x.Email == email).FirstOrDefault();
        }

        public List<Usuario> OneSignalIds()
        {
            var listIds = db.usuario.Where(x => x.OneSignalIDWeb != null || x.OneSignalIDApp != null).ToList();
            return listIds;
        }
    }
}
