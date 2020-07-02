import { Filters } from '../Usuario/DashBoard/Filters.js'
import "jquery.inputmask/dist/jquery.inputmask.bundle";

$(function () {
    var symbolfilter = Filters.GetFilterValues(symbol);
    var priecefilter = symbolfilter.filter(function (element, index) {
        return element.filterType == "PRICE_FILTER";
    })[0];
    var digits = Intl.NumberFormat([], { minimumFractionDigits: 8 }).format(priecefilter.tickSize).indexOf(1) - 1;
    $("#NewGain").inputmask('Regex', { regex: "^[0-9]{0,6}(\\.\\d{" + digits + "})?$" })
    $("#NewLoss").inputmask('Regex', { regex: "^[0-9]{0,6}(\\.\\d{" + digits + "})?$" })


    $("form").on("submit", function () {
        var valormercado = $("#valor-mercado").text();
        var NewGain = $("#NewGain").val();
        var NewLoss = $("#NewLoss").val();

        if (NewGain < valormercado || NewLoss > valormercado) {
            $(".display-msg").text("Valores Inválidos");
            event.preventDefault();
        }
    })

    $.ajax({
        url: "/Analista/symbols/getPriece",
        data: { "symbol": symbol },
        success: function (marketvalue) {
            $("#valor-mercado").text(parseFloat(marketvalue).toFixed(8));
            if (marketvalue > _entrada) {
                var maxvalue = _gain - _entrada;
                var qtd_na_escala = marketvalue - _entrada;
                var variacaopercentual = parseInt((qtd_na_escala / maxvalue) * 100);
                $("#progress-gain").css("width", (variacaopercentual >= 100) ? "100%" : variacaopercentual + "%");
                $("#progress-loss").css("width", 0);
            }
            else if (marketvalue < _entrada) {
                var maxvalue = _entrada - _loss;
                var qtd_na_escala = _entrada - marketvalue;
                var variacaopercentual = parseInt((qtd_na_escala / maxvalue) * 100);
                $("#progress-loss").css("width", (variacaopercentual >= 100) ? "100%" : variacaopercentual + "%");
                $("#progress-gain").css("width", 0);
            }
            else if (marketvalue == _entrada) {
                $("#progress-gain").css("width", 0);
                $("#progress-loss").css("width", 0);
            }
        }
    });

    var ws = new WebSocket("wss://stream.binance.com:9443/stream?streams=" + symbol.toLowerCase() + "@trade");
    ws.onmessage = function (obj) {
        var marketvalue = parseFloat(JSON.parse(obj.data).data.p).toFixed(8);
        if (marketvalue > _entrada) {
            var maxvalue = _gain - _entrada;
            var qtd_na_escala = marketvalue - _entrada;
            var variacaopercentual = parseInt((qtd_na_escala / maxvalue) * 100);
            $("#progress-gain").css("width", (variacaopercentual >= 100) ? "100%" : variacaopercentual + "%");
            $("#progress-loss").css("width", 0);
        }
        else if (marketvalue < _entrada) {
            var maxvalue = _entrada - _loss;
            var qtd_na_escala = _entrada - marketvalue;
            var variacaopercentual = parseInt((qtd_na_escala / maxvalue) * 100);
            $("#progress-loss").css("width", (variacaopercentual >= 100) ? "100%" : variacaopercentual + "%");
            $("#progress-gain").css("width", 0);
        }
        else if (marketvalue == _entrada) {
            $("#progress-gain").css("width", 0);
            $("#progress-loss").css("width", 0);
        }
        if (marketvalue > _gain || marketvalue < _loss) {
            $("form").addClass("d-none");
            $(".display-msg").text("Operação Finalizada");
            ws.close();
        }
        $("#valor-mercado").text(marketvalue)
    }
    ws.onclose = function (msg) {
        console.log("Operação finalizada");
    }
}); 