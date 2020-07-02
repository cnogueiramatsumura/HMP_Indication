using DataAccess.Entidades;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityConfig
{
    public class MotivoCancelamentoConfiguration : EntityTypeConfiguration<MotivoCancelamento>
    {
        public MotivoCancelamentoConfiguration()
        {
            HasKey(x => x.Id);
            Property(x => x.Descricao).IsRequired().HasMaxLength(50);
            HasMany(x => x.Ordems).WithOptional(x => x.MotivoCancelamento).HasForeignKey(x => x.MotivoCancelamento_ID);
        }
    }
}
