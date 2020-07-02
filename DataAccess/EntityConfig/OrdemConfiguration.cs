using DataAccess.Entidades;
using System.Data.Entity.ModelConfiguration;

namespace DataAccess.EntityConfig
{
    public class OrdemConfiguration : EntityTypeConfiguration<Ordem>
    {
        public OrdemConfiguration()
        {
            HasKey(x => x.Id);
            Property(x => x.DataEntrada).IsOptional();
            Property(x => x.DataExecucao).IsOptional();
            Property(x => x.DataCancelamento).IsOptional();
            Property(x => x.OrderID).IsOptional();          
            Property(x => x.Quantidade).IsRequired().HasPrecision(18, 8);           
            Property(x => x.LimitOrder_ID).IsOptional().HasMaxLength(10);
            Property(x => x.StopOrder_ID).IsOptional().HasMaxLength(10);
            Property(x => x.MainOrderID).IsOptional();
            Property(x => x.MotivoCancelamento_ID).IsOptional();
            Property(x => x.PrecoVendaMercado).IsOptional().HasPrecision(18,8);
            HasMany(x => x.OrderComissions).WithRequired(x => x.Ordem).HasForeignKey(x => x.Order_Id);

            //Property(x => x.ValorExecutado).IsOptional().HasPrecision(18, 8);
            //Property(x => x.QuantidadeExecutada).IsOptional().HasPrecision(18, 8);
            //Property(x => x.CommissionAmount).IsOptional().HasPrecision(18, 8);
            //Property(x => x.CommissionAsset).IsOptional();


            //HasRequired(x => x.Chamada).WithMany(x => x.Ordems).HasForeignKey(x => x.chamada_Id);
            //HasRequired(x => x.Usuario).WithMany(x => x.Ordems).HasForeignKey(x => x.Usuario_Id);
            //HasRequired(x => x.OrdemStatus).WithMany(x => x.Ordems).HasForeignKey(x => x.OrdemStatus_Id);
            //HasOptional(x => x.BinanceStatus).WithMany(x => x.Ordems).HasForeignKey(x => x.BinanceStatus_Id);
            //HasRequired(x => x.TipoOrdem).WithMany(x => x.Ordems).HasForeignKey(x => x.TipoOrdem_Id);
        }
    }
}
