using DataAccess.Entidades;
using DataAccess.Serialized_Objects;
using System.Collections.Generic;

namespace DataAccess.Interfaces
{
    public interface IChamadaEditadaRepository : IRepositoryBase<ChamadaEditada>
    {
        List<EdicoesAbertas> GetAllOpen(int userId);
        List<ChamadaEditada> GetListEdit(int chamadaId);
    }
}
