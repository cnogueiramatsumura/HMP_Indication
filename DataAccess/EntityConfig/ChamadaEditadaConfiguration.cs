using DataAccess.Entidades;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityConfig
{
    public class ChamadaEditadaConfiguration : EntityTypeConfiguration<ChamadaEditada>
    {
        public ChamadaEditadaConfiguration()
        {
            HasKey(x => x.Id);
            Property(x => x.DataEdicao).IsRequired();
            Property(x => x.Chamada_Id).IsRequired();
            Property(x => x.NewGain).IsRequired().HasPrecision(18, 8);
            Property(x => x.NewLoss).IsRequired().HasPrecision(18, 8);
            HasMany(x => x.EdicoesAceitas).WithRequired(x => x.ChamadaEditada).HasForeignKey(x => x.ChamadaEditada_ID);
        }
    }
}
