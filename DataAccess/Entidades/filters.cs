using Newtonsoft.Json;
using System.ComponentModel;

namespace DataAccess.Entidades
{
    public class filters
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string filterType { get; set; }
        //PRICE_FILTER
        public decimal minPrice { get; set; }
        public decimal maxPrice { get; set; }
        public decimal tickSize { get; set; }
        //PERCENT_PRICE   
        public decimal multiplierUp { get; set; }
        public decimal multiplierDown { get; set; }
        //LOT_SIZE & MARKET_LOT_SIZE
        public decimal minQty { get; set; }
        public decimal maxQty { get; set; }
        public decimal stepSize { get; set; }
        //MIN_NOTIONAL
        public decimal minNotional { get; set; }
        public bool applyToMarket { get; set; }
        //MIN_NOTIONAL & PERCENT_PRICE
        public int avgPriceMins { get; set; }
        //ICEBERG_PARTS & MAX_NUM_ORDERS
        public int limit { get; set; }
        //MAX_NUM_ALGO_ORDERS
        public int maxNumAlgoOrders { get; set; }
        //MAX_NUM_ICEBERG_ORDERS
        public int maxNumIcebergOrders { get; set; }     
        public int Symbol_Id { get; set; }


       
        public virtual Symbol Symbol { get; set; }
    }
}
