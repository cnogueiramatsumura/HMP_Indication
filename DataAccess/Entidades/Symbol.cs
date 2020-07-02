using Newtonsoft.Json;
using System.Collections.Generic;

namespace DataAccess.Entidades
{
    public class Symbol
    {
        [JsonProperty(Order = 1)]
        public int Id { get; set; }
        [JsonProperty(Order = 2)]
        public string symbol { get; set; }
        [JsonProperty(Order = 3)]
        public string status { get; set; }
        [JsonProperty(Order = 4)]
        public string baseAsset { get; set; }
        [JsonProperty(Order = 5)]
        public int baseAssetPrecision { get; set; }
        [JsonProperty(Order = 6)]
        public string quoteAsset { get; set; }
        [JsonProperty(Order = 7)]
        public int quotePrecision { get; set; }
        [JsonProperty(Order = 8)]
        public string[] orderTypes { get; set; }
        [JsonProperty(Order =9)]
        public bool icebergAllowed { get; set; }
        [JsonProperty(Order = 10)]
        public bool ocoAllowed { get; set; }
        [JsonProperty(Order = 11)]
        public bool isSpotTradingAllowed { get; set; }
        [JsonProperty(Order = 12)]
        public bool isMarginTradingAllowed { get; set; }         



  
        public virtual ICollection<filters> filters { get; set; }          
        public virtual ICollection<Chamada> chamadas { get; set; }
    }
}
