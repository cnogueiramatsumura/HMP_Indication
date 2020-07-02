using DataAccess.Entidades;
using System.Data.Entity.ModelConfiguration;

namespace DataAccess.EntityConfig
{
   public class BinanceStatusConfiguration : EntityTypeConfiguration<BinanceStatus>
    {

        public BinanceStatusConfiguration()
        {
            HasKey(x => x.Id);
            Property(x => x.Descricao).IsRequired().HasMaxLength(50);
            HasMany(x => x.Ordems).WithOptional(x => x.BinanceStatus).HasForeignKey(x => x.BinanceStatus_Id);
        }
    }
}
