$(function () {

    $("#testkeys").click(function () {
        if ($("#binanceKey").val() == "" || $("#binanceSecret").val() == "") {
            $(".validation-summary-valid").html("<ul><li>O campo binanceKey é obrigatório.</li><li>O campo binanceSecret é obrigatório.</li></ul>");
            $(".validation-summary-errors").html("<ul><li>O campo binanceKey é obrigatório.</li><li>O campo binanceSecret é obrigatório.</li></ul>");
        }
        else {
            $.ajax({
                url: "/usuario/binance/TestKey",
                data: {
                    binanceKey: $("#binanceKey").val(),
                    binanceSecret: $("#binanceSecret").val()
                },
                success: function (data) {
                    alert("Chave Api Configurada Corretamente");
                },
                error: function (data) {
                    if (data.status == 400) {
                        var jsonresponse = JSON.parse(data.responseJSON);
                        alert(jsonresponse.motivo ? jsonresponse.motivo : jsonresponse.msg);
                    }
                    else if (data.status == 401) {
                        window.location.href = "/usuario/login/index";
                    }
                    else if (data.status == 406) {
                        alert(data.responseJSON.replace(/"/g, ''));
                    }
                }
            })
        }
    })
})
