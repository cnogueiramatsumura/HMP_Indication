using DataAccess.Entidades;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityConfig
{
    public class ServerConfigConfiguration : EntityTypeConfiguration<ServerConfig>
    {
        public ServerConfigConfiguration()
        {
            HasKey(x => x.Id);
            Property(x => x.PagseguroToken).IsRequired();
            Property(x => x.PrecoLicenca).IsRequired().HasPrecision(5, 2);
            Property(x => x.BitpayToken).IsRequired();
            Property(x => x.BitpayIdentity).IsRequired().HasColumnType("varchar").HasMaxLength(256);
            Property(x => x.ApiServer).IsRequired(); 
            Property(x => x.ApiServer).IsRequired();
            Property(x => x.OneSignalAppId).IsRequired();
            Property(x => x.OneSignalToken).IsRequired();
            Property(x => x.SmtpAdress).IsOptional();
            Property(x => x.SmtpPort).IsOptional();
            Property(x => x.SmtpUsername).IsOptional();
            Property(x => x.SmtpPassword).IsOptional();
        }
    }
}
