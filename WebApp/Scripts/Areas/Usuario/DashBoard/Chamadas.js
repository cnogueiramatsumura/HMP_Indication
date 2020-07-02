import { fadeIn, formatHourMenu, PercentualGain, PercentualLoss, showTime } from './../../ComunFunctions.js'
import { Notificacoes } from './Notificacoes.js'
import { Menu } from './Menu.js'

export const Chamadas = (function () {
    var posFilter = 1;  //variavel definel qual foi o ultimo filtro clicado abertas; encerradas ou recusadas
    return {
        AddChamada: function (chamada) {
            if (!this.PossuiAbertas()) {
                this.Clear();
            }

            if (posFilter == 1) {
                var menu = document.getElementById("painel-chamadas");
                var wrapper = document.createElement("div");
                wrapper.classList.add("card-chamadas");
                wrapper.setAttribute("data-chamadaId", chamada.id);
                wrapper.setAttribute("data-symbol", chamada.symbol.symbol);
                wrapper.setAttribute("data-entrada", parseFloat(chamada.precoEntrada).toFixed(8));
                wrapper.setAttribute("data-gain", parseFloat(chamada.precoGain).toFixed(8));
                wrapper.setAttribute("data-rangeentrada", parseFloat(chamada.rangeEntrada).toFixed(8));
                wrapper.setAttribute("data-loss", parseFloat(chamada.precoLoss).toFixed(8));
                wrapper.setAttribute("data-indicado", chamada.percentualIndicado);
                wrapper.setAttribute("data-observacao", chamada.observacao);
                wrapper.style.opacity = 0.1;

                var divcardheader = document.createElement("div");
                divcardheader.classList.add("card-chamada-header");
                var cardheaderp1 = document.createElement("p");
                var icon = document.createElement("i");
                icon.classList.add("fas");
                icon.classList.add("fa-user-circle");
                var span1 = document.createElement("span");
                span1.innerText = "Doing Now | " + chamada.analista.nome;
                var span2 = document.createElement("span");
                span2.setAttribute("data-id", chamada.id);
                span2.classList.add("float-right");
                span2.classList.add("recusarchamada");
                span2.innerText = "x";
                var cardheaderp2 = document.createElement("p");
                cardheaderp2.classList.add("text-right");
                cardheaderp2.classList.add("counttime");
                cardheaderp2.innerText = "agora";
                cardheaderp2.setAttribute("data-horario", formatHourMenu(chamada.dataCadastro))

                cardheaderp1.append(icon);
                cardheaderp1.append(span1);
                cardheaderp1.append(span2);
                divcardheader.append(cardheaderp1);
                divcardheader.append(cardheaderp2);
                wrapper.append(divcardheader);

                var divrow = document.createElement("div");
                divrow.classList.add("row");

                var col1 = document.createElement("div");
                col1.classList.add("col-6");

                var cardbody1 = document.createElement("div");
                cardbody1.classList.add("card-chamada-body-1");
                var body1p1 = document.createElement("p");
                body1p1.innerText = chamada.symbol.baseAsset + " / " + chamada.symbol.quoteAsset;
                var body1p2 = document.createElement("p");
                body1p2.innerText = chamada.symbolDescription;
                var body1p3 = document.createElement("p");
                body1p3.innerText = parseFloat(chamada.precoEntrada).toFixed(8) + "";
                cardbody1.append(body1p1);
                cardbody1.append(body1p2);
                cardbody1.append(body1p3);
                col1.append(cardbody1);

                var col2 = document.createElement("div");
                col2.classList.add("col-6");

                var cardbody2 = document.createElement("div");
                cardbody2.classList.add("card-chamada-body-2");
                cardbody2.classList.add("text-center");

                var body2p1 = document.createElement("p");
                body2p1.innerText = "objetivo";
                body2p1.classList.add("p-desc-objetivo")

                var body2pobjetivo = document.createElement("p");
                body2pobjetivo.innerText = PercentualGain(chamada.precoEntrada, chamada.precoGain) + "% ";
                body2pobjetivo.classList.add("p-objetivo")

                var spanArrowUp = document.createElement("i")
                spanArrowUp.classList.add("fa")
                spanArrowUp.classList.add("fa-caret-up")
                spanArrowUp.classList.add("icon-arrowup")
                body2pobjetivo.append(spanArrowUp);

                var body2p2 = document.createElement("p");
                body2p2.innerText = "risco";
                body2p2.classList.add("p-desc-risco")

                var body2prisco = document.createElement("p");
                body2prisco.innerText = PercentualLoss(chamada.precoEntrada, chamada.precoLoss) + "% ";
                body2prisco.classList.add("p-risco");

                var spanArrowdown = document.createElement("ion-icon")
                spanArrowdown.classList.add("fa")
                spanArrowdown.classList.add("fa-caret-down")
                spanArrowdown.classList.add("icon-arrowdown")
                body2prisco.append(spanArrowdown);

                cardbody2.append(body2p1);
                cardbody2.append(body2pobjetivo);
                cardbody2.append(body2p2);
                cardbody2.append(body2prisco);
                col2.append(cardbody2)

                divrow.append(col1);
                divrow.append(col2);

                wrapper.append(divrow);
                menu.prepend(wrapper);
                fadeIn(wrapper);
            }
        },

        LoadAtivas: function (obj) {
            var menu = $("#painel-chamadas")
            var wrapperdiv = $("<div class='card-chamadas' data-chamadaid='" + obj.id + "' data-symbol='" + obj.symbol.symbol + "' data-entrada='" + parseFloat(obj.precoEntrada).toFixed(8) + "' data-rangeentrada='" + parseFloat(obj.rangeEntrada).toFixed(8) + "' data-gain='" + parseFloat(obj.precoGain).toFixed(8) + "' data-loss='" + parseFloat(obj.precoLoss).toFixed(8) + "' data-indicado='" + obj.percentualIndicado + "' data-horario='" + obj.dataCadastro + "' data-observacao='" + obj.observacao + "'></div>")
            wrapperdiv.css("opacity", 0.1);
            var divCardHeader = $("<div class='card-chamada-header'><p><i class='fas fa-user-circle'></i> <span>Doing Now | " + obj.analista.nome + "</span><span data-id='" + obj.id + "' class='float-right recusarchamada'>x</span></p><p class='text-right counttime' data-horario='" + formatHourMenu(obj.dataCadastro) + "'>" + showTime(obj.dataCadastro) + "</p></div>")
            var divrow = $("<div class='row'></div>")
            var divrow1 = $("<div class='col-6'><div class='card-chamada-body-1'><p>" + obj.symbol.baseAsset + " / " + obj.symbol.quoteAsset + "</p><p>" + obj.symbolDescription + "</p><p>" + obj.precoEntrada + "</p></div></div>");
            var divrow2 = $("   <div class='col-6'>" +
                "<div class='card-chamada-body-2 text-center' > " +
                "<p class='p-desc-objetivo'>Objetivo</p>" +
                "<p class='p-objetivo'>" + PercentualGain(obj.precoEntrada, obj.precoGain) + " % <i class='fa fa-caret-up icon-arrowup'></i></p>" +
                "<p class='p-desc-risco'>Risco</p>" +
                "<p class='p-risco'>" + PercentualLoss(obj.precoEntrada, obj.precoLoss) + " % <i class='fa fa-caret-down icon-arrowdown'></i></p>" +
                "</div>" +
                "</div>")
            divrow.append(divrow1);
            divrow.append(divrow2);
            wrapperdiv.append(divCardHeader);
            wrapperdiv.append(divrow);
            menu.append(wrapperdiv);
            fadeIn(wrapperdiv[0]);
        },

        LoadEditadas: function (obj) {
            var menu = $("#painel-editadas")
            var wrapperDiv = $("<div class='card-Edicao' data-horario='" + obj.dataEdicao + "' data-entrada='" + obj.precoEntrada.toFixed(8) + "' data-rangeentrada='" + obj.rangeEntrada.toFixed(8) + "' data-gain='" + obj.precoGain.toFixed(8) + "' data-loss='" + obj.precoLoss.toFixed(8) + "' data-chamadaid='" + obj.chamada_Id + "' data-edicaoid='" + obj.id + "' data-symbol='" + obj.symbol + "' data-newgain='" + obj.newGain.toFixed(8) + "' data-newloss='" + obj.newLoss.toFixed(8) + "'></div>");
            var div1 = $("<div><h3 class='header-Edicao'>Edição " + obj.baseAsset + " / " + obj.quoteAsset + "</h3></div>");
            var div2 = $("<div class='div-buttons-edicao'><button class='btn-visualizar-edicao'>Visualizar</button> <button class='btn-aceitar-edicao'>Aceitar</button><button class='btn-recusar-edicao'><i class='uil uil-trash-alt'></i></button></div>");
            wrapperDiv.append(div1);
            wrapperDiv.append(div2);
            menu.append(wrapperDiv);
        },

        LoadCanceladas: function (obj) {
            var menu = $("#painel-canceladas")
            var wrapperDiv = $("<div class='card-cancelar' data-ordemid='" + obj.ordemId + "' data-entrada='" + obj.precoEntrada.toFixed(8) + "' data-rangeentrada='" + obj.rangeEntrada.toFixed(8) + "' data-gain='" + obj.precoGain.toFixed(8) + "' data-loss='" + obj.precoLoss.toFixed(8) + "' data-qtd='" + obj.quantidade + "' data-observacao='" + obj.observacao + "' data-symbol='" + obj.symbol + "' data-cancelamentochamadaid='" + obj.id + "' data-chamadaid='" + obj.chamada_Id + "'></div>");
            var div1 = $("<div><h3 class='header-cancelar'>Cancelar " + obj.baseAsset + " / " + obj.quoteAsset + "</h3></div>");
            var div2 = $("<div class='div-buttons-edicao'><button class='btn-visualizar-cancelamento'>Visualizar</button> <button class='btn-aceitar-cancelamento'>Aceitar</button><button class='btn-recusar-cancelamento'><i class='uil uil-trash-alt'></i></button></div>");
            wrapperDiv.append(div1);
            wrapperDiv.append(div2);
            menu.append(wrapperDiv);
        },

        LoadEncerradas_e_Recusadas: function (obj, classname) {
            var menu = $("#painel-chamadas")
            var wrapperdiv = $("<div class='" + classname + "' data-chamadaid='" + obj.id + "' data-symbol='" + obj.symbol.symbol + "' data-entrada='" + parseFloat(obj.precoEntrada).toFixed(8) + "' data-gain='" + parseFloat(obj.precoGain).toFixed(8) + "' data-loss='" + parseFloat(obj.precoLoss).toFixed(8) + "' data-indicado='" + obj.percentualIndicado + "'></div>")
            wrapperdiv.css("opacity", 0.1);
            var divCardHeader = $("<div class='card-chamada-header'><p><i class='fas fa-user-circle'></i> <span>Doing Now | " + obj.analista.nome + "</span></p><p class='text-right counttime' data-horario='" + formatHourMenu(obj.dataCadastro) + "'>" + showTime(obj.dataCadastro) + "</p></div>")
            var divrow = $("<div class='row'></div>")
            var divrow1 = $("<div class='col-6'><div class='card-chamada-body-1'><p>" + obj.symbol.baseAsset + " / " + obj.symbol.quoteAsset + "</p><p>" + obj.symbolDescription + "</p><p>" + obj.precoEntrada + "</p></div></div>");
            var divrow2 = $("   <div class='col-6'>" +
                "<div class='card-chamada-body-2 text-center' > " +
                "<p class='p-desc-objetivo'>Objetivo</p>" +
                "<p class='p-objetivo'>" + PercentualGain(obj.precoEntrada, obj.precoGain) + " % <i class='fa fa-caret-up icon-arrowup'></i></p>" +
                "<p class='p-desc-risco'>Risco</p>" +
                "<p class='p-risco'>" + PercentualLoss(obj.precoEntrada, obj.precoLoss) + " % <i class='fa fa-caret-down icon-arrowdown'></i></p>" +
                "</div>" +
                "</div>")
            divrow.append(divrow1);
            divrow.append(divrow2);
            wrapperdiv.append(divCardHeader);
            wrapperdiv.append(divrow);
            menu.append(wrapperdiv);
            fadeIn(wrapperdiv[0]);
        },

        AddEdicao: function (ChamadaEditada) {
            if (!this.PossuiAbertas()) {
                this.Clear();
            }

            if (posFilter == 1) {
                var cardAntigo = document.querySelector(".card-Edicao[data-chamadaId='" + ChamadaEditada.chamada_Id + "']");
                if (cardAntigo) {
                    cardAntigo.remove()
                };

                var menu = $("#painel-editadas");
                var wrapper = document.createElement("div");
                wrapper.classList.add("card-Edicao");
                wrapper.setAttribute("data-chamadaId", ChamadaEditada.chamada_Id);
                wrapper.setAttribute("data-EdicaoId", ChamadaEditada.id);
                wrapper.setAttribute("data-symbol", ChamadaEditada.symbol);
                wrapper.setAttribute("data-newGain", ChamadaEditada.newGain.toFixed(8));
                wrapper.setAttribute("data-newLoss", ChamadaEditada.newLoss.toFixed(8));
                wrapper.setAttribute("data-entrada", ChamadaEditada.chamada.precoEntrada.toFixed(8));
                wrapper.setAttribute("data-rangeEntrada", ChamadaEditada.chamada.rangeEntrada.toFixed(8));
                wrapper.setAttribute("data-gain", ChamadaEditada.chamada.precoGain.toFixed(8));
                wrapper.setAttribute("data-loss", ChamadaEditada.chamada.precoLoss.toFixed(8));
                wrapper.style.opacity = 0.1;

                var divitem1 = document.createElement("div");
                var header = document.createElement("h3");
                header.classList.add("header-Edicao")
                header.innerText = "Edição " + ChamadaEditada.symbol;
                divitem1.append(header);

                var divitem2 = document.createElement("div");
                divitem2.classList.add("div-buttons-edicao")
                var buttonvisualizar = document.createElement("button");
                buttonvisualizar.innerText = "Visualizar";
                buttonvisualizar.classList.add("btn-visualizar-edicao");

                var buttonEdicao = document.createElement("button")
                buttonEdicao.innerText = "Aceitar";
                buttonEdicao.classList.add("btn-aceitar-edicao");

                var recusaredicao = document.createElement("button")
                recusaredicao.classList.add("btn-recusar-edicao");

                var icon = document.createElement("i");
                icon.classList.add("uil");
                icon.classList.add("uil-trash-alt");
                recusaredicao.append(icon);

                divitem2.append(buttonvisualizar);
                divitem2.append(buttonEdicao);
                divitem2.append(recusaredicao);

                wrapper.append(divitem1);
                wrapper.append(divitem2);

                menu.prepend(wrapper);
                fadeIn(wrapper);
            }
        },

        RecusarChamada: function (chamadaId) {
            var chamada = document.querySelector(".card-chamadas:not(.chamada-encerrada):not(.chamada-recusada)[data-chamadaid='" + chamadaId + "']");
            if ($("#chamadaId").val() == chamadaId) {
                Menu.ShowPosicionadasContent();
            }
            if (chamada != null) {
                chamada.remove();
                Notificacoes.ShowNotificacao("notif-error", "Você recusou uma chamada.");
            }
        },

        EncerrarChamada: function (chamadaId) {
            var chamada = document.querySelector(".card-chamadas[data-chamadaid='" + chamadaId + "']");
            if ($("#chamadaId").val() == chamadaId) {
                Menu.ShowPosicionadasContent();
            }
            if (chamada) {
                chamada.remove();
            }
            Notificacoes.ShowNotificacao("notif-error", "Chamada Encerrada.");
        },

        CancelarEntrada: function (symbol) {
            Notificacoes.ShowNotificacao("notif-error", "O Analista Indicou o Cancelamento da Entrada do Symbol " + symbol);
        },

        DeletaChamada: function (chamadaId, shownotif) {
            var chamada = document.querySelector(".card-chamadas:not(.chamada-encerrada):not(.chamada-recusada)[data-chamadaid='" + chamadaId + "']");
            if ($("#chamadaId").val() == chamadaId) {
                Menu.ShowPosicionadasContent();
            }
            if (chamada) {
                //fadeOut(chamada);
                setTimeout(function () {
                    chamada.remove();
                }, 500);
            }
            if (shownotif) {
                Notificacoes.ShowNotificacao("notif-error", "Ponto de Entrada Ultrapassado, não é mais possível aceitar essa indicação.");
            }
        },

        Clear: function () {          
            $("#painel-canceladas").html("");
            $("#painel-editadas").html("");
            $("#painel-chamadas").html("");
        },

        AtualizarFiltro: function (posicao, element) {
            posFilter = posicao;
            $("#filter-chamada-abertas").removeClass("ultimofiltro");
            $("#filter-chamada-encerradas").removeClass("ultimofiltro");
            $("#filter-chamada-recusadas").removeClass("ultimofiltro");
            element.classList.add("ultimofiltro");
        },

        PossuiAbertas: function () {
            var chamadas = document.getElementsByClassName("card-chamadas");
            var Editadas = document.getElementsByClassName("card-Edicao");
            var Canceladas = document.getElementsByClassName("card-cancelar");
            return chamadas.length > 0 || Editadas.length > 0 || Canceladas.length > 0;
        }
    }
})()