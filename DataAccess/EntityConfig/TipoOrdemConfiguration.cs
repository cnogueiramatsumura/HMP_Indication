using DataAccess.Entidades;
using System.Data.Entity.ModelConfiguration;

namespace DataAccess.EntityConfig
{
    public class TipoOrdemConfiguration : EntityTypeConfiguration<TipoOrdem>
    {
        public TipoOrdemConfiguration()
        {
            HasKey(x => x.Id);
            Property(x => x.Descricao).IsRequired().HasMaxLength(50);
            HasMany(x => x.Ordems).WithRequired(x => x.TipoOrdem).HasForeignKey(x => x.TipoOrdem_Id);
        }
    }
}
