using DataAccess.Entidades;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityConfig
{
    public class PagamentoLicencaStatusConfiguration : EntityTypeConfiguration<PagamentoLicencaStatus>
    {
        public PagamentoLicencaStatusConfiguration()
        {
            HasKey(x => x.Id);
            Property(x => x.Descricao).IsRequired().HasMaxLength(50);
            HasMany(x => x.PagamentoLicenca).WithRequired(x => x.PagamentoLicencaStatus).HasForeignKey(x => x.PagamentoLicencaStatusId);
        }
    }
}
