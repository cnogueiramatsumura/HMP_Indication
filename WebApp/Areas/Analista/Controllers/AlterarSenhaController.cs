using DataAccess.Interfaces;
using DataAccess.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WebApp.ActionFilters.Analista;
using WebApp.Areas.Usuario.Controllers;
using WebApp.Helpers;

namespace WebApp.Areas.Analista.Controllers
{
    [AnalistaAuthorize]
    public class AlterarSenhaController : MyBaseController
    {
        private readonly IAnalistaRepository _AnalistaRepository;
        public AlterarSenhaController(IAnalistaRepository AnalistaRepository)
        {
            _AnalistaRepository = AnalistaRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(AlterarSenhaViewModel model)
        {
            if (ModelState.IsValid)
            {
                var analista = _AnalistaRepository.GetByEmail(User.Identity.Name);
                if (analista != null)
                {
                    analista.Password = Hashing.HashPassword(model.NovaSenha);
                    _AnalistaRepository.Update(analista);
                    return View("MudarSenha");
                }
            }
            return View(model);
        }
    }
}