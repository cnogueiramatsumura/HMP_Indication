[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(WebAPI.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(WebAPI.App_Start.NinjectWebCommon), "Stop")]

namespace WebAPI.App_Start
{
    using DataAccess.Interfaces;
    using DataAccess.Repository;
    using Microsoft.AspNet.SignalR;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Newtonsoft.Json;
    using Ninject;
    using Ninject.Web.Common;
    using System;
    using System.Web;
    using WebAPI.SignalR;
    using WebAPI.SignalR.SerializeSettings;

    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            #region Serialize Settings
            //Aplicar a serializaçao de nomes para camelse
            var settings = new JsonSerializerSettings();
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;         
            settings.ContractResolver = new SignalRContractResolver();
            var serializer = JsonSerializer.Create(settings);
            kernel.Bind<JsonSerializer>().ToConstant(serializer);
            #endregion

            #region Resolver Dependency SignalR
            //Aplica Resoluçao de nomes para Injeçao de dependencia dentro do signalR
            GlobalHost.DependencyResolver = new NinjectSignalRDependencyResolver(kernel);
            #endregion
            //Coloca o Contexto pra receber injeçao de dependencia
            kernel.Bind<IHubContext>().ToMethod(_ => GlobalHost.ConnectionManager.GetHubContext<SignalChamadas>());

            //data acess binds
            kernel.Bind<IAnalistaRepository>().To<AnalistaRepository>();
            kernel.Bind<IBinanceStatusRepository>().To<BinanceStatusRepository>();
            kernel.Bind<ICancelamentoChamadaRepository>().To<CancelamentoChamadasRepository>();
            kernel.Bind<ICancelamentoRecusadoRepository>().To<CancelamentoRecusadoRepository>();
            kernel.Bind<IChamadasRecusadasRepository>().To<ChamadasRecusadasRepository>();
            kernel.Bind<IChamadasRepository>().To<ChamadasRepository>();
            kernel.Bind<IChamadaEditadaRepository>().To<ChamadaEditadaRepository>();
            kernel.Bind<IChamadaStatusRepository>().To<ChamadasStatusRepository>();
            kernel.Bind<IConfirmEmailRepository>().To<ConfirmEmailRepository>();
            kernel.Bind<IEdicaoAceitaRepository>().To<EdicaoAceitaRepository>();
            kernel.Bind<IFiltersRepository>().To<FiltersRepository>();
            kernel.Bind<IOrdemComissionRepository>().To<OrdemComissionRepository>();
            kernel.Bind<IOrdemRepository>().To<OrdemRepository>();
            kernel.Bind<IOrdemStatusRepository>().To<OrdemStatusRepository>();
            kernel.Bind<IPagamentoLicencaRepository>().To<PagamentoLicencaRepository>();
            kernel.Bind<IRecuperarSenha>().To<RecuperarSenhaRepository>();
            kernel.Bind<IServerConfigRepository>().To<ServerConfigRepository>();
            kernel.Bind<ISymbolRepository>().To<SymbolRepository>();
            kernel.Bind<ITipoEdicaoAceitaRepository>().To<TipoEdicaoAceitaRepository>();
            kernel.Bind<ITipoOrdemRepository>().To<TipoOrdemRepository>();
            kernel.Bind<IUsuarioRepository>().To<UsuarioRepository>();
        }
    }
}