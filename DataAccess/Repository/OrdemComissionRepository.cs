using DataAccess.Entidades;
using DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
   public class OrdemComissionRepository : RepositoryBase<OrdemComission> , IOrdemComissionRepository
    {

        public List<OrdemComission> GetOrderComissions(int OrderID)
        {
            return db.ordemComission.Where(x => x.Order_Id == OrderID).ToList();
        }
    }
}
