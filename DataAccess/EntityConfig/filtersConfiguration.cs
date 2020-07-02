using DataAccess.Entidades;
using System.Data.Entity.ModelConfiguration;

namespace DataAccess.EntityConfig
{
    public class filtersConfiguration : EntityTypeConfiguration<filters>
    {
        public filtersConfiguration()
        {
            HasKey(x => x.Id);
            Property(x => x.minPrice).IsOptional().HasPrecision(22, 8);
            Property(x => x.maxPrice).IsOptional().HasPrecision(22, 8);
            Property(x => x.tickSize).IsOptional().HasPrecision(22, 8);

            Property(x => x.multiplierUp).IsOptional().HasPrecision(22, 8);
            Property(x => x.multiplierDown).IsOptional().HasPrecision(22, 8);
            Property(x => x.avgPriceMins).IsOptional();

            Property(x => x.minQty).IsOptional().HasPrecision(22, 8);
            Property(x => x.maxQty).IsOptional().HasPrecision(22, 8);
            Property(x => x.stepSize).IsOptional().HasPrecision(22, 8);

            Property(x => x.minNotional).IsOptional().HasPrecision(22, 8);
            Property(x => x.applyToMarket).IsOptional();

            Property(x => x.limit).IsOptional();

            Property(x => x.maxNumAlgoOrders).IsOptional();

            Property(x => x.maxNumIcebergOrders).IsOptional();

            HasRequired(x => x.Symbol).WithMany(x => x.filters).HasForeignKey(x => x.Symbol_Id);
        }
    }
}
