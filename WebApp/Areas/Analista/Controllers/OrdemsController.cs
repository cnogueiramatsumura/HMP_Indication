using DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.ActionFilters.Analista;
using WebApp.Areas.Analista.Models;

namespace WebApp.Areas.Analista.Controllers
{
    [AnalistaAuthorize(Roles = "Analista")]
    public class OrdemsController : Controller
    {
        private readonly IChamadasRepository _ChamadasRepo;
        public OrdemsController(IChamadasRepository chamadasRepository)
        {
            _ChamadasRepo = chamadasRepository;
        }
    }
}