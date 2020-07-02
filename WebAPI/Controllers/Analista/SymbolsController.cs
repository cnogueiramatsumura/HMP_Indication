using DataAccess.Entidades;
using DataAccess.Helpers;
using DataAccess.Interfaces;
using DataAccess.Serialized_Objects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Http;
using WebApi.OutputCache.V2;
using WebAPI.Helpers;

namespace WebAPI.Controllers.Analista
{
    public class SymbolsController : ApiController
    {
        private readonly ISymbolRepository _ISymbolRepo;
        private readonly IFiltersRepository _IFiltersRepo;
        public SymbolsController(ISymbolRepository SymbolRepository, IFiltersRepository FiltersRepository)
        {
            _ISymbolRepo = SymbolRepository;
            _IFiltersRepo = FiltersRepository;
        }

        [HttpGet]
        [Route("api/symbols/IniciarBancoVazio")]
        public HttpResponseMessage IniciarBancoVazio()
        {
            try
            {
                var res = BinanceRestApi.GetExchangeInfo();
                List<filters> filterstoAdd = new List<filters>();
                List<Symbol> SymboltoAdd = new List<Symbol>();
                if (res.IsSuccessStatusCode)
                {

                    var content = res.Content.ReadAsStringAsync().Result;
                    var jsonObj = JsonConvert.DeserializeObject<ExchangeInfo>(content);
                    _ISymbolRepo.AddRange(jsonObj.symbols.ToList());
                    //foreach (var item in jsonObj.symbols)
                    //{
                    //    var symbol = new Symbol();
                    //    symbol.symbol = item.symbol.ToLower();
                    //    symbol.status = item.status;
                    //    symbol.baseAsset = item.baseAsset;
                    //    symbol.baseAssetPrecision = item.baseAssetPrecision;
                    //    symbol.quoteAsset = item.quoteAsset;
                    //    symbol.quotePrecision = item.quotePrecision;
                    //    symbol.orderTypes = item.orderTypes;
                    //    symbol.icebergAllowed = item.icebergAllowed;
                    //    symbol.ocoAllowed = item.ocoAllowed;
                    //    symbol.isSpotTradingAllowed = item.isSpotTradingAllowed;
                    //    symbol.isMarginTradingAllowed = item.isMarginTradingAllowed;
                    //    foreach (var filteritem in item.filters)
                    //    {
                    //        var filters = new filters();
                    //        filters.Symbol_Id = symbol.Id;
                    //        filters.filterType = filteritem.filterType;
                    //        //PRICE_FILTER
                    //        filters.minPrice = filteritem.minPrice;
                    //        filters.maxPrice = filteritem.maxPrice;
                    //        filters.tickSize = filteritem.tickSize;
                    //        //PERCENT_PRICE
                    //        filters.multiplierUp = filteritem.multiplierUp;
                    //        filters.multiplierDown = filteritem.multiplierDown;
                    //        //LOT_SIZE & MARKET_LOT_SIZE
                    //        filters.minQty = filteritem.minQty;
                    //        filters.maxQty = filteritem.maxQty;
                    //        filters.stepSize = filteritem.stepSize;
                    //        //MIN_NOTIONAL
                    //        filters.minNotional = filteritem.minNotional;
                    //        filters.applyToMarket = filteritem.applyToMarket;
                    //        //MIN_NOTIONAL & PERCENT_PRICE
                    //        filters.avgPriceMins = filteritem.avgPriceMins;
                    //        //ICEBERG_PARTS & MAX_NUM_ORDERS
                    //        filters.limit = filteritem.limit;
                    //        //MAX_NUM_ALGO_ORDERS
                    //        filters.maxNumAlgoOrders = filteritem.maxNumAlgoOrders;
                    //        //MAX_NUM_ICEBERG_ORDERS
                    //        filters.maxNumIcebergOrders = filteritem.maxNumIcebergOrders;
                    //        filterstoAdd.Add(filters);
                    //    }                     
                    //    symbol.filters = filterstoAdd;
                    //    _ISymbolRepo.Add(symbol);
                    //    _ISymbolRepo.Detach(symbol);
                    //    filterstoAdd.Clear();
                    //_IFiltersRepo.AddRange(filterstoAdd);
                    //}
                    return Request.CreateResponse(HttpStatusCode.OK, jsonObj.symbols.Count());
                }
                else
                {
                    var BinanceErrorObj = Helper.GetBinanceErrorObj(res);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, BinanceErrorObj);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message + " => inner => " + ex.InnerException.Message);
            }
        }

        [HttpGet]
        [Route("api/symbols/AtualizarBanco")]
        public HttpResponseMessage AtualizarBanco()
        {
            try
            {
                var res = BinanceRestApi.GetExchangeInfo();
                //var jsonObj = JsonConvert.DeserializeObject<ExchangeInfo>(content, new JsonSerializerSettings() { DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore });             
                if (res.IsSuccessStatusCode)
                {
                    var content = res.Content.ReadAsStringAsync().Result;
                    var jsonObj = JsonConvert.DeserializeObject<ExchangeInfo>(content);
                    foreach (var item in jsonObj.symbols)
                    {
                        var dbsymbol = _ISymbolRepo.GetBySymbol(item.symbol.ToLower());
                        if (dbsymbol == null)
                        {
                            var symbol = new Symbol();
                            symbol.symbol = item.symbol.ToLower();
                            symbol.status = item.status;
                            symbol.baseAsset = item.baseAsset;
                            symbol.baseAssetPrecision = item.baseAssetPrecision;
                            symbol.quoteAsset = item.quoteAsset;
                            symbol.quotePrecision = item.quotePrecision;
                            symbol.orderTypes = item.orderTypes;
                            symbol.icebergAllowed = item.icebergAllowed;
                            symbol.ocoAllowed = item.ocoAllowed;
                            symbol.isSpotTradingAllowed = item.isSpotTradingAllowed;
                            symbol.isMarginTradingAllowed = item.isMarginTradingAllowed;
                            _ISymbolRepo.Add(symbol);
                            foreach (var filteritem in item.filters)
                            {
                                var filters = new filters();
                                filters.Symbol_Id = symbol.Id;
                                filters.filterType = filteritem.filterType;
                                //PRICE_FILTER
                                filters.minPrice = filteritem.minPrice;
                                filters.maxPrice = filteritem.maxPrice;
                                filters.tickSize = filteritem.tickSize;
                                //PERCENT_PRICE
                                filters.multiplierUp = filteritem.multiplierUp;
                                filters.multiplierDown = filteritem.multiplierDown;
                                //LOT_SIZE & MARKET_LOT_SIZE
                                filters.minQty = filteritem.minQty;
                                filters.maxQty = filteritem.maxQty;
                                filters.stepSize = filteritem.stepSize;
                                //MIN_NOTIONAL
                                filters.minNotional = filteritem.minNotional;
                                filters.applyToMarket = filteritem.applyToMarket;
                                //MIN_NOTIONAL & PERCENT_PRICE
                                filters.avgPriceMins = filteritem.avgPriceMins;
                                //ICEBERG_PARTS & MAX_NUM_ORDERS
                                filters.limit = filteritem.limit;
                                //MAX_NUM_ALGO_ORDERS
                                filters.maxNumAlgoOrders = filteritem.maxNumAlgoOrders;
                                //MAX_NUM_ICEBERG_ORDERS
                                filters.maxNumIcebergOrders = filteritem.maxNumIcebergOrders;
                                _IFiltersRepo.Add(filters);
                            }
                        }
                        else
                        {
                            var updatesymbol = dbsymbol;
                            updatesymbol.status = item.status;
                            updatesymbol.baseAsset = item.baseAsset;
                            updatesymbol.baseAssetPrecision = item.baseAssetPrecision;
                            updatesymbol.quoteAsset = item.quoteAsset;
                            updatesymbol.quotePrecision = item.quotePrecision;
                            updatesymbol.orderTypes = item.orderTypes;
                            updatesymbol.icebergAllowed = item.icebergAllowed;
                            updatesymbol.ocoAllowed = item.ocoAllowed;
                            updatesymbol.isSpotTradingAllowed = item.isSpotTradingAllowed;
                            updatesymbol.isMarginTradingAllowed = item.isMarginTradingAllowed;
                            if (updatesymbol != dbsymbol)
                            {
                                _ISymbolRepo.Update(dbsymbol);
                            }

                            foreach (var filteritem in item.filters)
                            {
                                var oldFilter = _IFiltersRepo.GetBySymbol_Type(filteritem.filterType, updatesymbol.Id);
                                var Updatefilters = (oldFilter != null) ? oldFilter : new filters();
                                Updatefilters.Symbol_Id = updatesymbol.Id;
                                Updatefilters.filterType = filteritem.filterType;
                                //PRICE_FILTER
                                Updatefilters.minPrice = filteritem.minPrice;
                                Updatefilters.maxPrice = filteritem.maxPrice;
                                Updatefilters.tickSize = filteritem.tickSize;
                                //PERCENT_PRICE
                                Updatefilters.multiplierUp = filteritem.multiplierUp;
                                Updatefilters.multiplierDown = filteritem.multiplierDown;
                                //LOT_SIZE & MARKET_LOT_SIZE
                                Updatefilters.minQty = filteritem.minQty;
                                Updatefilters.maxQty = filteritem.maxQty;
                                Updatefilters.stepSize = filteritem.stepSize;
                                //MIN_NOTIONAL
                                Updatefilters.minNotional = filteritem.minNotional;
                                Updatefilters.applyToMarket = filteritem.applyToMarket;
                                //MIN_NOTIONAL & PERCENT_PRICE
                                Updatefilters.avgPriceMins = filteritem.avgPriceMins;
                                //ICEBERG_PARTS & MAX_NUM_ORDERS
                                Updatefilters.limit = filteritem.limit;
                                //MAX_NUM_ALGO_ORDERS
                                Updatefilters.maxNumAlgoOrders = filteritem.maxNumAlgoOrders;
                                //MAX_NUM_ICEBERG_ORDE    RS
                                Updatefilters.maxNumIcebergOrders = filteritem.maxNumIcebergOrders;
                                if (Updatefilters != oldFilter && oldFilter != null)
                                {
                                    _IFiltersRepo.Update(Updatefilters);
                                }
                                else if (oldFilter == null)
                                {
                                    _IFiltersRepo.Add(Updatefilters);
                                }
                            }
                        }
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, jsonObj.symbols.Count());
                }
                else
                {
                    var BinanceErrorObj = Helper.GetBinanceErrorObj(res);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, BinanceErrorObj);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

    }
}