using DataAccess.Entidades;
using DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApp.ActionFilters.Analista;
using WebApp.Areas.Analista.Models;
using WebApp.Helpers;

namespace WebApp.Areas.Analista.Controllers
{
    [AnalistaAuthorize(Roles = "Analista")]
    public class DashboardController : Controller
    {
        private readonly IServerConfigRepository _serverConfigRepo;
        public DashboardController(IServerConfigRepository serverConfigRepository)
        {
            _serverConfigRepo = serverConfigRepository;
        }
        public async Task<ActionResult> Index()
        {
            var config = _serverConfigRepo.GetAllConfig();
            var ViewModel = new DashboardViewModel
            {
                Serverconfig = config
            };
            if (config.AppServer != "http://localhost")
            {
                var uri = new Uri(config.AppServer);
                using (HttpClient client = new HttpClient())
                {
                    using (HttpResponseMessage response = await client.GetAsync(uri))
                    {
                        var cert = ServicePointManager.FindServicePoint(uri).Certificate;
                        ViewModel.DataExpiracaoSSL = Convert.ToDateTime(cert.GetExpirationDateString());
                    }
                }
            }        
            return View(ViewModel);
        }
    }
}