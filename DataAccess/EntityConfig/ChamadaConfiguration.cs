using DataAccess.Entidades;
using System.Data.Entity.ModelConfiguration;


namespace DataAccess.EntityConfig
{
    public class ChamadaConfiguration : EntityTypeConfiguration<Chamada>
    {
        public ChamadaConfiguration()
        {
            HasKey(x => x.Id);
            Property(x => x.Symbol_id).IsRequired();
            Property(x => x.PrecoMercadoHoraChamada).IsRequired().HasPrecision(18, 8);
            Property(x => x.PrecoEntrada).IsRequired().HasPrecision(18, 8);
            Property(x => x.RangeEntrada).IsRequired().HasPrecision(18, 8);
            Property(x => x.PrecoGain).IsRequired().HasPrecision(18, 8);
            Property(x => x.PrecoLoss).IsRequired().HasPrecision(18, 8);
            Property(x => x.Observacao).IsOptional().HasMaxLength(500);
            Property(x => x.PercentualIndicado).IsOptional();         

            HasMany(x => x.Ordems).WithRequired(x => x.Chamada).HasForeignKey(x => x.Chamada_Id);
            HasMany(x => x.ChamadaEditada).WithRequired(x => x.Chamada).HasForeignKey(x => x.Chamada_Id);
            HasRequired(x => x.Symbol).WithMany(x => x.chamadas).HasForeignKey(x => x.Symbol_id);
            HasOptional(x => x.Analista).WithMany(x => x.chamadas).HasForeignKey(x => x.Analista_Id);
        }
    }
}
