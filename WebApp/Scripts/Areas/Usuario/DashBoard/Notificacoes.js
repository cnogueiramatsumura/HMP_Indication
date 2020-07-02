import { fadeIn, fadeOut } from './../../ComunFunctions.js'

export var Notificacoes = (function () {
    return {
        ShowNotificacao: function (className, Msg) {
            var divnot = document.createElement("div");
            divnot.style.opacity = 0.1;
            var spanfechamento = document.createElement("span");
            spanfechamento.innerText = "x";
            spanfechamento.classList.add("float-right");
            spanfechamento.classList.add("fechar-notificacao");

            var paragraf = document.createElement("p");
            paragraf.classList.add("text-center");
            paragraf.classList.add("notparagraf");
            paragraf.innerText = Msg;
            paragraf.appendChild(spanfechamento);

            divnot.classList.add(className);
            divnot.appendChild(paragraf);

            var wrapper = document.getElementsByClassName("posicionadas-notificacao")[0];
            wrapper.appendChild(divnot);

            fadeIn(divnot);

            setTimeout(function () {
                fadeOut(divnot);
                setTimeout(function () {
                    divnot.remove();
                }, 1000);
            }, 10000);
        },

        RemoverNotificacao: function (element) {
            //var divnot = element.parentNode.parentNode;
            fadeOut(element);
            setTimeout(function () {
                element.remove();
            }, 1000);
        },

        NotificacaoCancelamentoEntrada(chamadaId) {
            var divnot = document.createElement("div");
            divnot.style.opacity = 0.1;
            var spanfechamento = document.createElement("span");
            spanfechamento.innerText = "x";
            spanfechamento.classList.add("float-right");
            spanfechamento.classList.add("fechar-notificacao");

            var paragraf = document.createElement("p");
            paragraf.classList.add("text-center");
            paragraf.classList.add("notparagraf");
            paragraf.innerText = "Indicação de Cancelamento de entrada ";

            var spanshowCancel = document.createElement("span");
            spanshowCancel.classList.add("mostrarcancelamento");
            spanshowCancel.innerText = " clique aqui";
            spanshowCancel.setAttribute("data-chamadaId", chamadaId);
            paragraf.appendChild(spanshowCancel);
            paragraf.appendChild(spanfechamento);

            divnot.classList.add("notif-CancelEntrada");
            divnot.appendChild(paragraf);

            var wrapper = document.getElementsByClassName("posicionadas-notificacao")[0];
            wrapper.appendChild(divnot);
            fadeIn(divnot);
        }
    }
})();