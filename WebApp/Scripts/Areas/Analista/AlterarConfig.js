$(function () {
    $("#edit-PrecoLicenca").click(function () {
        $("#PrecoLicenca").prop("disabled", false);
        $("#salvar-PrecoLicenca").prop("disabled", false);
        $(this).prop("disabled", true);
    });


    $("#reset-SMTP").click(function () {
        $.ajax({
            url: "/Analista/configuracoes/resetSMTP",
            method: "POST",
            success: function () {
                $(".validation-summary-errors").empty();
                $("#alertas-msgs").append("<div class='alert alert-warning alert-dismissible' role='alert'>" +
                    "<h4 class='alert-heading'>Atenção!</h4>" +
                    "<p>As configurações de SMT foram resetadas, Seus emails de cadastro, e alteração de senha serao enviados por um email padrão</p>" +
                    "<button type='button' class='close' data-dismiss='alert' aria-label='Close'>" +
                    "<span aria-hidden='true'>&times;</span>" +
                    "</button> </div>");
                $('#modalresetSMTP .close').click();
            },
            error: function () {
                $(".validation-summary-errors").empty();
                $("#alertas-msgs").append("<div class='alert alert-danger alert-dismissible' role='alert'>" +
                    "<h4 class='alert-heading'>Atenção!</h4>" +
                    "<p>Houve um erro ao tentar resetar as configuraçoes, consulte a administraçao de sistemas para corrigir o problema</p>" +
                    "<button type='button' class='close' data-dismiss='alert' aria-label='Close'>" +
                    "<span aria-hidden='true'>&times;</span>" +
                    "</button> </div>");
                $('#modalresetSMTP .close').click();
            },
            complete: function () {
            }
        })
    })
})