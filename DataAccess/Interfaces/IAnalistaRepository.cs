using DataAccess.Entidades;

namespace DataAccess.Interfaces
{
    public interface IAnalistaRepository : IRepositoryBase<Analista>
    {
        Analista GetByEmail(string email);
    }
}
