using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Serialized_Objects
{
    public class WSPayload
    {
        public string e { get; set; }  // Event type
        [JsonIgnore]
        public long E { get; set; }  // Event time
        public string s { get; set; }  // Symbol
        public dynamic c { get; set; }  // Client order ID
        public string S { get; set; }  // Side
        public string o { get; set; }  // Order type
        public string f { get; set; }  // Time in force
        public decimal q { get; set; }  // Order quantity
        public decimal p { get; set; }  // Order price
        public decimal P { get; set; }  // Stop price
        [JsonIgnore]
        public decimal F { get; set; }  // Iceberg quantity
        public int g { get; set; }  // OrderListId
        public string C { get; set; }  // Original client order ID; This is the ID of the order being canceled
        public string x { get; set; }  // Current execution type
        public string X { get; set; }  // Current order status
        public string r { get; set; }  // Order reject reason; will be an error code.
        public dynamic i { get; set; }  // Order ID
        public decimal l { get; set; }  // Last executed quantity
        [JsonIgnore]
        public decimal z { get; set; }  // Cumulative filled quantity
        public dynamic L { get; set; }  // Last executed price
        public decimal n { get; set; }  // Commission amount
        public string N { get; set; }  // Commission asset
        [JsonIgnore]
        public long T { get; set; }  // Transaction time
        [JsonIgnore]
        public int t { get; set; }  // Trade ID
        [JsonIgnore]
        public int I { get; set; }  // Ignore
        [JsonIgnore]
        public bool w { get; set; }  // Is the order working? Stops will have
        [JsonIgnore]
        public bool m { get; set; }  // Is this trade the maker side?
        [JsonIgnore]
        public bool M { get; set; }  // Ignore
        [JsonIgnore]
        public long O { get; set; }  // Order creation time
        [JsonIgnore]
        public decimal Z { get; set; }  // Cumulative quote asset transacted quantity
        [JsonIgnore]
        public decimal Y { get; set; }  // Last quote asset transacted quantity (i.e. lastPrice * lastQty)

        [JsonIgnore]
        public int b { get; set; }  // Buyer commission rate (bips)
        [JsonIgnore]
        public bool W { get; set; }  // Can withdraw?
        [JsonIgnore]
        public bool D { get; set; }  // Can deposit?
        [JsonIgnore]
        public long u { get; set; }  // Time of last account update

    }
}
