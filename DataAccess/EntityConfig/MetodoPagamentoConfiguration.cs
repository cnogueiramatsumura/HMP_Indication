using DataAccess.Entidades;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityConfig
{
    public class MetodoPagamentoConfiguration : EntityTypeConfiguration<MetodoPagamento>
    {
        public MetodoPagamentoConfiguration()
        {
            HasKey(x => x.Id);
            Property(x => x.Descricao).IsRequired().HasMaxLength(50);
            HasMany(x => x.PagamentoLicenca).WithRequired(x => x.MetodoPagamento).HasForeignKey(x => x.MetodoPagamentoId);
        }
    }
}
