import { Filters } from './../Usuario/DashBoard/Filters.js'
import { BinanceTrade } from './BinanceTrade.js'
import "jquery.inputmask/dist/jquery.inputmask.bundle";
import "jquery-validation-unobtrusive";


$(function () {
    var symbolfilter;
    var digits;
    var url;
    $("#SymbolName").on("change", function () {
        var symbol = this.value;
        url = "wss://stream.binance.com:9443/stream?streams=" + symbol + "@@trade";
        if (symbol.length >= 6) {
            $("#PrecoGain").val('');
            $("#PrecoLoss").val('');
            $("#SymbolDescription").val('');
            symbolfilter = Filters.GetFilterValues(symbol);
            if (symbolfilter) {
                var priecefilter = symbolfilter.filter(function (element, index) {
                    return element.filterType == "PRICE_FILTER";
                })[0];

                digits = Intl.NumberFormat([], { minimumFractionDigits: 8 }).format(priecefilter.tickSize).indexOf(1) - 1;
                $("#PrecoEntrada").inputmask('Regex', { regex: "^[0-9]{0,6}(\\.\\d{" + digits + "})?$" });
                $("#PrecoEntrada").inputmask('Regex', { regex: "^[0-9]{0,6}(\\.\\d{" + digits + "})?$" })
                $("#RangeEntrada").inputmask('Regex', { regex: "^[0-9]{0,6}(\\.\\d{" + digits + "})?$" })
                $("#PrecoGain").inputmask('Regex', { regex: "^[0-9]{0,6}(\\.\\d{" + digits + "})?$" })

                if (BinanceTrade.IsConnected() && BinanceTrade.currentUrl() != url) {
                    BinanceTrade.disconnect();
                }

                setTimeout(function () {
                    $("#display-symbol").text(symbol);
                    $("#v-mercado").text("");
                    $.ajax({
                        url: "/Analista/Symbols/getPriece",
                        method: "GET",
                        data: { symbol: symbol }
                    }).done(function (data, textStatus, request) {
                        $("#v-mercado").text(parseFloat(data).toFixed(digits))
                        $("#PrecoEntrada").val(parseFloat(data).toFixed(digits))
                        $("#RangeEntrada").val(parseFloat(data).toFixed(digits))
                        $("#ValorMercado").val(parseFloat(data).toFixed(digits))
                    }).fail(function (erro, b, c) {
                        console.log(erro);
                    })
                    BinanceTrade.connect(symbol, digits);
                }, 1000);
            }
            else {
                $("#display-symbol").text("");
                $("#v-mercado").text("");
                BinanceTrade.disconnect();
            }
        }
    })

    $("#percente-PrecoGain").bind("keyup change", function (e) {
        var value = $(this).val();
        if ((value > 100) && e.keyCode != 46 && e.keyCode != 8) {
            $(this).val(100);
            event.preventDefault();
        }
        var precoentrada = parseFloat($("#PrecoEntrada").val());
        var total = precoentrada + (precoentrada / 100) * value;
        $("#PrecoGain").val(total.toFixed(digits));
    })

    $("#percente-PrecoLoss").bind("keyup change", function (e) {
        var value = $(this).val();
        if ((value > 100) && e.keyCode != 46 && e.keyCode != 8) {
            $(this).val(100);
            event.preventDefault();
        }
        var precoentrada = parseFloat($("#PrecoEntrada").val());
        var total = precoentrada - (precoentrada / 100) * value;
        $("#PrecoLoss").val(total.toFixed(digits));
    })

    $("#PercentualIndicado").bind("keyup change", function (e) {
        var value = $(this).val();
        if ((value > 100) && e.keyCode != 46 && e.keyCode != 8) {
            $(this).val(100);
            event.preventDefault();
        }
        else if ((value < 0) && e.keyCode != 46 && e.keyCode != 8) {
            $(this).val(null);
            event.preventDefault();
        }
    })

    $("form").on("submit", function (el) {
        $("#criar-chamada").attr("disabled", true);
        setTimeout(function () {
            $("#criar-chamada").attr("disabled", false);
        }, 3000);
    })

    if ($("#SymbolName").val()) {
        var symbol = $("#SymbolName").val();
        $("#display-symbol").text(symbol)
        symbolfilter = Filters.GetFilterValues(symbol);
        if (symbolfilter) {
            var priecefilter = symbolfilter.filter(function (element, index) {
                return element.filterType == "PRICE_FILTER";
            })[0];

            digits = Intl.NumberFormat([], { minimumFractionDigits: 8 }).format(priecefilter.tickSize).indexOf(1) - 1;
            $("#PrecoEntrada").inputmask('Regex', { regex: "^[0-9]{0,6}(\\.\\d{" + digits + "})?$" })
            $("#RangeEntrada").inputmask('Regex', { regex: "^[0-9]{0,6}(\\.\\d{" + digits + "})?$" })
            $("#PrecoGain").inputmask('Regex', { regex: "^[0-9]{0,6}(\\.\\d{" + digits + "})?$" })
            $("#PrecoLoss").inputmask('Regex', { regex: "^[0-9]{0,6}(\\.\\d{" + digits + "})?$" })

            $.ajax({
                url: "/Analista/Symbols/getPriece",
                method: "GET",
                data: { symbol: symbol }
            }).done(function (data, textStatus, request) {
                $("#v-mercado").text(parseFloat(data).toFixed(digits))
                $("#ValorMercado").val(parseFloat(data).toFixed(digits))
            }).fail(function (erro, b, c) {
                console.log(erro);
            })
            BinanceTrade.connect(symbol, digits);
        }
    }
});

