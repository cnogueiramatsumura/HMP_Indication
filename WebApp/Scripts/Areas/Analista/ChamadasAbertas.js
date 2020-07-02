import * as signalR from 'signalr'
import { SignalRLogs } from './../Usuario/DashBoard/SignalRLogs.js'


$(function ($) {
    $(".cancelar-chamada").on("click", function () {
        var row = $(this).parent().parent();
        var id = this.getAttribute("data-id");
        $(this).attr("disabled", true);
        $.ajax({
            url: "/Analista/Chamada/CancelarAbertas",
            method: "POST",
            data: { id: id },
            success: function () {
                row.remove();
            },
            error: function (data) {
                alert("Erro ao cancelar essa chamada");
            },
            complete: function () {
                $(".cancelar-chamada").attr("disabled", false);
            }
        })
    })

    var connection = $.hubConnection(ApiDomainName, { useDefaultPath: true, logging: true });
    connection.qs = { "role": "admin" };
    var signalrclass = connection.createHubProxy('signalChamadas');

    signalrclass.on("MudarParaEdicao", function (idChamada) {
        SignalRLogs.DisplayConsoleLogs("MudarParaEdicao", idChamada);
        var trChamada = document.querySelector("tr[data-chamadaid='" + idChamada + "']");
        if (trChamada) {
            var posicionados = trChamada.cells[1].innerText;
            if (posicionados > 0) {
                trChamada.cells[6].innerHTML = "<a href='/Analista/Chamada/EditarChamada?chamadaId=" + idChamada + "' class='btn btn-primary'>Editar</a>"
                var tablegainloss = $(".table-gainloss tbody")
                tablegainloss.prepend(trChamada);
            }
            else {
                trChamada.remove();
            }
        }
    });

    signalrclass.on("AddPosicionado", function (idChamada) {
        SignalRLogs.DisplayConsoleLogs("AddPosicionado", idChamada);
        var trChamada = document.querySelector("tr[data-chamadaid='" + idChamada + "']");
        if (trChamada) {
            var posicionados = parseInt(trChamada.cells[1].innerText);
            trChamada.cells[1].innerHTML = "<a href='/Analista/chamada/Posicionados?chamadaId=" + idChamada + "'>" + ++posicionados + " </a>";
        }
    });

    signalrclass.on("RemoverEdicao", function (idChamada) {
        SignalRLogs.DisplayConsoleLogs("RemoverEdicao", idChamada);
        var trChamada = document.querySelector("tr[data-chamadaid='" + idChamada + "']");
        if (trChamada) {
            trChamada.remove();
        }
    });
    connection.start();
}); 