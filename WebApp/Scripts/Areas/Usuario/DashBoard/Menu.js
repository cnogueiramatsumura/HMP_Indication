import { PercentualGain, PercentualLoss, showTime } from '../../ComunFunctions.js'
export var Menu = (function () {
    return {
        ClearIcon: function () {
            var iconEntrada = document.getElementById("icon-entrada")
            var iconGain = document.getElementById("icon-gain")
            var iconLoss = document.getElementById("icon-loss")
            iconEntrada.innerText = "";
            iconGain.innerText = "";
            iconLoss.innerText = "";
        },

        IconsAguardandoEntrada: function () {
            $("#icon-entrada").append("<i class='uil uil-bitcoin-circle'></i>");
            $("#icon-gain").append("<i class='uil uil-clock'></i>");
            $("#icon-loss").append("<i class='uil uil-clock'></i>");
        },

        IconsEntradaRealizada: function () {
            $("#icon-entrada").append("<i class='uil uil-angle-down'></i>");
            $("#icon-gain").append("<i class='uil uil-bitcoin-circle'></i>");
            $("#icon-loss").append("<i class='uil uil-bitcoin-circle'></i>");
        },

        IconsGainRealizado: function () {
            $("#icon-entrada").append("<i class='uil uil-check'></i>");
            $("#icon-gain").append("<i class='uil uil-check'></i>");
            $("#icon-loss").append("<i class='uil uil-times'></i>");
        },

        IconLossRealizado: function () {
            $("#icon-entrada").append("<i class='uil uil-check'></i>");
            $("#icon-loss").append("<i class='uil uil-check'></i>");
            $("#icon-gain").append("<i class='uil uil-times'></i>");
        },

        ShowPosicionadasContent: function () {
            $('#show-observacao').popover('hide');
            $(".chamadas-content").css("display", "none");
            $(".posicionadas-content").css("display", "block");
        },

        ShowChamadasContent: function () {
            $(".chamadas-content").css("display", "block");
            $(".posicionadas-content").css("display", "none");
        },

        ShowBtnIndicacao: function () {
            document.getElementById("btn-executar-indicacao").classList.remove("d-none");
            document.getElementById("btn-cancelar-entrada").classList.add("d-none");
            document.getElementById("btn-sair-mercado").classList.add("d-none");
        },

        ShowBtnCancelarEntrada: function () {
            document.getElementById("btn-cancelar-entrada").classList.remove("d-none");
            document.getElementById("btn-executar-indicacao").classList.add("d-none");
            document.getElementById("btn-sair-mercado").classList.add("d-none");
        },

        ShowBtnSairMercado: function () {
            document.getElementById("btn-executar-indicacao").classList.add("d-none");
            document.getElementById("btn-cancelar-entrada").classList.add("d-none");
            document.getElementById("btn-sair-mercado").classList.remove("d-none");
        },

        HideAllActionButtons: function () {
            document.getElementById("btn-executar-indicacao").classList.add("d-none");
            document.getElementById("btn-cancelar-entrada").classList.add("d-none");
            document.getElementById("btn-sair-mercado").classList.add("d-none");
        },

        HideOrdemPercentualButtons: function () {
            document.querySelector(".botoespercentual").classList.add("invisible");
            document.querySelector(".range-slider").classList.add("invisible");
            document.getElementById("input-amount").disabled = true;
            document.getElementById("input-brl").disabled = true;
            document.getElementById("input-btc").disabled = true;
            document.querySelector("#range-input").value = 0
        },

        ShowOrdemPercentualButtons: function () {
            document.querySelector(".botoespercentual").classList.remove("invisible");
            document.querySelector(".range-slider").classList.remove("invisible");
            document.getElementById("input-amount").disabled = false;
            document.getElementById("input-brl").disabled = false;
            document.getElementById("input-btc").disabled = false;
        },

        ClearInputs: function () {
            $("#input-amount").val("");
            $("#input-brl").val("");
            $("#input-btc").val("");
            $("#amountvalidademsg").text("");
        },

        AtualizarHeader: function (horario, symbol, entrada, rangeEntrada, gain, loss) {
            $("#tempo-chamada").text(showTime(horario));
            $("#nome-ativo").text(symbol.toUpperCase());
            $("#valor-mercado").text("");
            $(".entrada").text(entrada);
            $("#rangeentrada").text(rangeEntrada);
            $(".loss").text(loss);
            $(".gain").text(gain);
        },

        AtualizarPercentuais: function (entrada, gain, loss) {
            $("#valor-objetivo").html(PercentualGain(entrada, gain) + "% <i class='fa fa-caret-up'></i>");
            $("#valor-risco").html(PercentualLoss(entrada, loss) + "% <i class='fa fa-caret-down'></i>");
        },

        AtualizarCardValores: function (qtd, amoutbtc, totalreal) {
            $("#input-btc").val(amoutbtc);
            $("#input-brl").val(totalreal);
            $("#input-amount").val(parseFloat(qtd));             
        },

        VisualizarEdicao: function (NewGain, NewLoss) {
            $(".gain,.loss").css("text-decoration", "line-through");
            $(".newgain").text(NewGain);
            $(".newloss").text(NewLoss);
            $(".desc-newgain").removeClass("d-none");
            $(".desc-newloss").removeClass("d-none");
        },

        HideEdicao: function () {
            $(".gain,.loss").css("text-decoration", "none");
            $(".newgain").text("");
            $(".newloss").text("");
            $(".desc-newgain").addClass("d-none");
            $(".desc-newloss").addClass("d-none");
        }
    }
})()