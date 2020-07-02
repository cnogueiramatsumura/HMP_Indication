using DataAccess.Entidades;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityConfig
{
    public class SymbolConfiguration : EntityTypeConfiguration<Symbol>
    {
        public SymbolConfiguration()
        {
            HasKey(x => x.Id);
            Property(x => x.symbol).IsRequired();
            Property(x => x.status).IsRequired();
            Property(x => x.baseAsset).IsRequired();
            Property(x => x.baseAssetPrecision).IsRequired();
            Property(x => x.quoteAsset).IsRequired();
            Property(x => x.quotePrecision).IsRequired();
            Property(x => x.icebergAllowed).IsRequired();
            Property(x => x.ocoAllowed).IsRequired();
            Property(x => x.isSpotTradingAllowed).IsRequired();
            Property(x => x.isMarginTradingAllowed).IsRequired();

            HasMany(x => x.filters).WithRequired(x => x.Symbol).HasForeignKey(x => x.Symbol_Id);
            HasMany(x => x.chamadas).WithRequired(x => x.Symbol).HasForeignKey(x => x.Symbol_id);
        }
    }
}
