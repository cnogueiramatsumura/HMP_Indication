using DataAccess.Entidades;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityConfig
{
    public class ResultadoChamadaConfiguration : EntityTypeConfiguration<ResultadoChamada>
    {
        public ResultadoChamadaConfiguration()
        {
            HasKey(x => x.Id);
            Property(x => x.Descricao).IsRequired().HasMaxLength(50);
            HasMany(x => x.Chamadas).WithOptional(x => x.ResultadoChamada).HasForeignKey(x => x.ResultadoChamada_Id);
        }
    }
}
