using DataAccess.Entidades;
using System.Data.Entity.ModelConfiguration;

namespace DataAccess.EntityConfig
{
    public class ChamadaStatusConfiguration : EntityTypeConfiguration<ChamadaStatus>
    {
        public ChamadaStatusConfiguration()
        {
            HasKey(x => x.Id);
            Property(x => x.Descricao).IsRequired().HasMaxLength(50);
            HasMany(x => x.Chamadas).WithRequired(x => x.ChamadaStatus).HasForeignKey(x => x.ChamadaStatus_Id);
        }
    }
}
