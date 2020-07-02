import { WebppAppRedirect } from '../../ComunFunctions.js'

export var Filters = (function () {
    var listFilters;
    var LotSize, MinNotional, LimiteDisponinvel;
 
    return {
        GetFilterValues: function (symbol) {
            $.ajax({
                url: "/Analista/Symbols/Filter",
                method: "GET",
                data: { symbol: symbol },
                async: false
            }).done(function (data) {
                listFilters = data;
            }).fail(function (data) {
                listFilters = null;           
                WebppAppRedirect(data);
            })
            return listFilters;
        },

        ValidateLimitDisponivel: function (value) {
            var btcValue = $("#input-btc").val();
            var saldoDisponivel = parseFloat(document.getElementById("display-btc-value").innerText.replace(",", "."));
            if (btcValue > saldoDisponivel) {
                $("#amountvalidademsg").text("Saldo Indisponível");
                LimiteDisponinvel = false;
            }
            else {
                LimiteDisponinvel = true;
            }
        },

        ValidateBTCField: function (value) {
            var minNotionalFilter = this.GetFilterByType("MIN_NOTIONAL");
            if (value < (minNotionalFilter.minNotional * 2)) {
                MinNotional = false;
                $("#amountvalidademsg").text("O valor em BTC nao pode ser menor que " + (minNotionalFilter.minNotional * 2))
                return false;
            }
            MinNotional = true;
            return true;
        },

        ValidateAmountField: function (chamadaid, qtd) {
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
            else if (lotsizeFilter.stepSize == 1 && qtd < 2) {
                $("#amountvalidademsg").text("A quantidade não pode ser menor que 2 unidades")
                LotSize = false;
                return false;
            }
            else {
                LotSize = true;
            }

            if (qtd && chamadaid) {
                $.ajax({
                    url: "/usuario/chamada/validateLotSize/",
                    method: "GET",
                    data: { id: chamadaid, qtd: qtd },
                    async: false,
                    success: function (result) {
                        if (result.isValid == false) {
                            $("#amountvalidademsg").text(result.msg)
                            LotSize = false;
                        }
                        else {
                            LotSize = true;
                        }
                    },
                    error: function (data) {
                        WebppAppRedirect(data);
                    }
                })
            }
            return LotSize;
        },

        listfilter: function () {
            return listFilters;
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
                return n2.toFixed(numerocasas);
            }
        },

        GetFilterByType: function (filterType) {
            var filtro = listFilters.filter(function (element, index) {
                return element.filterType == filterType;
            })
            return filtro[0];
        },

        isValid: function () {
            if (LotSize && MinNotional && LimiteDisponinvel) {
                $("#amountvalidademsg").text("");
                return true;
            }
            else
                return false;
        }        
    }
})();