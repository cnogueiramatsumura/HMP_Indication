using DataAccess.Entidades;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityConfig
{
    public class ChamadasRecusadasConfiguration : EntityTypeConfiguration<ChamadasRecusadas>
    {
        public ChamadasRecusadasConfiguration()
        {
            ToTable("ChamadasRecusadas");
            HasKey(x => x.Id);
            Property(x => x.Chamada_ID).HasColumnType("int").IsRequired();
            Property(x => x.Usuario_ID).HasColumnType("int").IsRequired();
            Property(x => x.HoraRecusada).HasColumnType("DateTimeOffset").IsRequired();

            HasRequired(a => a.Chamada).WithMany(b => b.ChamadasRecusadas).HasForeignKey(x => x.Chamada_ID);
            HasRequired(a => a.Usuario).WithMany(b => b.ChamadasRecusadas).HasForeignKey(x => x.Usuario_ID);
        }
    }
}
