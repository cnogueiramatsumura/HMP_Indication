using DataAccess.Interfaces;
using System;
using System.Web.Mvc;
using System.Text;
using System.Security.Cryptography;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Web;
using DataAccess.Serialized_Objects;
using WebApp.Helpers;
using DataAccess.Helpers;
using System.IO;
using DataAccess.Repository;
using DataAccess.Entidades;
using System.Linq;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {

        private IUsuarioRepository _UserRepo;
        private IChamadasRepository _ChamadasRepo;
        private IOrdemRepository _OrdemRepo;
        private IOrdemComissionRepository _OrdemComissionRepo;
        public HomeController(IUsuarioRepository userRepository, IChamadasRepository ChamadasRepository, IOrdemRepository ordemRepository, IOrdemComissionRepository OrdeComission)
        {
            _UserRepo = userRepository;
            _ChamadasRepo = ChamadasRepository;
            _OrdemRepo = ordemRepository;
            _OrdemComissionRepo = OrdeComission;
        }

        public ActionResult Index()
        {          
            return RedirectToAction("Index", "Dashboard", new { Area = "Usuario" });
        }

        public ActionResult testerro()
        {
            throw new Exception("cdadsa");           
        }
    }
}