using DataAccess.Entidades;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityConfig
{
    public class PagamentoLicencaConfiguration : EntityTypeConfiguration<PagamentoLicenca>
    {
        public PagamentoLicencaConfiguration()
        {
            HasKey(x => x.Id);
            Property(x => x.DataCriacaoInvoice).IsRequired();      
            Property(x => x.ValoraReceber).IsRequired().HasPrecision(18, 8);
            Property(x => x.ValorPago).IsOptional().HasPrecision(18, 8);
            Property(x => x.DataPagamento).IsOptional();
            Property(x => x.CodigoPagSeguro).IsOptional();
            Property(x => x.CodigoBitPay).IsOptional();
            Property(x => x.TokenPagamento).IsRequired();
            Property(x => x.PagamentoLicencaStatusId).IsRequired();
            Property(x => x.MetodoPagamentoId).IsRequired();
        }     
    }
}
