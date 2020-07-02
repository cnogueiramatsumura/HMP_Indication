[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(WebApp.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(WebApp.App_Start.NinjectWebCommon), "Stop")]

namespace WebApp.App_Start
{
    using System;
    using System.Web;
    using DataAccess.Interfaces;
    using DataAccess.Repository;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using Ninject.Web.Common.WebHost;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
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
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
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
            kernel.Bind<IAnalistaRepository>().To<AnalistaRepository>();
            kernel.Bind<IBinanceStatusRepository>().To<BinanceStatusRepository>();
            kernel.Bind<ICancelamentoChamadaRepository>().To<CancelamentoChamadasRepository>();
            kernel.Bind<ICancelamentoRecusadoRepository>().To<CancelamentoRecusadoRepository>();
            kernel.Bind<IChamadasRepository>().To<ChamadasRepository>();
            kernel.Bind<IChamadaEditadaRepository>().To<ChamadaEditadaRepository>();
            kernel.Bind<IChamadaStatusRepository>().To<ChamadasStatusRepository>();
            kernel.Bind<IConfirmEmailRepository>().To<ConfirmEmailRepository>();
            kernel.Bind<IEdicaoAceitaRepository>().To<EdicaoAceitaRepository>();
            kernel.Bind<IFiltersRepository>().To<FiltersRepository>();
            kernel.Bind<IOrdemRepository>().To<OrdemRepository>();
            kernel.Bind<IOrdemComissionRepository>().To<OrdemComissionRepository>();
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