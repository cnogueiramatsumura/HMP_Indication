using DataAccess.Entidades;
using DataAccess.Interfaces;
using System.Linq;

namespace DataAccess.Repository
{
    public class AnalistaRepository : RepositoryBase<Analista>, IAnalistaRepository
    {
        public Analista GetByEmail(string email)
        {
            return db.analista.Where(x => x.Email == email).FirstOrDefault();
        }
    }
}
