using DataAccess.Entidades;
using DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.OutputCache.V2;

namespace WebAPI.Controllers
{
    [Authorize]
    public class FiltersController : ApiController
    {      
        private readonly IFiltersRepository _IFiltersRepo;
        public FiltersController(ISymbolRepository SymbolRepository, IFiltersRepository FiltersRepository)
        {        
            _IFiltersRepo = FiltersRepository;
        }


        [CacheOutput(ServerTimeSpan = 600)]
        public HttpResponseMessage Get(int id)
        {
            try
            {
                var ListFilter = _IFiltersRepo.GetBySymbol_Id(id);
                return Request.CreateResponse(HttpStatusCode.OK, ListFilter);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
