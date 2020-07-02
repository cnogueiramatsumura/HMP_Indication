using DataAccess.Entidades;
using DataAccess.Interfaces;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Helpers;
using WebAPI.Models;
using WebAPI.SignalR;

namespace WebAPI.Controllers
{
    public class CancelamentoChamadaController : ApiController
    {

        private readonly ICancelamentoRecusadoRepository _CancelamentorecusadoRepo;
        private IHubContext _signalContext;

        public CancelamentoChamadaController(ICancelamentoRecusadoRepository CancelamentorecusadoRepository, IHubContext hubcontext)
        {
            _CancelamentorecusadoRepo = CancelamentorecusadoRepository;
            _signalContext = hubcontext;
        }

        [HttpPost]
        [Route("api/CancelamentoChamada/recusar")]
        public HttpResponseMessage recusar(RecusarChamadaViewModel ViewModel)
        {
            try
            {
                if (ViewModel == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotAcceptable, "Campos Inválidos", "text/plain");
                }
                if (ModelState.IsValid)
                {
                    var userId = int.Parse(Helper.GetJWTPayloadValue(Request, "id"));
                    var cancelamentorecusar = new CancelamentoRecusado
                    {
                        Usuario_Id = userId,
                        DataCancelamento = DateTime.UtcNow,
                        CancelamentoChamada_Id = ViewModel.CancelamentoChamada_ID
                    };
                    _CancelamentorecusadoRepo.Add(cancelamentorecusar);
                    return Request.CreateResponse(HttpStatusCode.OK, cancelamentorecusar);
                }
                var modelstateError = ModelStateErrors.DisplayModelStateError(ModelState);
                return Request.CreateResponse(HttpStatusCode.NotAcceptable, modelstateError, "text/plain");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
