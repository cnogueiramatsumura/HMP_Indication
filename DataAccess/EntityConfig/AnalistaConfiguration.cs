using DataAccess.Entidades;
using System.Data.Entity.ModelConfiguration;

namespace DataAccess.EntityConfig
{
    public class AnalistaConfiguration : EntityTypeConfiguration<Analista>
    {
        public AnalistaConfiguration()
        {
            HasKey(x => x.Id);
            Property(x => x.Email).IsRequired().HasMaxLength(150);
            Property(x => x.Nome).IsRequired().HasMaxLength(150);
            Property(x => x.Sobrenome).IsOptional();
            Property(x => x.Password).IsRequired().HasMaxLength(128);
            Property(x => x.DataCadastro).IsRequired();
            Property(x => x.Foto).IsOptional();
        }
    }
}