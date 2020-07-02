using DataAccess.Entidades;
using DataAccess.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.Repository
{
    public class FiltersRepository : RepositoryBase<filters>, IFiltersRepository
    {
        public void AddRange(List<filters> Filters)
        {
            db.BulkInsert(Filters);         
        }

        public List<filters> GetBySymbol_Id(int symbol_Id)
        {
            return (from p in db.filters where p.Symbol_Id == symbol_Id select p).ToList();
        }

        public filters GetBySymbol_Type(string filterType, int SymbolId)
        {
            return (from p in db.filters where p.filterType == filterType && p.Symbol_Id == SymbolId select p).FirstOrDefault();
        }
    }
}
