using DataAccess.Entidades;
using System.Collections.Generic;

namespace DataAccess.Interfaces
{
    public interface ISymbolRepository : IRepositoryBase<Symbol>
    {
        Symbol GetBySymbol(string Symbol);
        bool GetValidSymbol(string Symbol);
        void AddRange(List<Symbol> Symbols);
    }
}
