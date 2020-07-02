using DataAccess.Entidades;
using System.Data.Entity.ModelConfiguration;

namespace DataAccess.EntityConfig
{
    public class UsuarioConfiguration : EntityTypeConfiguration<Usuario>
    {
        public UsuarioConfiguration()
        {
            HasKey(x => x.Id);
            Property(x => x.Email).IsRequired();   
            Property(x => x.Nome).IsRequired();
            Property(x => x.Password).IsRequired().HasMaxLength(128);
            Property(x => x.Sobrenome).IsOptional();
            Property(x => x.DataCadastro).IsRequired();
            Property(x => x.DataVencimentoLicenca).IsRequired(); 
            Property(x => x.Foto).IsOptional();
            Property(x => x.OneSignalIDWeb).IsOptional();
            Property(x => x.OneSignalIDApp).IsOptional();
            Property(x => x.BinanceAPIKey).IsOptional();
            Property(x => x.BinanceAPISecret).IsOptional();
            Property(x => x.IsValidBinanceKeys).IsOptional();
            Property(x => x.AnalistaID).IsOptional();
            HasMany(x => x.Ordems).WithRequired(x => x.Usuario).HasForeignKey(x => x.Usuario_Id);
            HasMany(x => x.PagamentoLicenca).WithRequired(x => x.Usuario).HasForeignKey(x => x.Usuario_Id);
            HasMany(x => x.ConfirmEmail).WithRequired(x => x.Usuario).HasForeignKey(x => x.Usuario_Id);
            HasMany(x => x.EdicoesAceitas).WithRequired(x => x.Usuario).HasForeignKey(x => x.Usuario_Id);
            HasMany(x => x.CancelamentoAceito).WithRequired(x => x.Usuario).HasForeignKey(x => x.Usuario_Id);
            HasMany(x => x.RecuperarSenha).WithRequired(x => x.Usuario).HasForeignKey(x => x.Usuario_Id);
        }
    }
}
