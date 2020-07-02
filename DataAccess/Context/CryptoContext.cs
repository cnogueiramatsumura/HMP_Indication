using DataAccess.Entidades;
using DataAccess.EntityConfig;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace DataAccess.Context
{
    public class CryptoContext : DbContext
    {
        public CryptoContext()
            : base("name=ProjetcCrypto")
        {
            this.Configuration.LazyLoadingEnabled = false;                       
        }
        public virtual DbSet<Analista> analista { get; set; }
        public virtual DbSet<BinanceStatus> binanceStatus { get; set; }       
        public virtual DbSet<CancelamentoChamada> cancelamentoChamada { get; set; }
        public virtual DbSet<CancelamentoRecusado> cancelamentoRecusado { get; set; }
        public virtual DbSet<Chamada> chamada { get; set; }
        public virtual DbSet<ChamadaEditada> chamadaEditada { get; set; }
        public virtual DbSet<ChamadasRecusadas> chamadasRecusadas { get; set; }
        public virtual DbSet<ChamadaStatus> chamadastatus { get; set; }
        public virtual DbSet<ConfirmEmail> confirmEmail { get; set; }
        public virtual DbSet<EdicaoAceita> edicaoAceita { get; set; }
        public virtual DbSet<filters> filters { get; set; }
        public virtual DbSet<MetodoPagamento> metodoPagamento { get; set; }
        public virtual DbSet<MotivoCancelamento> motivoCancelamento { get; set; }
        public virtual DbSet<Ordem> ordem { get; set; }
        public virtual DbSet<OrdemComission> ordemComission { get; set; }
        public virtual DbSet<OrdemStatus> ordemStatus { get; set; }
        public virtual DbSet<PagamentoLicenca> pagamentoLicenca { get; set; }
        public virtual DbSet<PagamentoLicencaStatus> pagamentoLicencastatus { get; set; }
        public virtual DbSet<RecuperarSenha> recuperarSenha { get; set; }
        public virtual DbSet<ResultadoChamada> resultadoChamada { get; set; }
        public virtual DbSet<ServerConfig> serverConfig { get; set; }
        public virtual DbSet<Symbol> symbol { get; set; }
        public virtual DbSet<TipoEdicaoAceita> tipoEdicaoAceita { get; set; }
        public virtual DbSet<TipoOrdem> tipoOrdem { get; set; }
        public virtual DbSet<Usuario> usuario { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {           

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Properties<string>().Configure(x => x.HasColumnType("varchar").HasMaxLength(100));


            modelBuilder.Configurations.Add(new AnalistaConfiguration());
            modelBuilder.Configurations.Add(new BinanceStatusConfiguration());        
            modelBuilder.Configurations.Add(new CancelamentoChamadaConfiguration());
            modelBuilder.Configurations.Add(new CancelamentoRecusadoConfiguration());
            modelBuilder.Configurations.Add(new ChamadaConfiguration());
            modelBuilder.Configurations.Add(new ChamadaEditadaConfiguration());
            modelBuilder.Configurations.Add(new ChamadasRecusadasConfiguration());
            modelBuilder.Configurations.Add(new ChamadaStatusConfiguration());
            modelBuilder.Configurations.Add(new ConfirmEmailConfiguration());
            modelBuilder.Configurations.Add(new EdicaoAceitaConfiguration());
            modelBuilder.Configurations.Add(new filtersConfiguration());
            modelBuilder.Configurations.Add(new MetodoPagamentoConfiguration());
            modelBuilder.Configurations.Add(new MotivoCancelamentoConfiguration());
            modelBuilder.Configurations.Add(new OrdemConfiguration());
            modelBuilder.Configurations.Add(new OrdemComissionConfiguration());
            modelBuilder.Configurations.Add(new OrdemStatusConfiguration());
            modelBuilder.Configurations.Add(new PagamentoLicencaConfiguration());
            modelBuilder.Configurations.Add(new PagamentoLicencaStatusConfiguration());
            modelBuilder.Configurations.Add(new RecuperarSenhaConfiguration());
            modelBuilder.Configurations.Add(new ResultadoChamadaConfiguration());
            modelBuilder.Configurations.Add(new ServerConfigConfiguration());
            modelBuilder.Configurations.Add(new SymbolConfiguration());
            modelBuilder.Configurations.Add(new TipoEdicaoAceitaConfiguration());
            modelBuilder.Configurations.Add(new TipoOrdemConfiguration());
            modelBuilder.Configurations.Add(new UsuarioConfiguration());            
        }
    }
}
