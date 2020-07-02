import { PercentualGain, PercentualLoss } from './../../ComunFunctions.js'
import moment from 'moment'
import { WebppAppRedirect } from '../../ComunFunctions.js'

export var Ordems = (function () {
    var posFilter = 1;
    var browserLocale = navigator.language || navigator.userLanguage;
    return {
        AddOrdem: function (ordem) {
            if (!this.PossuiAbertas()) {
                this.Clear();
            }

            if (posFilter == 1) {
                var tbody = $(".table-list-posicionadas tbody");
                var tr = "<tr class='OrdensAtivas' data-tipoOrdem='" + ordem.tipoOrdem + "' data-status='" + ordem.status_id + "' data-symbol='" + ordem.symbol + "' data-entrada='" + parseFloat(ordem.precoEntrada).toFixed(8) + "' data-rangeentrada='" + parseFloat(ordem.rangeEntrada).toFixed(8) + "' data-gain='" + parseFloat(ordem.precoGain).toFixed(8) + "' data-loss='" + parseFloat(ordem.precoLoss).toFixed(8) + "' data-ordemId='" + ordem.id + "' data-qtd='" + ordem.quantidade + "' data-chamadaId='" + ordem.chamada_Id + "' data-observacao='" + ordem.observacao + "' data-horario='" + ordem.dataCadastro + "'>" +
                    "<td>" + ordem.chamada_Id + "</td>" +
                    "<td>" + moment(ordem.dataCadastro).locale(browserLocale).format("L LTS") + "</td>" +
                    "<td>" + ordem.symbol + "</td>" +
                    "<td>" + parseFloat(ordem.precoEntrada).toFixed(8) + "</td>" +
                    "<td>" + ordem.descricao + "</td>" +
                    "<td>" + PercentualGain(ordem.precoEntrada, ordem.precoGain) + "% <i class='fa fa-caret-up icon-arrowup'></i></td>" +
                    "<td>" + PercentualLoss(ordem.precoEntrada, ordem.precoLoss) + "% <i class='fa fa-caret-down icon-arrowdown'></i></td>" +
                    "<td></td>"
                "</tr>";
                tbody.prepend(tr);
            }
        },

        AddOrdemExecutada: function (ordem) {
            if (!this.PossuiAbertas()) {
                this.Clear();
            }

            var tbody = $(".table-list-posicionadas tbody");
            var tr = "<tr class='OrdensFinalizadas' data-status='" + ordem.ordemStatus_Id + "' data-symbol='" + ordem.chamada.symbol.symbol + "' data-entrada='" + parseFloat(ordem.chamada.precoEntrada).toFixed(8) + "' data-rangeentrada='" + parseFloat(ordem.chamada.rangeEntrada).toFixed(8) + "' data-gain='" + parseFloat(ordem.chamada.precoGain).toFixed(8) + "' data-loss='" + parseFloat(ordem.chamada.precoLoss).toFixed(8) + "' data-ordemId='" + ordem.id + "' data-qtd='" + ordem.quantidade + "' + data-chamadaId='" + ordem.chamada_Id + "' data-observacao='" + ordem.chamada.observacao + "' data-horario='" + ordem.dataExecucao + "'>" +
                "<td>" + ordem.chamada_Id + "</td>" +
                "<td>" + moment(ordem.dataCadastro).locale(browserLocale).format("L LTS") + "</td>" +
                "<td>" + ordem.chamada.symbol.symbol + "</td>" +
                "<td>" + parseFloat(ordem.chamada.precoEntrada).toFixed(8) + "</td>" +
                "<td>" + ordem.ordemStatus.descricao + " " + moment(ordem.dataExecucao).locale(browserLocale).format("L LTS") + "</td>" +
                "<td>" + PercentualGain(ordem.chamada.precoEntrada, ordem.chamada.precoGain) + "% <i class='fa fa-caret-up icon-arrowup'></i></td>" +
                "<td>" + PercentualLoss(ordem.chamada.precoEntrada, ordem.chamada.precoLoss) + "% <i class='fa fa-caret-down icon-arrowdown'></i></td>" +         
                "<td></td>"
            "</tr>";
            tbody.prepend(tr);
        },

        LoadOrdensAtivas: function (ordem) {
            var tbody = $(".table-list-posicionadas tbody");
            var tr = "<tr class='OrdensAtivas' data-status='" + ordem.ordemStatus_Id + "' data-symbol='" + ordem.symbol + "' data-entrada='" + ordem.precoEntrada.toFixed(8) + "' data-rangeentrada='" + ordem.rangeEntrada.toFixed(8) + "' data-gain='" + (ordem.newGain != null ? ordem.newGain.toFixed(8) : ordem.precoGain.toFixed(8)) + "' data-loss='" + (ordem.newLoss != null ? ordem.newLoss.toFixed(8) : ordem.precoLoss.toFixed(8)) + "' data-ordemId='" + ordem.id + "' data-qtd='" + ordem.quantidade + "' + data-chamadaId='" + ordem.chamada_Id + "' data-observacao='" + ordem.observacao + "' data-horario='" + (ordem.ordemStatus_Id == 1 ? ordem.dataCadastro : ordem.dataEntrada) + "'>" +
                "<td>" + ordem.chamada_Id + "</td>" +
                "<td>" + moment(ordem.dataCadastro).locale(browserLocale).format("L LTS") + "</td>" +
                "<td>" + ordem.symbol + "</td>" +
                "<td>" + ordem.precoEntrada.toFixed(8) + "</td>" +
                (ordem.dataEntrada != null ? "<td>" + ordem.descricao + " " + moment(ordem.dataEntrada).locale(browserLocale).format("L LTS") + "</td>" : "<td>" + ordem.descricao + "</td>") +
                "<td>" + PercentualGain(ordem.precoEntrada, (ordem.newGain != null ? ordem.newGain : ordem.precoGain)) + "% <i class='fa fa-caret-up icon-arrowup'></i></td>" +
                "<td>" + PercentualLoss(ordem.precoEntrada, (ordem.newLoss != null ? ordem.newLoss : ordem.precoLoss)) + "% <i class='fa fa-caret-down icon-arrowdown'></i></td>" +
                "<td></td>" +
            "</tr>";
            tbody.prepend(tr);
        },

        LoadOrdensFinalizadas: function (ordem) {
            var tbody = $(".table-list-posicionadas tbody");
            var tr = "<tr class='OrdensFinalizadas' data-status='" + ordem.ordemStatus_Id + "' data-symbol='" + ordem.symbol + "' data-entrada='" + ordem.precoEntrada.toFixed(8) + "' data-rangeentrada='" + ordem.rangeEntrada.toFixed(8) + "' data-gain='" + (ordem.newGain != null ? ordem.newGain.toFixed(8) : ordem.precoGain.toFixed(8)) + "' data-loss='" + (ordem.newLoss != null ? ordem.newLoss.toFixed(8) : ordem.precoLoss.toFixed(8)) + "' data-ordemId='" + ordem.id + "' data-qtd='" + ordem.quantidade + "' + data-chamadaId='" + ordem.chamada_Id + "' data-observacao='" + ordem.observacao + "' data-horario='" + (ordem.dataExecucao != null ? ordem.dataExecucao : ordem.dataEntrada) + "' data-vendamercado='" + ordem.precoVendaMercado + "'>" +
                "<td>" + ordem.chamada_Id + "</td>" +
                "<td>" + moment(ordem.dataCadastro).locale(browserLocale).format("L LTS") + "</td>" +
                "<td>" + ordem.symbol + "</td>" +
                "<td>" + ordem.precoEntrada.toFixed(8) + "</td>" +
                (ordem.dataExecucao != null ? "<td>" + ordem.descricao + " " + moment(ordem.dataExecucao).locale(browserLocale).format("L LTS") + "</td>" : "<td>" + ordem.descricao + "</td>") +
                "<td>" + PercentualGain(ordem.precoEntrada, (ordem.newGain != null ? ordem.newGain : ordem.precoGain)) + "% <i class='fa fa-caret-up icon-arrowup'></i></td>" +
                "<td>" + PercentualLoss(ordem.precoEntrada, (ordem.newLoss != null ? ordem.newLoss : ordem.precoLoss)) + "% <i class='fa fa-caret-down icon-arrowdown'></i></td>" +
                "<td></td>" +
                "</tr>";
            tbody.prepend(tr);
        },

        LoadCanceladas: function (ordem) {
            var tbody = $(".table-list-posicionadas tbody");
            var tr = "<tr class='OrdensCanceladas' data-status='" + ordem.ordemStatus_Id + "' data-symbol='" + ordem.symbol + "' data-entrada='" + ordem.precoEntrada.toFixed(8) + "' data-rangeentrada='" + ordem.rangeEntrada.toFixed(8) + "' data-gain='" + (ordem.newGain != null ? ordem.newGain.toFixed(8) : ordem.precoGain.toFixed(8)) + "' data-loss='" + (ordem.newLoss != null ? ordem.newLoss.toFixed(8) : ordem.precoLoss.toFixed(8)) + "' data-ordemId='" + ordem.id + "' data-qtd='" + ordem.quantidade + "' + data-chamadaId='" + ordem.chamada_Id + "'data-horario='" + ordem.dataCancelamento + "' data-observacao='" + ordem.observacao + "'>" +
                "<td>" + ordem.chamada_Id + "</td>" +
                "<td>" + moment(ordem.dataCadastro).locale(browserLocale).format("L LTS") + "</td>" +
                "<td>" + ordem.symbol + "</td>" +
                "<td>" + ordem.precoEntrada.toFixed(8) + "</td>" +
                "<td>" + ordem.descricao + " " + moment(ordem.dataCancelamento).locale(browserLocale).format("L LTS") + "</td>" +
                "<td>" + PercentualGain(ordem.precoEntrada, (ordem.newGain != null ? ordem.newGain : ordem.precoGain)) + "% <i class='fa fa-caret-up icon-arrowup'></i></td>" +
                "<td>" + PercentualLoss(ordem.precoEntrada, (ordem.newLoss != null ? ordem.newLoss : ordem.precoLoss)) + "% <i class='fa fa-caret-down icon-arrowdown'></i></td>" +
                "<td></td>" +
                "</tr>";
            tbody.prepend(tr);
        },

        ClearTable: function () {
            var table_body = document.querySelector(".table-list-posicionadas tbody");
            table_body.innerHTML = "";
        },

        AtualizarStatus: function (ordemId, Status_Id, Msg) {
            var tr = document.querySelector(".table-list-posicionadas tr[data-ordemId='" + ordemId + "']");
            if (tr) {
                tr.setAttribute("data-status", Status_Id);
                tr.cells[4].innerText = Msg;
                return tr;
            }
        },

        AtualizarFiltro: function (posicao, element) {
            posFilter = posicao;
            $("#filter-posicionadas").removeClass("ultimofiltro");
            $("#filter-finalizadas").removeClass("ultimofiltro");
            $("#filter-canceladas").removeClass("ultimofiltro");
            element.classList.add("ultimofiltro");
        },

        AtualizarEdicao: function (ChamadaEditada) {

            var tr = $(".table-list-posicionadas > tbody > tr[data-chamadaId='" + ChamadaEditada.chamadaId + "']")[0];
            tr.setAttribute("data-gain", ChamadaEditada.newGain);
            tr.setAttribute("data-loss", ChamadaEditada.newLoss);

            tr.cells[4].innerHTML = PercentualGain(ChamadaEditada.precoEntrada, ChamadaEditada.newGain) + "% <i class='fa fa-caret-up icon-arrowup'></i>";
            tr.cells[5].innerHTML = PercentualLoss(ChamadaEditada.precoEntrada, ChamadaEditada.newLoss) + "% <i class='fa fa-caret-down icon-arrowdown'></i>";
        },

        RemoveOrdem: function (ordemId) {
            var tr = document.querySelector(".table-list-posicionadas tr[data-ordemId='" + ordemId + "']");
            if (tr && posFilter == 1) {
                tr.remove();
            }
        },

        GetOrderQuantity: function (chamadaId) {
            var currentOrder;
            $.ajax({
                url: "/usuario/ordems/GetOrdembyChamadaID",
                data: { "chamadaId": chamadaId },
                method: "GET",
                async: false,
                success: function (data) {
                    currentOrder = data;
                },
                error: function (data) {
                    WebppAppRedirect(data);
                }
            });
            return currentOrder;
        },

        PossuiAbertas: function () {
            var ordens = document.querySelectorAll(".table-list-posicionadas > tbody > tr[data-status]");
            return ordens.length > 0;
        },

        Clear: function () {
            var menu = document.querySelector(".table-list-posicionadas > tbody");
            menu.innerHTML = "";
        },

        getFilterPos: function () {
            return posFilter;
        }
    }
})()