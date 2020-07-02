using DataAccess.Entidades;
using System.Collections.Generic;

namespace DataAccess.Interfaces
{
    public interface IFiltersRepository : IRepositoryBase<filters>
    {
        filters GetBySymbol_Type(string filterType, int SymbolId);
        List<filters> GetBySymbol_Id(int symbol_Id);
        void AddRange(List<filters> Filters);
    }
}
