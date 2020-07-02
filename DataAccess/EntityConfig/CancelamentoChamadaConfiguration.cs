using DataAccess.Entidades;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityConfig
{
    public class CancelamentoChamadaConfiguration : EntityTypeConfiguration<CancelamentoChamada>
    {
        public CancelamentoChamadaConfiguration()
        {
            HasKey(x => x.Id);
            Property(x => x.DataCancelamento).IsRequired();
            Property(x => x.Chamada_Id).IsRequired();
            HasRequired(x => x.Chamada).WithMany().HasForeignKey(x => x.Chamada_Id);
            HasMany(x => x.CancelamentoRecusado).WithRequired(x => x.CancelamentoChamada).HasForeignKey(x => x.CancelamentoChamada_Id);
        }
    }
}
