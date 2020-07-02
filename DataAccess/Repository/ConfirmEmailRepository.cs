using DataAccess.Entidades;
using DataAccess.Interfaces;
using System.Linq;

namespace DataAccess.Repository
{
    public class ConfirmEmailRepository : RepositoryBase<ConfirmEmail>, IConfirmEmailRepository
    {
        public ConfirmEmail GetByToken(string token)
        {
            var res = (from p in db.confirmEmail.Include("usuario") where p.Token == token select p).FirstOrDefault();
            return res;
        }

        public ConfirmEmail GetByUserID(int UserID)
        {
            var res = (from p in db.confirmEmail where p.Usuario_Id == UserID orderby p.Id descending select p).FirstOrDefault();
            return res;
        }
    }
}
