using DataAccess.Entidades;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityConfig
{
    public class EdicaoAceitaConfiguration : EntityTypeConfiguration<EdicaoAceita>
    {
        public EdicaoAceitaConfiguration()
        {
            HasKey(x => x.Id);
            Property(x => x.ChamadaEditada_ID).IsRequired();
            Property(x => x.TipoEdicao_ID).IsRequired();
            Property(x => x.Usuario_Id).IsRequired();
            Property(x => x.DataCadastro).IsRequired();           
            HasRequired(x => x.Chamada).WithMany(x => x.EdicaoAceita).HasForeignKey(x => x.Chamada_ID);
        }
    }
}
