using DataAccess.Entidades;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityConfig
{
    public class TipoEdicaoAceitaConfiguration : EntityTypeConfiguration<TipoEdicaoAceita>
    {
        public TipoEdicaoAceitaConfiguration()
        {
            HasKey(x => x.Id);
            Property(x => x.Descricao).IsRequired();
            HasMany(x => x.EdicaoAceitas).WithRequired(x => x.TipoEdicao).HasForeignKey(x => x.TipoEdicao_ID);
        }
    }
}
