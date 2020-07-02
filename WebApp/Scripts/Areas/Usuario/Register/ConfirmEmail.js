import * as signalR from 'signalr'

$(function () {
    $("#resend-email").click(function () {
        $.ajax({
            url: "http://api.test.com.br:90/api/Login/ReenviarEmail",
            method: "GET",
            data: { "token": mytoken },
            success: function (result) {
                alert(result);
            },
            error: function (msg) {
                alert("houve um erro, entre em contato com a admnistração.")
            }
        })
    });

    var connection = $.hubConnection(ApiDomainName, { useDefaultPath: true, logging: true });
    var signalrclass = connection.createHubProxy('signalChamadas');

    signalrclass.on("EmailConfirmado", function (token) {
        if (mytoken == token) {
            var contador = 10;
            $(".display-email-log").html("<i class='uil uil-envelope-open'></i>");
            $(".link-enviado").html("<p>Seu email foi confirmado</p>");
            $(".desc-email").html("<h2>Redirecionando...</h2>");
            $(".desc-email").append("<p class='contador'>" + contador + "</p>");

            setInterval(function () {
                contador--;
                $(".contador").text(contador);
                if (contador <= 0) {
                    contador = 0;
                    window.location.href = "/usuario/dashboard";
                }
            }, 1000);
        }
    });   
    connection.start();
})