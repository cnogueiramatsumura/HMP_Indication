using DataAccess.Entidades;
using DataAccess.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.Repository
{
    public class SymbolRepository : RepositoryBase<Symbol>, ISymbolRepository
    {
        public Symbol GetBySymbol(string Symbol)
        {
            return (from p in db.symbol where p.symbol == Symbol select p).FirstOrDefault();
        }

        public bool GetValidSymbol(string Symbol)
        {
            return (from p in db.symbol where p.symbol == Symbol.ToLower() && p.ocoAllowed == true select p).FirstOrDefault() != null;
        }

        public void AddRange(List<Symbol> Symbols)
        {
            db.symbol.AddRange(Symbols);
            db.SaveChanges();
        }
    }
}
