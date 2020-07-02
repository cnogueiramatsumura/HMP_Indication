using DataAccess.Entidades;
using DataAccess.Serialized_Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    public interface ICancelamentoChamadaRepository : IRepositoryBase<CancelamentoChamada>
    {
        List<ChamadasCanceladasViewModel> GetAllOpen(int UsuarioId);
    }
}
