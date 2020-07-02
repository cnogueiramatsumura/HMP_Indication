import popover from 'bootstrap/js/src/popover'
import { MargenPercentual } from './MargenPercentual.js'
import { Chamadas } from './Chamadas.js'
import { Filters } from './Filters.js'
import { Ordems } from './Ordems.js'
import { Notificacoes } from './Notificacoes.js'
import { Menu } from './Menu.js'
import { BinanceTrade } from './BinanceTrade.js'
import { counttime } from '../../ComunFunctions.js'
import moment from 'moment'
import * as signalR from 'signalr'
import { SignalRLogs } from './SignalRLogs'
import { MarketValue } from './MarketValue'

$(function () {
    var cardNotifi;
    var handlertimeoutpopover;
    var browserLocale = navigator.language || navigator.userLanguage;

    MarketValue.init();
    MargenPercentual.init(listsymbol);


    $("#recolher-menu").on("click", function () {
        if ($(this).text() == ">") {
            $(".menu-chamadas").hide();
            $(this).text("<").css("right", 0);
            $(".posicionadas-content").removeClass("col-9")
            $(".posicionadas-content").addClass("col-12")
        }
        else {
            $(".menu-chamadas").show();
            $(this).text(">").css("right", "25%");
            $(".posicionadas-content").removeClass("col-12")
            $(".posicionadas-content").addClass("col-9")
        }
    })

    $("#filter-chamada-abertas").on("click", function () {
        Chamadas.AtualizarFiltro(1, this);
        Chamadas.Clear();
        $(this).attr("disabled", true);
        try {
            $.ajax({
                url: "/usuario/chamada/Ativas",
                method: "GET",
                success: function (obj) {
                    try {
                        obj.cancelamentoChamadas.forEach((item) => {
                            Chamadas.LoadCanceladas(item);
                        })
                        obj.chamadaEditadas.forEach((item) => {
                            Chamadas.LoadEditadas(item);
                        })
                        obj.ativas.forEach((item) => {
                            Chamadas.LoadAtivas(item);
                        })

                        if (obj.ativas.length == 0 && obj.chamadaEditadas.length == 0 && obj.cancelamentoChamadas.length == 0) {
                            $("#painel-chamadas").html("Nenhuma Chamada Ativa");
                        }
                    }
                    catch{
                        $("#filter-chamada-abertas").attr("disabled", false);
                    }
                },
                error: function (data) {
                    redirectAuthorize(data);
                },
                complete: function () {
                    $("#filter-chamada-abertas").attr("disabled", false);
                }
            })
        }
        catch (ex) {
            $("#filter-chamada-abertas").attr("disabled", false);
        }
    })

    $("#filter-chamada-encerradas").on("click", function () {
        Chamadas.Clear();
        Chamadas.AtualizarFiltro(2, this);
        $(this).attr("disabled", true);
        try {
            $.ajax({
                url: "/usuario/chamada/Encerradas",
                method: "GET",
                success: function (obj) {
                    try {
                        obj.forEach((item) => {
                            Chamadas.LoadEncerradas_e_Recusadas(item, 'card-chamadas-encerradas');
                        })
                        if (obj.length == 0) {
                            $("#painel-chamadas").html("Nenhuma Chamada Encerrada");
                        }
                    }
                    catch{
                        $("#filter-chamada-encerradas").attr("disabled", false);
                    }
                },
                error: function (data) {
                    redirectAuthorize(data);
                },
                complete: function () {
                    $("#filter-chamada-encerradas").attr("disabled", false);
                }
            })
        }
        catch (ex) {
            $("#filter-chamada-encerradas").attr("disabled", false);
        }
    })

    $("#filter-chamada-recusadas").on("click", function () {
        Chamadas.Clear();
        Chamadas.AtualizarFiltro(3, this);
        $(this).attr("disabled", true);
        try {
            $.ajax({
                url: "/usuario/chamada/Recusadas",
                method: "GET",
                success: function (obj) {
                    try {
                        obj.forEach((item) => {
                            Chamadas.LoadEncerradas_e_Recusadas(item, 'card-chamadas-recusadas');
                        })
                        if (obj.length == 0) {
                            $("#painel-chamadas").html("Nenhuma Chamada Recusada");
                        }
                    }
                    catch{
                        $("#filter-chamada-recusadas").attr("disabled", false);
                    }
                },
                error: function (data) {
                    redirectAuthorize(data);
                },
                complete: function () {
                    $("#filter-chamada-recusadas").attr("disabled", false);
                }
            })
        }
        catch (ex) {
            $("#filter-chamada-recusadas").attr("disabled", false);
        }
    })



    //edicao
    $("#painel-editadas").on("click", ".btn-visualizar-edicao", function () {
        var cardedicao = $(this).parent().parent();
        var symbol = cardedicao.data("symbol");
        var entrada = cardedicao.data("entrada");
        var gain = cardedicao.data("gain");
        var loss = cardedicao.data("loss");
        var rangeEntrada = cardedicao.data("rangeentrada");
        var NewGain = parseFloat(cardedicao.data("newgain")).toFixed(8);
        var NewLoss = parseFloat(cardedicao.data("newloss")).toFixed(8);

        var ordem = Ordems.GetOrderQuantity(cardedicao.data("chamadaid"));
        $("#link-tradeview").attr("href", "https://www.tradingview.com/chart?symbol=binance" + encodeURIComponent(":" + symbol));
        var qtd = ordem.quantidade;

        var amoutbtc = parseFloat(qtd * entrada).toFixed(8);
        var bitcoinvalue = MarketValue.getBTCtoBRLValue();
        var totalreal = (amoutbtc * bitcoinvalue).toFixed(2);

        Menu.ClearIcon();
        Menu.AtualizarHeader("", symbol, entrada, rangeEntrada, gain, loss);
        Menu.AtualizarPercentuais(entrada, gain, loss);
        Menu.AtualizarCardValores(qtd, amoutbtc, totalreal);
        Menu.VisualizarEdicao(NewGain, NewLoss);

        Menu.HideOrdemPercentualButtons();
        Menu.IconsEntradaRealizada();
        Menu.HideAllActionButtons();
        Menu.ShowChamadasContent();

        BinanceTrade.updaChangeColor(true);
        BinanceTrade.startMonitor(symbol, entrada, gain, loss, true);
    })

    $("#painel-editadas").on("click", ".btn-aceitar-edicao", function () {
        var cardEdicao = $(this).parent().parent();
        var edicaoId = cardEdicao.data("edicaoid");
        var chamadaId = cardEdicao.data("chamadaid");
        $(this).attr("disabled", true);
        try {
            $.ajax({
                url: "/ChamadasEditadas/aceitar",
                data: { EdicaoId: edicaoId, ChamadaId: chamadaId },
                method: "POST",
                success: function (data) {
                    cardEdicao.remove();
                    Ordems.AtualizarEdicao(data);
                    BinanceTrade.disconnect();
                    Menu.ShowPosicionadasContent();
                    Notificacoes.ShowNotificacao("notif-addChamada", "Sua Ordem foi Atualizada.");
                },
                error: function (data) {
                    redirectAuthorize(data);
                },
                complete: function () {
                    $(".btn-aceitar-edicao").attr("disabled", false);
                }
            })
        }
        catch (ex) {
            $(".btn-aceitar-edicao").attr("disabled", false);
        }
    })

    $("#painel-editadas").on("click", ".btn-recusar-edicao", function () {
        var cardEdicao = $(this).parent().parent();
        var edicaoId = cardEdicao.data("edicaoid");
        var chamadaId = cardEdicao.data("chamadaid");
        $(this).attr("disabled", true);
        try {
            $.ajax({
                url: "/ChamadasEditadas/recusar",
                data: { EdicaoId: edicaoId, ChamadaId: chamadaId },
                method: "POST",
                success: function () {
                    cardEdicao.remove();
                },
                error: function (data) {
                    redirectAuthorize(data);
                },
                complete: function () {
                    $(".btn-recusar-edicao").attr("disabled", false);
                }
            })
        }
        catch (ex) {
            $(".btn-recusar-edicao").attr("disabled", false);
        }
    })
    //cancelamento
    $("#painel-canceladas").on("click", ".btn-visualizar-cancelamento", function () {
        var cardCancelamento = $(this).parent().parent();
        var symbol = cardCancelamento.data("symbol");
        var entrada = cardCancelamento.data("entrada");
        var gain = cardCancelamento.data("gain");
        var loss = cardCancelamento.data("loss");
        var OrderId = cardCancelamento.data("ordemid");
        var qtd = cardCancelamento.data("qtd");
        var rangeEntrada = cardCancelamento.data("rangeentrada");
        var observacao = cardCancelamento.data("observacao");

        var amoutbtc = parseFloat(qtd * rangeEntrada).toFixed(8);
        var bitcoinvalue = MarketValue.getBTCtoBRLValue();
        var totalreal = (amoutbtc * bitcoinvalue).toFixed(2);

        $("#ordemId").val(OrderId);
        $("#show-observacao").attr('data-content', observacao).data('bs.popover').setContent();
        $("#link-tradeview").attr("href", "https://www.tradingview.com/chart?symbol=binance" + encodeURIComponent(":" + symbol));

        Menu.ClearIcon();
        Menu.AtualizarHeader("", symbol, entrada, rangeEntrada, gain, loss);
        Menu.AtualizarPercentuais(entrada, gain, loss);
        Menu.AtualizarCardValores(qtd, amoutbtc, totalreal);
        Menu.HideEdicao();

        Menu.HideOrdemPercentualButtons();
        Menu.IconsAguardandoEntrada();
        Menu.ShowBtnCancelarEntrada();
        Menu.ShowChamadasContent();

        BinanceTrade.startMonitor(symbol, entrada, gain, loss, false);
    })

    $("#painel-canceladas").on("click", ".btn-aceitar-cancelamento", function () {
        var cardCancelamento = $(this).parent().parent();
        var OrderId = cardCancelamento.data("ordemid");
        $(this).attr("disabled", true);
        try {
            $.ajax({
                url: "/usuario/Ordems/CancelarEntrada/",
                method: "POST",
                data: { "OrderID": OrderId },
                success: function (data) {
                    BinanceTrade.disconnect();
                    Menu.ShowPosicionadasContent();
                    cardCancelamento.remove();
                    var trOrder = document.querySelector("tr[data-ordemid='" + data.Id + "']");
                    if (trOrder) {
                        trOrder.remove();
                    }
                },
                error: function (data) {
                    redirectAuthorize(data);
                },
                complete: function () {
                    $(".btn-aceitar-cancelamento").attr("disabled", false);
                }
            })
        }
        catch (ex) {
            $(".btn-aceitar-cancelamento").attr("disabled", false);
        }
    })

    $("#painel-canceladas").on("click", ".btn-recusar-cancelamento", function () {
        var cardEdicao = $(this).parent().parent();
        var cancelamentochamadaid = cardEdicao.data("cancelamentochamadaid");
        $(this).attr("disabled", true);
        $.ajax({
            url: "/CancelamentoChamada/recusar",
            data: { CancelamentoChamada_ID: cancelamentochamadaid },
            method: "POST",
            success: function () {
                cardEdicao.remove();
            },
            error: function (data) {
                redirectAuthorize(data);
            }
        })
    })

    $("#painel-chamadas").on("click", ".recusarchamada", function () {
        var chamadaid = $(this).data("id");
        this.remove();
        //$(this).css("pointer-events", "none");
        $.ajax({
            url: "/usuario/chamada/recusarchamada/",
            method: "POST",
            data: { id: chamadaid },
            success: function (ChamadaRecusada) {
                Chamadas.RecusarChamada(ChamadaRecusada.chamada_ID);
                BinanceTrade.disconnect();
            },
            error: function (data) {
                redirectAuthorize(data);
            }
        });
    })

    $("#painel-chamadas").on("click", ".card-chamadas", function () {
        if ($(event.target).hasClass("recusarchamada")) {
            event.stopPropagation();
            return;
        }
        MarketValue.UpdateMarketValues();

        var symbol = $(this).data("symbol");
        var entrada = $(this).data("entrada");
        var gain = $(this).data("gain");
        var loss = $(this).data("loss");
        var chamadaid = $(this).data("chamadaid");
        var indicado = $(this).data("indicado");
        var rangeEntrada = $(this).data("rangeentrada");
        var horario = $(this).data("horario");
        var observacao = $(this).data("observacao");

        $("#chamadaId").val(chamadaid);
        $("#show-observacao").attr('data-content', observacao).data('bs.popover').setContent();
        $("#ordemId").val(" ");
        $("#link-tradeview").attr("href", "https://www.tradingview.com/chart?symbol=binance" + encodeURIComponent(":" + symbol));

        Menu.AtualizarHeader(horario, symbol, entrada, rangeEntrada, gain, loss);
        Menu.AtualizarPercentuais(entrada, gain, loss);
        Menu.ClearIcon();
        Menu.ClearInputs();
        Menu.ShowOrdemPercentualButtons();
        Menu.ShowBtnIndicacao();
        Menu.HideEdicao();
        Menu.ShowChamadasContent();
        BinanceTrade.updaChangeColor(false);

        if (indicado) {
            $("#perc-indicado").removeClass("d-none");
            $("#perc-indicado").text(indicado + "%")
        }
        else {
            $("#perc-indicado").addClass("d-none");
        }

        $('#show-observacao').popover('hide');
        $("#show-observacao").trigger("click");
        clearTimeout(handlertimeoutpopover);
        handlertimeoutpopover = setTimeout(function () {
            $('#show-observacao').popover('hide');
        }, 5000)
        Filters.GetFilterValues(symbol);
        BinanceTrade.startMonitor(symbol, entrada, gain, loss, false);
    })



    $(".posicionadas-notificacao").on("click", ".fechar-notificacao", function () {
        $(this).css("pointer-events", "none");
        var divnotificacao = this.parentNode.parentNode
        Notificacoes.RemoverNotificacao(divnotificacao);
    })

    $("#filter-posicionadas").click(function () {
        Ordems.AtualizarFiltro(1, this);
        Ordems.ClearTable();
        $(this).attr("disabled", true);
        try {
            $.ajax({
                url: "/usuario/ordems/Ativas",
                method: "GET",
                success: function (obj) {
                    try {
                        obj.forEach((item) => {
                            Ordems.LoadOrdensAtivas(item);
                        })
                        if (obj.length == 0) {
                            var tbody = $(".table-list-posicionadas tbody");
                            $(tbody).html("<tr><td  class='no-cliclable' colspan='8'> Não possui Ordens Posicionadas </td></tr>");
                        }
                        MargenPercentual.updaterows();
                    }
                    catch{
                        $("#filter-posicionadas").attr("disabled", false);
                    }
                },
                error: function (data) {
                    redirectAuthorize(data);
                },
                complete: function () {
                    $("#filter-posicionadas").attr("disabled", false);
                }
            })
        }
        catch (ex) {
            $("#filter-posicionadas").attr("disabled", false);
        }
    })

    $("#filter-finalizadas").click(function () {
        Ordems.AtualizarFiltro(2, this);
        Ordems.ClearTable();
        $(this).attr("disabled", true);
        try {
            $.ajax({
                url: "/usuario/ordems/Finalizadas",
                method: "GET",
                success: function (obj) {
                    try {
                        obj.forEach((item) => {
                            Ordems.LoadOrdensFinalizadas(item);
                        })
                        if (obj.length == 0) {
                            var tbody = $(".table-list-posicionadas tbody");
                            $(tbody).html("<tr><td  class='no-cliclable' colspan='8'> Não possui Ordens Finalizadas </td></tr>");
                        }
                    }
                    catch{
                        $("#filter-finalizadas").attr("disabled", false);
                    }
                },
                error: function (data) {
                    redirectAuthorize(data);
                },
                complete: function () {
                    $("#filter-finalizadas").attr("disabled", false);
                }
            })
        }
        catch (ex) {
            $("#filter-finalizadas").attr("disabled", false);
        }
    })

    $("#filter-canceladas").click(function () {
        Ordems.AtualizarFiltro(3, this);
        Ordems.ClearTable();
        $(this).attr("disabled", true);
        try {
            $.ajax({
                url: "/usuario/ordems/Canceladas",
                method: "GET",
                success: function (obj) {
                    try {
                        obj.forEach((item) => {
                            Ordems.LoadCanceladas(item);
                        })
                        if (obj.length == 0) {
                            var tbody = $(".table-list-posicionadas tbody");
                            $(tbody).html("<tr><td  class='no-cliclable' colspan='8'> Não possui Ordens Canceladas </td></tr>");
                        }
                    }
                    catch{
                        $("#filter-canceladas").attr("disabled", false);
                    }
                },
                error: function (data) {
                    redirectAuthorize(data);
                },
                complete: function () {
                    $("#filter-canceladas").attr("disabled", false);
                }
            })
        }
        catch (ex) {
            $("#filter-canceladas").attr("disabled", false);
        }
    })

    $(".wrapper-table").on("click", ".OrdensAtivas", function () {
        var ordem = {};
        getOrdemValues(ordem, this);

        var amoutbtc = parseFloat(ordem.qtd * ordem.rangeEntrada).toFixed(8);
        var bitcoinvalue = MarketValue.getBTCtoBRLValue();
        var totalreal = (amoutbtc * bitcoinvalue).toFixed(2);

        $("#ordemId").val(ordem.OrderId);
        $("#show-observacao").attr('data-content', ordem.observacao).data('bs.popover').setContent();
        $("#link-tradeview").attr("href", "https://www.tradingview.com/chart?symbol=binance" + encodeURIComponent(":" + ordem.symbol));
        Menu.ClearIcon();
        Menu.HideOrdemPercentualButtons();
        Menu.AtualizarHeader(ordem.horario, ordem.symbol, ordem.entrada, ordem.rangeEntrada, ordem.gain, ordem.loss);
        Menu.AtualizarPercentuais(ordem.entrada, ordem.gain, ordem.loss);
        Menu.AtualizarCardValores(ordem.qtd, amoutbtc, totalreal);
        Menu.HideEdicao();
        if (ordem.status == 1) {
            Menu.IconsAguardandoEntrada();
            Menu.ShowBtnCancelarEntrada();
            BinanceTrade.updaChangeColor(false);
            BinanceTrade.startMonitor(ordem.symbol, ordem.entrada, ordem.gain, ordem.loss, false);

        }
        else if (ordem.status == 3) {
            Menu.IconsEntradaRealizada();
            Menu.ShowBtnSairMercado();
            BinanceTrade.updaChangeColor(true);
            BinanceTrade.startMonitor(ordem.symbol, ordem.entrada, ordem.gain, ordem.loss, true);
        }
        Menu.ShowChamadasContent();
    })

    $(".wrapper-table").on("click", ".OrdensFinalizadas", function () {
        var ordem = {};
        getOrdemValues(ordem, this);

        var amoutbtc = parseFloat(ordem.qtd * ordem.rangeEntrada).toFixed(8);
        var bitcoinvalue = MarketValue.getBTCtoBRLValue();
        var totalreal = (amoutbtc * bitcoinvalue).toFixed(2);
        $("#ordemId").val(ordem.OrderId);
        $("#show-observacao").attr('data-content', ordem.observacao).data('bs.popover').setContent();
        $("#link-tradeview").attr("href", "https://www.tradingview.com/chart?symbol=binance" + encodeURIComponent(":" + ordem.symbol));

        Menu.ClearIcon();
        Menu.HideAllActionButtons();
        Menu.AtualizarHeader(ordem.horario, ordem.symbol, ordem.entrada, ordem.rangeEntrada, ordem.gain, ordem.loss);
        Menu.AtualizarPercentuais(ordem.entrada, ordem.gain, ordem.loss);
        Menu.AtualizarCardValores(ordem.qtd, amoutbtc, totalreal);
        Menu.HideOrdemPercentualButtons();
        Menu.HideEdicao();
        Menu.ShowChamadasContent();

        if (ordem.status == 2) {
            var vendarcado = $(this).data("vendamercado");
            BinanceTrade.clearProgressBar();
            BinanceTrade.showVendaMercado(vendarcado, ordem.entrada, ordem.gain, ordem.loss);
        }
        else if (ordem.status == 5) {
            Menu.IconsGainRealizado();
            BinanceTrade.GainRealizado();
        }
        else if (ordem.status == 6) {
            Menu.IconLossRealizado();
            BinanceTrade.LossRealizado();
        }
    })

    $(".wrapper-table").on("click", ".OrdensCanceladas", function () {
        var ordem = {};
        getOrdemValues(ordem, this);

        var amoutbtc = parseFloat(ordem.qtd * ordem.rangeEntrada).toFixed(8);
        var bitcoinvalue = MarketValue.getBTCtoBRLValue();
        var totalreal = (amoutbtc * bitcoinvalue).toFixed(2);
        $("#show-observacao").attr('data-content', ordem.observacao).data('bs.popover').setContent();
        $("#link-tradeview").attr("href", "https://www.tradingview.com/chart?symbol=binance" + encodeURIComponent(":" + ordem.symbol));

        Menu.ClearIcon();
        Menu.HideAllActionButtons();
        Menu.AtualizarHeader(ordem.horario, ordem.symbol, ordem.entrada, ordem.rangeEntrada, ordem.gain, ordem.loss);
        Menu.AtualizarPercentuais(ordem.entrada, ordem.gain, ordem.loss);
        Menu.AtualizarCardValores(ordem.qtd, amoutbtc, totalreal);
        Menu.HideEdicao();
        Menu.HideOrdemPercentualButtons();
        Menu.ShowChamadasContent();
        BinanceTrade.clearProgressBar();
        BinanceTrade.updaChangeColor(false);
    })



    $("#fecharChamadas").on("click", function () {
        BinanceTrade.disconnect();
        Menu.ShowPosicionadasContent();
    })

    $("#input-btc").on("keyup", function () {
        var amoutbtc = this.value;
        var precoEntrada = $("#rangeentrada").text();
        var qtd = Filters.setAmoutField(amoutbtc / precoEntrada);
        $("#input-amount").val(qtd);

        var bitcoinvalue = MarketValue.getBTCtoBRLValue();
        var totalreal = (amoutbtc * bitcoinvalue).toFixed(2);
        $("#input-brl").val(totalreal);

        var totaldisponivel = parseFloat(document.getElementById("display-btc-value").innerText.replace(",", "."));
        var valorpercentual = (amoutbtc / totaldisponivel * 100);
        $("#range-input").val(parseInt(valorpercentual));
    });

    $("#input-btc").on("change", function () {
        var btcValue = this.value;
        Filters.ValidateBTCField(btcValue);

        var precoEntrada = $("#rangeentrada").text();
        var qtd = Filters.setAmoutField(btcValue / precoEntrada);
        var chamadaid = $("#chamadaId").val();

        Filters.ValidateAmountField(chamadaid, qtd);
        Filters.ValidateLimitDisponivel();
        Filters.isValid();
    });

    $("#input-amount").on("keyup", function () {
        var amoutcripto = this.value;
        var precoEntrada = $("#rangeentrada").text();
        var btcValue = (amoutcripto * precoEntrada).toFixed(8);
        $("#input-btc").val(btcValue);

        var bitcoinvalue = MarketValue.getBTCtoBRLValue();
        var totalreal = (btcValue * bitcoinvalue).toFixed(2);
        $("#input-brl").val(totalreal);

        var totaldisponivel = parseFloat(document.getElementById("display-btc-value").innerText.replace(",", "."));
        var valorpercentual = (btcValue / totaldisponivel * 100);
        $("#range-input").val(parseInt(valorpercentual));
    })

    $("#input-amount").on("change", function () {
        var qtd = this.value;
        var chamadaid = $("#chamadaId").val();
        Filters.ValidateAmountField(chamadaid, qtd);

        var amoutcripto = this.value;
        var precoEntrada = $("#rangeentrada").text();
        var btcValue = (amoutcripto * precoEntrada).toFixed(8);
        Filters.ValidateBTCField(btcValue);
        Filters.ValidateLimitDisponivel();
        Filters.isValid();
    })

    $("#input-brl").on("keyup", function () {
        var value = this.value;
        var bitcoinvalue = MarketValue.getBTCtoBRLValue();
        var totalbtc = (value / bitcoinvalue).toFixed(8);
        var precoEntrada = $("#rangeentrada").text();
        var qtd = Filters.setAmoutField(totalbtc / precoEntrada);

        $("#input-amount").val(Filters.setAmoutField(qtd));
        $("#input-btc").val(totalbtc.trimEnd('0'));

        var totaldisponivel = parseFloat(document.getElementById("display-btc-value").innerText.replace(",", "."));
        var valorpercentual = (totalbtc / totaldisponivel * 100);
        $("#range-input").val(parseInt(valorpercentual));
    })

    $("#input-brl").on("change", function () {
        var value = this.value;
        var bitcoinvalue = MarketValue.getBTCtoBRLValue();
        var totalbtc = (value / bitcoinvalue).toFixed(8);
        Filters.ValidateBTCField(totalbtc);
        var precoEntrada = $("#rangeentrada").text();
        var qtd = Filters.setAmoutField(totalbtc / precoEntrada);
        var chamadaid = $("#chamadaId").val();
        Filters.ValidateAmountField(chamadaid, qtd);
        Filters.ValidateLimitDisponivel();
        Filters.isValid();
    })

    $("#range-input").on("change", function () {
        var value = this.value;
        var chamadaid = $("#chamadaId").val();
        var totaldisponivel = parseFloat(document.getElementById("display-btc-value").innerText.replace(",", "."));

        var precoEntrada = parseFloat($("#rangeentrada").text().replace(",", "."));
        var qtd_disponivel = Filters.setAmoutField(totaldisponivel / precoEntrada);
        var qtddaporcentagem = Filters.setAmoutField((qtd_disponivel / 100) * value);

        var bitcoinvalue = MarketValue.getBTCtoBRLValue();
        var totalbtc = (qtddaporcentagem * precoEntrada);
        var totalbrl = (totalbtc * bitcoinvalue);

        $("#input-amount").val(qtddaporcentagem);
        $("#input-btc").val(totalbtc.toFixed(8));
        $("#input-brl").val(totalbrl.toFixed(2));

        Filters.ValidateAmountField(chamadaid, Filters.setAmoutField(qtddaporcentagem));
        Filters.ValidateLimitDisponivel();
        Filters.ValidateBTCField(totalbtc);
        Filters.isValid();
    })

    $(".botoespercentual button").on("click", function () {
        var totaldisponivel = parseFloat(document.getElementById("display-btc-value").innerText.replace(",", "."));
        var totalpercentual = parseInt(this.innerText.replace("%", ""));
        var precoEntrada = parseFloat($("#rangeentrada").text().replace(",", "."));
        var qtd_disponivel = Filters.setAmoutField(totaldisponivel / precoEntrada);
        var qtddaporcentagem = Filters.setAmoutField((qtd_disponivel / 100) * totalpercentual);

        var bitcoinvalue = MarketValue.getBTCtoBRLValue();
        var totalbtc = (qtddaporcentagem * precoEntrada);
        var totalbrl = (totalbtc * bitcoinvalue);

        $("#input-amount").val(qtddaporcentagem);
        $("#input-btc").val(totalbtc.toFixed(8));
        $("#input-brl").val(totalbrl.toFixed(2));
        $("#range-input").val(totalpercentual);
        var chamadaid = $("#chamadaId").val();

        Filters.ValidateAmountField(chamadaid, Filters.setAmoutField(qtddaporcentagem));
        Filters.ValidateLimitDisponivel();
        Filters.ValidateBTCField(totalbtc);
        Filters.isValid();
    });

    $("#btn-executar-indicacao").on("click", function () {
        var qtd = $("#input-amount").val();
        var chamadaid = $("#chamadaId").val();
        if (qtd && chamadaid && Filters.isValid()) {
            $(this).attr("disabled", true);
            $.ajax({
                url: "/usuario/chamada/aceitarchamada/",
                method: "POST",
                data: { id: chamadaid, qtd: qtd },
                success: function (result) {
                    BinanceTrade.disconnect();
                    Menu.ShowPosicionadasContent();
                    Ordems.AddOrdem(result);
                    Chamadas.DeletaChamada(result.chamadaId, false);
                    Notificacoes.ShowNotificacao("notif-addChamada", "Você aceitou uma indicação, posicionando sua ordem na exchange.");
                },
                error: function (data) {
                    redirectAuthorize(data);
                },
                complete: function () {
                    $("#btn-executar-indicacao").attr("disabled", false);
                }
            });
        }
        else if (!qtd) {
            $("#amountvalidademsg").text("Preencha a Quantidade");
        }
    })

    $("#btn-cancelar-entrada").on("click", function () {
        var OrderID = $("#ordemId").val();
        $(this).attr("disabled", true);
        $.ajax({
            url: "/usuario/Ordems/CancelarEntrada/",
            method: "POST",
            data: { "OrderID": OrderID },
            success: function (Ordem) {
                BinanceTrade.disconnect();
                Menu.ShowPosicionadasContent();
                var trOrder = document.querySelector("tr[data-ordemid='" + Ordem.id + "']");
                if (trOrder) {
                    trOrder.remove();
                }
                var cardCancelamento = document.querySelector(".card-cancelar[data-chamadaid='" + Ordem.chamada_Id + "']");
                if (cardCancelamento) {
                    cardCancelamento.remove();
                }
            },
            error: function (data) {
                redirectAuthorize(data);
            },
            complete: function () {
                $("#btn-cancelar-entrada").attr("disabled", false);
            }
        });
    });

    $("#btn-sair-mercado").on("click", function () {
        var OrderID = $("#ordemId").val();
        $(this).attr("disabled", true);
        $.ajax({
            url: "/usuario/Ordems/venderamercado",
            method: "POST",
            data: { "OrderID": OrderID },
            success: function (data) {
                BinanceTrade.disconnect();
                Menu.ShowPosicionadasContent();
                var trOrder = document.querySelector("tr[data-ordemid='" + data.id + "']");
                if (trOrder) {
                    trOrder.remove();
                }
                var hasAnotherOrder = MargenPercentual.hasOrder(data.chamada.symbol.symbol);
                if (!hasAnotherOrder) {
                    MargenPercentual.Remove(data.chamada.symbol.symbol);
                }
                Notificacoes.ShowNotificacao("notif-error", "Voce vendeu sua ordem a mercado.");
            },
            error: function (data) {
                redirectAuthorize(data);
            },
            complete: function () {
                $("#btn-sair-mercado").attr("disabled", false);
            }
        });
    });



    $('[data-toggle="popover"]').popover();
    var contador = setInterval(counttime, 60000);
    //var signalrclass = $.connection.signalChamadas;//Classe Conexao SignalR
    var connection = $.hubConnection(ApiDomainName, { useDefaultPath: true, logging: true });
    connection.qs = { "token": jwtToken };
    var signalrclass = connection.createHubProxy('signalChamadas');

    signalrclass.on("AdicionarChamada", function (objeto) {
        SignalRLogs.DisplayConsoleLogs("AdicionarChamada", objeto);
        Chamadas.AddChamada(objeto);
    });
    //sync com o app
    signalrclass.on("RemoverChamada", function (chamadaId) {
        SignalRLogs.DisplayConsoleLogs("RemoverChamada", chamadaId);
        var chamada = document.querySelector(".card-chamadas[data-chamadaid='" + chamadaId + "']");
        if ($("#chamadaId").val() == chamadaId) {
            BinanceTrade.disconnect();
            Menu.ShowPosicionadasContent();
        }
        chamada.remove();
    });

    signalrclass.on("EncerrarChamada", function (chamadaId) {
        SignalRLogs.DisplayConsoleLogs("EncerrarChamada", chamadaId);
        Chamadas.EncerrarChamada(chamadaId);
    });

    signalrclass.on("CancelarEntrada", function (symbol) {
        SignalRLogs.DisplayConsoleLogs("CancelarEntrada", symbol);
        Chamadas.CancelarEntrada(symbol);
    });

    signalrclass.on("EntradaRealizada", function (Ordem) {
        SignalRLogs.DisplayConsoleLogs("EntradaRealizada", Ordem);
        var cardCancelamento = document.querySelector(".card-cancelar[data-chamadaid='" + Ordem.chamada_Id + "']");
        if (cardCancelamento) {
            cardCancelamento.remove();
        }
        BinanceTrade.disconnect();
        Menu.ShowPosicionadasContent();
        Ordems.AtualizarStatus(Ordem.id, 3, Ordem.ordemStatus.descricao + " " + moment().locale(browserLocale).format("L LTS"));
        Notificacoes.ShowNotificacao("notif-entrada", "Entrada Realizada, Aguardando loss ou gain");

        var ws;
        var obj = { "symbol": Ordem.chamada.symbol.symbol, "ws": ws };
        MargenPercentual.Add(obj);
    });

    signalrclass.on("GainRealizado", function (Ordem) {
        SignalRLogs.DisplayConsoleLogs("GainRealizado", Ordem);
        var MenuOrderId = $("#ordemId").val();
        var filterPosition = Ordems.getFilterPos();
        if (MenuOrderId == Ordem.id) {
            Menu.HideAllActionButtons();
            BinanceTrade.disconnect();
            Menu.ClearIcon();
            Menu.IconsGainRealizado();
        }
        Notificacoes.ShowNotificacao("notif-entrada", "Gain Realizado");
        if (filterPosition == 1) {
            Ordems.RemoveOrdem(Ordem.id);
            MargenPercentual.Remove(Ordem.chamada.symbol.symbol);
        }
        else if (filterPosition == 2) {
            Ordems.AddOrdemExecutada(Ordem);
        }
    });

    signalrclass.on("LossRealizado", function (Ordem) {
        SignalRLogs.DisplayConsoleLogs("LossRealizado", Ordem);
        var MenuOrderId = $("#ordemId").val();
        var filterPosition = Ordems.getFilterPos();
        if (MenuOrderId == Ordem.id) {
            Menu.HideAllActionButtons();
            BinanceTrade.disconnect();
            Menu.ClearIcon();
            Menu.IconLossRealizado();
        }
        Notificacoes.ShowNotificacao("notif-error", "Loss Realizado");
        if (filterPosition == 1) {
            Ordems.RemoveOrdem(Ordem.id)
            MargenPercentual.Remove(Ordem.chamada.symbol.symbol);
        }
        else if (filterPosition == 2) {
            Ordems.AddOrdemExecutada(Ordem);
        }
    });

    signalrclass.on("AtualizarSaldo", function (saldo) {
        SignalRLogs.DisplayConsoleLogs("AtualizarSaldo", saldo);
        $("#display-btc-value").html("<i class='uil uil-bitcoin'></i> " + saldo)
        $("#display-brl-value").html("R$ " + MarketValue.getBRLAmount(saldo).toFixed(2))
    });

    signalrclass.on("OrdemCancelada", function (ordemId) {
        SignalRLogs.DisplayConsoleLogs("OrdemCancelada", ordemId);
        Notificacoes.ShowNotificacao("notif-error", "Sua Ordem foi Cancelada");
        Ordems.RemoveOrdem(ordemId);
        var MenuOrderId = $("#ordemId").val();
        if (MenuOrderId == ordemId) {
            BinanceTrade.disconnect();
            Menu.ShowPosicionadasContent();
        }
    });

    signalrclass.on("ChamadaEditada", function (chamadaEditada) {
        SignalRLogs.DisplayConsoleLogs("ChamadaEditada", chamadaEditada);
        Chamadas.AddEdicao(chamadaEditada);
    });

    signalrclass.on("RejeitadaMercadoemFalta", function (ordemId) {
        SignalRLogs.DisplayConsoleLogs("RejeitadaMercadoemFalta", ordemId);
        Ordems.RemoveOrdem(ordemId);
        Notificacoes.ShowNotificacao("notif-error", "Ordem Recusada, Mercado com quantidade Insuficiente para suplir sua ordem");
    });

    signalrclass.on("RemoverEdicao", function (chamadaId) {
        SignalRLogs.DisplayConsoleLogs("RemoverEdicao", chamadaId);
        var CardEdicao = document.querySelector(".card-Edicao[data-chamadaId='" + chamadaId + "']");
        if (CardEdicao) {
            CardEdicao.remove()
        };
    });

    signalrclass.on("test", function (obj) {
        SignalRLogs.DisplayConsoleLogs("test", obj);
    });
    //$.connection.hub.start();
    connection.start();


    var OneSignal = window.OneSignal || [];
    OneSignal.push(function () {
        OneSignal.init({
            appId: onesignalId,
            notifyButton: {
                enable: true,
            },
            allowLocalhostAsSecureOrigin: true
        });
        OneSignal.on('subscriptionChange', function (isSubscribed) {
            if (isSubscribed) {
                OneSignal.getUserId(function (userId) {
                    $.ajax({
                        type: "POST",
                        url: "/usuario/OneSignal/Subscrible",
                        data: { "onesignalid": userId },
                        success: function (data) {
                            console.log(data)
                        },
                        error: function (erro) {
                            console.log(erro)
                        }
                    });
                });
            }
            else {
                OneSignal.getUserId(function (userId) {
                    $.ajax({
                        type: "POST",
                        url: "/usuario/OneSignal/Unsubscrible",
                        data: { "onesignalid": userId },
                        success: function (data) {
                            console.log(data)
                        },
                        error: function (erro) {
                            console.log(erro)
                        }
                    });
                });
            }
        });

    });

})

function redirectAuthorize(data) {
    //binance api error
    if (data.status == 400) {
        var jsonresponse = JSON.parse(data.responseJSON);
        BinanceTrade.disconnect();
        Menu.ShowPosicionadasContent();
        Notificacoes.ShowNotificacao("notif-error", jsonresponse.motivo ? jsonresponse.motivo : jsonresponse.msg);
    }
    else if (data.status == 401) {
        window.location.href = "/usuario/login/index";
    }
    else if (data.status == 406) {
        BinanceTrade.disconnect();
        Menu.ShowPosicionadasContent();
        Notificacoes.ShowNotificacao("notif-error", data.responseJSON.replace(/"/g, ''));
    }
    else {
        console.log(data);
        Notificacoes.ShowNotificacao("notif-error", "Houve um erro entre em contato com a administração.");
    }
}

function getOrdemValues(obj, trordem) {
    obj.status = trordem.getAttribute("data-status"); //status da ordem
    obj.symbol = trordem.getAttribute("data-symbol");
    obj.entrada = trordem.getAttribute("data-entrada");
    obj.gain = trordem.getAttribute("data-gain");
    obj.loss = trordem.getAttribute("data-loss");
    obj.OrderId = trordem.getAttribute("data-ordemid");
    obj.qtd = trordem.getAttribute("data-qtd");
    obj.rangeEntrada = trordem.getAttribute("data-rangeentrada");
    obj.observacao = trordem.getAttribute("data-observacao");
    obj.horario = trordem.getAttribute("data-horario");
}