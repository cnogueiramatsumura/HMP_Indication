using DataAccess.Entidades;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityConfig
{
    public class CancelamentoRecusadoConfiguration : EntityTypeConfiguration<CancelamentoRecusado>
    {
        public CancelamentoRecusadoConfiguration()
        {
            HasKey(x => x.Id);
            Property(x => x.CancelamentoChamada_Id).IsRequired();                
            Property(x => x.DataCancelamento).IsRequired();            
        }
    }
}
