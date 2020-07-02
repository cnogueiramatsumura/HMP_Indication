using DataAccess.Entidades;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityConfig
{
    public class ConfirmEmailConfiguration : EntityTypeConfiguration<ConfirmEmail>
    {
        public ConfirmEmailConfiguration()
        {
            HasKey(x => x.Id);
            Property(x => x.Token).IsRequired();
            Property(x => x.Usuario_Id).IsRequired();          
        }
    }
}
