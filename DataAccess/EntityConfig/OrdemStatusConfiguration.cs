using DataAccess.Entidades;
using System.Data.Entity.ModelConfiguration;

namespace DataAccess.EntityConfig
{
    public class OrdemStatusConfiguration : EntityTypeConfiguration<OrdemStatus>
    {
        public OrdemStatusConfiguration()
        {
            HasKey(x => x.Id);
            Property(x => x.Descricao).IsRequired().HasMaxLength(50);
            HasMany(x => x.Ordems).WithRequired(x => x.OrdemStatus).HasForeignKey(x => x.OrdemStatus_Id);
        }
    }
}
