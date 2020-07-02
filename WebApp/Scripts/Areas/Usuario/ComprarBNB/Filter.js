
export const Filter = (function () {
    var BNBFilters;
    var BNBMarketValue;
    var LotSize, MinNotional; 

    return {
        setFilterValue: function () {
            $.ajax({
                url: "/Analista/Symbols/Filter",
                method: "GET",
                data: { symbol: "BNBBTC" },
                async: false
            }).done(function (data) {
                BNBFilters = data;
            }).fail(function (erro) {
                console.log(erro);
            })
            return BNBFilters;
        },

        ValidateBTCField: function (value) {
            var minNotionalFilter = this.GetFilterByType("MIN_NOTIONAL");
            if (value < (minNotionalFilter.minNotional)) {
                MinNotional = false;
                $("#amountvalidademsg").text("O valor em BTC nao pode ser menor que " + (minNotionalFilter.minNotional))
                return false;
            }
            MinNotional = true;
            return true;
        },

        ValidateAmountField: function (qtd) {
            var lotsizeFilter = this.GetFilterByType("LOT_SIZE");
            if (qtd < lotsizeFilter.minQty || !qtd) {
                $("#amountvalidademsg").text("A quantidade nao pode ser menor que " + lotsizeFilter.minQty)
                LotSize = false;
                return false;
            }
            else if (qtd > lotsizeFilter.maxQty) {
                $("#amountvalidademsg").text("A quantidade não pode ser maior que " + lotsizeFilter.maxQty)
                LotSize = false;
                return false;
            }
            else if (lotsizeFilter.stepSize == 1) {
                $("#amountvalidademsg").text("A quantidade não pode ser menor que 1 unidades")
                LotSize = false;
                return false;
            }
            else {
                LotSize = true;
            }
            return LotSize;
        },

        setAmoutField: function (qtd) {
            var lotsizeFilter = this.GetFilterByType("LOT_SIZE");
            var pos = lotsizeFilter.stepSize.toString().indexOf(1);
            if (lotsizeFilter.stepSize == 1) {
                return parseInt(qtd)
            }
            else {
                var numerocasas = (pos - 1);
                //para arredondar para baixo é necessario multiplicar pelo numero que desejas e dividir pelu numero que deseja
                var multiplicador = Math.pow(10, numerocasas);
                var n1 = parseFloat(qtd) * multiplicador;
                var n2 = Math.floor(n1) / multiplicador;
                return parseFloat(n2.toFixed(numerocasas));
            }
        },

        GetFilterByType: function (filterType) {
            var filtro = BNBFilters.filter(function (element, index) {
                return element.filterType == filterType;
            })
            return filtro[0];
        },

        isValid: function () {
            if (LotSize && MinNotional) {
                $("#amountvalidademsg").text("");
                return true;
            }
            else
                return false;
        },

        setBNBMarketValue: function () {
            $.ajax({
                url: "/Analista/Symbols/getPriece",
                method: "GET",
                data: { symbol: "BNBBTC" },
                async: false
            }).done(function (data) {
                BNBMarketValue = data;
            }).fail(function (erro) {
                console.log(erro);
            })
        },

        getBNBvalue: function () {
            return BNBMarketValue;
        },
        getMinNotionalValue: function () {
            var minNotionalFilter = this.GetFilterByType("MIN_NOTIONAL");
            return minNotionalFilter.minNotional;
        },
        getLotSizeValue: function () {
            var LotSize = this.GetFilterByType("LOT_SIZE");
            return LotSize.stepSize;
        }
    }
})();