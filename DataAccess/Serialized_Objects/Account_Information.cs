using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DataAccess.Serialized_Objects
{
    public class Account_Information
    {
        public int makerCommission { get; set; }
        public int takerCommission { get; set; }
        public int buyerCommission { get; set; }
        public int sellerCommission { get; set; }
        public bool canTrade { get; set; }
        public bool canWithdraw { get; set; }
        public bool canDeposit { get; set; }
        public string updateTime { get; set; }
        public string accountType { get; set; }
        public List<balances> balances { get; set; }
    }

    public class balances
    {
        public string asset { get; set; }
        [DisplayFormat(DataFormatString = "{0:N10}")]
        public double free { get; set; }
        [DisplayFormat(DataFormatString = "{0:N10}")]
        public double locked { get; set; }      
    }
}