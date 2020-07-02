import { Filter } from './Filter.js'
import { MarketValue } from './../DashBoard/MarketValue'


$(function () {
    Filter.setFilterValue();
    MarketValue.init();
    Filter.setBNBMarketValue();

    $("#input-btc").on("keyup", function () {
        var amoutbtc = this.value;
        var precoBNB = Filter.getBNBvalue();
        var qtd = Filter.setAmoutField(amoutbtc / precoBNB);
        $("#input-amount").val(qtd);
        var bitcoinvalue = MarketValue.getBTCtoBRLValue();
        var totalreal = (amoutbtc * bitcoinvalue).toFixed(2);
        $("#input-brl").val(totalreal);
    });

    $("#input-btc").on("change", function () {
        var btcValue = this.value;
        Filter.ValidateBTCField(btcValue);

        var precoBNB = Filter.getBNBvalue();
        var qtd = Filter.setAmoutField(btcValue / precoBNB);
        Filter.ValidateAmountField(qtd);  
        Filter.isValid();
    });

    $("#input-amount").on("keyup", function () {
        var amoutcripto = this.value;
        var precoBNB = Filter.getBNBvalue();
        var btcValue = (amoutcripto * precoBNB).toFixed(8);
        $("#input-btc").val(btcValue);

        var bitcoinvalue = MarketValue.getBTCtoBRLValue();
        var totalreal = (btcValue * bitcoinvalue).toFixed(2);
        $("#input-brl").val(totalreal);
    })

    $("#input-amount").on("change", function () {
        var qtd = this.value;
        Filter.ValidateAmountField(qtd);
        var amoutcripto = this.value;
        var precoBNB = Filter.getBNBvalue();
        var btcValue = (amoutcripto * precoBNB).toFixed(8);
        Filter.ValidateBTCField(btcValue);   
        Filter.isValid();
    })

    $("#input-brl").on("keyup", function () {
        var value = this.value;
        var bitcoinvalue = MarketValue.getBTCtoBRLValue();
        var totalbtc = (value / bitcoinvalue).toFixed(8);
        var precoBNB = Filter.getBNBvalue();
        var qtd = Filter.setAmoutField(totalbtc / precoBNB);
        if (precoBNB * qtd < Filter.getMinNotionalValue()) {
            var LotSize = Filter.getLotSizeValue();
            qtd = Filter.setAmoutField(qtd + LotSize);
        }

        $("#input-amount").val(qtd);
        $("#input-btc").val(totalbtc.trimEnd('0'));
    })

    $("#input-brl").on("change", function () {
        var value = this.value;
        var bitcoinvalue = MarketValue.getBTCtoBRLValue();
        var totalbtc = (value / bitcoinvalue).toFixed(8);
        Filter.ValidateBTCField(totalbtc);
        var precoBNB = Filter.getBNBvalue();
        var qtd = Filter.setAmoutField(totalbtc / precoBNB);
        Filter.ValidateAmountField(qtd);
        Filter.isValid();
    })
})