$(function () {
    $("#pagseguro-pagamento").click(function () {
        $.ajax({
            url: "/usuario/pagamentos/PagSeguroPayment",
            method: "POST",
            success: function (data) {
                window.open(data, "_blank");
                window.location.href = "/usuario/pagamentos/check";               
            },
            error: function (data) {
                if ((data.status == 400 || data.status == 401)) {
                    window.location.href = "/usuario/login/index";
                }
                else {
                    alert(data.statusText);
                }
            }
        })
    })

    $("#bitpay-pagamento").click(function () {
        $.ajax({
            url: "/usuario/pagamentos/bitpayPayment",
            method: "POST",
            success: function (data) {
                window.open(data, "_blank");
                window.location.href = "/usuario/pagamentos/check";
            },
            error: function (data) {
                if ((data.status == 400 || data.status == 401) && data.responseText != '"Erro no metodo de Pagamento"' ) {
                    window.location.href = "/usuario/login/index";
                }
                else {
                    alert(data.statusText);
                }
            }
        })
    })
})