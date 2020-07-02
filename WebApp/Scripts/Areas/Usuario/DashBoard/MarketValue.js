import { WebppAppRedirect } from '../../ComunFunctions.js'

export var MarketValue = (function () {
    var BTCValue;
    var DollarValue;

    return {
        init: function () {
            this.setBTCValue();
            this.setDollarValue();
            var amoutBTC = document.getElementById("display-btc-value").innerText;
            $("#display-brl-value").text("R$ " + this.getBRLAmount(amoutBTC).toFixed(2));
        },

        UpdateMarketValues: function () {
            this.setBTCValue();
            this.setDollarValue();
        },

        setBTCValue: function () {
            $.ajax({
                url: '/usuario/marketvalue/GetBTCValue',
                method: "GET",
                async: false,
                success: function (data) {
                    BTCValue = data;                   
                },
                error: function (data) {
                    WebppAppRedirect(data);
                }
            })
        },

        setDollarValue: function () {
            $.ajax({
                url: '/usuario/marketvalue/GetDollarValue',
                method: "GET",
                async: false,
                success: function (data) {                 
                    DollarValue = data;
                },
                error: function (data) {
                    WebppAppRedirect(data);                   
                }
            })
        },

        getBtcValue: function () {
            return BTCValue;
        },
        getDollarValue: function () {
            return DollarValue;
        },
        getDollarAmout: function (AmountBTC) {
            return AmountBTC * BTCValue;
        },
        getBRLAmount: function (AmoutBTC) {
            var amoutDollar = this.getDollarAmout(AmoutBTC);
            return amoutDollar * DollarValue;
        },
        getBTCtoBRLValue: function () {
            return BTCValue * DollarValue;
        }
    }
})();