using DataAccess.Helpers;
using DataAccess.Interfaces;
using DataAccess.Serialized_Objects;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebApp.ActionFilters;
using WebApp.Areas.Usuario.Models.DashBoard;
using WebApp.Helpers;

namespace WebApp.Areas.Usuario.Controllers
{

    [UsuarioAuthorize]
    public class DashBoardController : MyBaseController
    {
        private readonly IChamadasRepository _ChamadasRepo;
        private readonly IUsuarioRepository _UserRepo;
        private readonly IOrdemRepository _OrdemRepo;
        private readonly IChamadaEditadaRepository _ChamadaEditadaRepoRepo;
        private readonly IServerConfigRepository _ServerConfigRepo;
        private readonly ICancelamentoChamadaRepository _cancelamentoChamadaRepository;
        public DashBoardController(IChamadasRepository ChamadasRepository, IUsuarioRepository userRepository, IOrdemRepository OrdemRepository, IChamadaEditadaRepository ChamadasEditadasRepository, IServerConfigRepository _ServerConfigRepository, ICancelamentoChamadaRepository cancelamentoChamadaRepository)
        {
            _ChamadasRepo = ChamadasRepository;
            _UserRepo = userRepository;
            _OrdemRepo = OrdemRepository;
            _ChamadaEditadaRepoRepo = ChamadasEditadasRepository;
            _ServerConfigRepo = _ServerConfigRepository;
            _cancelamentoChamadaRepository = cancelamentoChamadaRepository;
        }

        [RedirectToActionPayment]
        [GetLimitBTC]
        public ActionResult Index()
        {
            var user = _UserRepo.GetByEmail(User.Identity.Name);
            var dashviewmodel = new DashBoardViewModel()
            {
                Chamadas = _ChamadasRepo.GetAllOpen(user.Id),
                ChamadaEditadas = _ChamadaEditadaRepoRepo.GetAllOpen(user.Id),
                ChamadasCanceladas = _cancelamentoChamadaRepository.GetAllOpen(user.Id),             
                Ordems = (List<ActionOrders>)_OrdemRepo.SelecionarPosicionadas(user.Id)
            };        
            var config = _ServerConfigRepo.GetAllConfig();
            ViewBag.ApiDomainName = config.ApiServer;
            ViewBag.OneSignalAppId = config.OneSignalAppId;
       
            return View(dashviewmodel);
        }

        [RedirectToActionKeys]
        public async Task<ActionResult> Limites()
        {           
            var user = _UserRepo.GetByEmail(User.Identity.Name);
            ViewBag.datavencimento = user.DataVencimentoLicenca.ToString("dd/MM/yyyy");
            ViewBag.username = user.Nome;
            var apicontent = BinanceRestApi.GetAccountInformation(user.BinanceAPIKey, user.BinanceAPISecret);
            if (apicontent.IsSuccessStatusCode)
            {
                var result = await apicontent.Content.ReadAsStringAsync();
                var AccounInformation = JsonConvert.DeserializeObject<Account_Information>(result);
                ViewBag.BTC = AccounInformation.balances.Where(x => x.asset == "BTC").Select(x => x.free).FirstOrDefault();
                var symbolsContent = BinanceRestApi.SymbolsPriece();
                if (symbolsContent.IsSuccessStatusCode)
                {
                    var symbolResult = await symbolsContent.Content.ReadAsStringAsync();
                    var symbolsInformation = JsonConvert.DeserializeObject<List<SymbolTicker>>(symbolResult);

                    var viewmodel = new LimitesViewModel
                    {
                        balances = AccounInformation.balances.Where(x => x.free != 0 || x.locked != 0).OrderByDescending(x => x.asset == "BTC").ThenByDescending(x => x.free).ToList(),
                        symbolTickers = symbolsInformation
                    };                    
                    return View(viewmodel);
                }
            }         
            return RedirectToAction("BinanceApiError", "Errors", new { area = "Usuario" });
        }               
    }
}