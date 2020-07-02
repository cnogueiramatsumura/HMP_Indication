import './picker'
import * as Picker from './picker.date'

$(document).ready(function () {

    var browserLocale = navigator.language || navigator.userLanguage;

    var opt = {
        today: 'Hoje',
        clear: 'Limpar',
        close: 'Cancelar',
        //selectMonths: true,
        //selectYears: true,
        max: new Date(),
    }

    $("#dataInicio").pickadate(opt);
    $("#dataFim").pickadate(opt);

    $(".p-Gain").click(function () {
        $(".Partial-Gain").removeClass("d-none")
        $(".Partial-Loss").addClass("d-none")
        $(".Partial-Mercado").addClass("d-none")
    })

    $(".p-Loss").click(function () {
        $(".Partial-Gain").addClass("d-none")
        $(".Partial-Loss").removeClass("d-none")     
        $(".Partial-Mercado").addClass("d-none")
    })

    $(".p-Mercado").click(function () {
        $(".Partial-Gain").addClass("d-none")
        $(".Partial-Loss").addClass("d-none")
        $(".Partial-Mercado").removeClass("d-none")
    })

    $("#btn-buscar-relatorio").on('click', function () {
        var datainicio = $("#dataInicio").val();
        var datafim = $("#dataFim").val();

        if (!datainicio || !datafim) {
            alert("Selecione o periodo")
            event.preventDefault();
            return;
        }
        window.location.href = "/Usuario/Relatorios/Individual?dataInicio=" + datainicio + "&DataFim=" + datafim;
    })

    $("#periodo").on('change', function () {
        var periodo = $("#periodo").val();
        //alert(peridodo)
        window.location.href = "/Usuario/Relatorios/Individual?periodo=" + periodo;
    })

})