using DataAccess.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    public interface IOrdemComissionRepository : IRepositoryBase<OrdemComission>
    {
        List<OrdemComission> GetOrderComissions(int OrderID);
    }
}
