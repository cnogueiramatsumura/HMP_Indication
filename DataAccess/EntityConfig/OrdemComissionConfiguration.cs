using DataAccess.Entidades;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityConfig
{
    public class OrdemComissionConfiguration : EntityTypeConfiguration<OrdemComission>
    {
        public OrdemComissionConfiguration()
        {
            HasKey(x => x.Id);
            Property(x => x.Order_Id).IsRequired();
            Property(x => x.ComissionAsset).IsRequired();
            Property(x => x.ComissionAmount).IsRequired().HasPrecision(18,8);        
            Property(x => x.ValorExecutado).IsRequired().HasPrecision(18, 8);
            Property(x => x.QtdExecutada).IsRequired().HasPrecision(18, 8);
        }
    }
}
