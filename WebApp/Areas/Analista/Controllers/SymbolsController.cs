using DataAccess.Entidades;
using DataAccess.Helpers;
using DataAccess.Interfaces;
using DataAccess.Serialized_Objects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Helpers;

namespace WebApp.Areas.Analista.Controllers
{

    public class SymbolsController : Controller
    {
        private readonly ISymbolRepository _ISymbolRepo;
        private readonly IFiltersRepository _IFiltersRepo;
        public SymbolsController(ISymbolRepository SymbolRepository, IFiltersRepository FiltersRepository)
        {
            _ISymbolRepo = SymbolRepository;
            _IFiltersRepo = FiltersRepository;
        }
              
        public MyJsonResult checksymbol(string Symbol)
        {
            var result = _ISymbolRepo.GetValidSymbol(Symbol);
            return new MyJsonResult(result, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public MyJsonResult checksymbolName(string SymbolName)
        {
            var result = _ISymbolRepo.GetValidSymbol(SymbolName);
            return new MyJsonResult(result, JsonRequestBehavior.AllowGet);
        }

        public MyJsonResult getPriece(string symbol)
        {
            var price = BinanceHelper.getMarketValue(symbol.ToUpper()).price;
            return new MyJsonResult(price, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(Duration = 600)]
        public MyJsonResult Filter(string symbol)
        {
            var symbolobj = _ISymbolRepo.GetBySymbol(symbol);
            if (symbolobj != null)
            {
                var filters = _IFiltersRepo.GetBySymbol_Id(symbolobj.Id);              
                return new MyJsonResult(filters, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Response.TrySkipIisCustomErrors = true;
                Response.StatusCode = 400;
                return new MyJsonResult("Inválid Symbol.", JsonRequestBehavior.AllowGet);
            }
        }
    }
}