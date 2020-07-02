import moment from 'moment'

export function fadeOut(element) {
    var op = 1;  // initial opacity
    var timer = setInterval(function () {
        if (op <= 0) {
            clearInterval(timer);
        }
        element.style.opacity = op;
        op -= op * 0.1;
    }, 50);
}

export function fadeIn(element) {
    var op = 0.1;  // initial opacity
    var timer = setInterval(function () {
        if (op >= 1) {
            clearInterval(timer);
        }
        element.style.opacity = op;
        op += op * 0.1;
    }, 50);
}

export function counttime() {
    var timers = document.getElementsByClassName("counttime");
    for (var item of timers) {
        var texthorario = item.getAttribute("data-horario");
        var DataCadastro = new Date(texthorario);
        var DataAtual = new Date();
        var qtdMilissegundos = DataAtual - DataCadastro;
        var qtdSegundos = Math.floor(qtdMilissegundos / 1000);
        var qtdminutos = Math.floor(qtdSegundos / 60);
        var qtdHoras = Math.floor(qtdminutos / 60);
        var qtddias = Math.floor(qtdHoras / 24);
        if (qtddias >= 1) {
            item.innerText = qtddias + (qtddias == 1 ? " dia" : " dias");
        }
        else if (qtdHoras >= 1) {
            item.innerText = qtdHoras + (qtdHoras == 1 ? " hora" : " horas");
        }
        else if (qtdminutos >= 1) {
            item.innerText = qtdminutos + (qtdminutos == 1 ? " minuto" : " minutos");
        }
        else {
            item.innerText = "agora";
        }
    }
}

export function formatHourMenu(data) {
    var d = new Date(data);
    var day = d.getDate();
    var month = d.getMonth() + 1;
    var year = d.getFullYear();
    var hour = d.getHours();
    var minutes = d.getMinutes();
    var seconds = d.getSeconds();
    if (day < 10) {
        day = "0" + day;
    }
    if (month < 10) {
        month = "0" + month;
    }
    if (hour < 10) {
        hour = "0" + hour;
    }
    if (minutes < 10) {
        minutes = "0" + minutes;
    }
    if (seconds < 10) {
        seconds = "0" + seconds;
    }
    var date = month + "/" + day + "/" + year + " " + hour + ":" + minutes + ":" + seconds;
    return date;
}

export function showTime(date) {
    var DataCadastro = new Date(date);
    var DataAtual = new Date();
    var qtdMilissegundos = DataAtual - DataCadastro;
    var qtdSegundos = Math.floor(qtdMilissegundos / 1000);
    var qtdminutos = Math.floor(qtdSegundos / 60);
    var qtdHoras = Math.floor(qtdminutos / 60);
    var qtddias = Math.floor(qtdHoras / 24);

    if (qtddias >= 1) {
        return qtddias + (qtddias == 1 ? " dia" : " dias");
    }
    else if (qtdHoras >= 1) {
        return qtdHoras + (qtdHoras == 1 ? " hora" : " horas");
    }
    else if (qtdminutos >= 1) {
        return qtdminutos + (qtdminutos == 1 ? " minuto" : " minutos");
    }
    else {
        return "agora";
    }
}

export function AtualizarStatusTabela(ordemId, Status_Id, Msg) {
    var tr = document.querySelector(".table-list-posicionadas tr[data-ordemId='" + ordemId + "']");
    if (tr) {
        tr.setAttribute("data-status", Status_Id);
        tr.cells[2].innerText = Msg;
        return tr;
    }
}

export function PercentualGain(valorentrada, valorstop) {
    var percentualLucro = 0;
    percentualLucro = ((valorstop - valorentrada) / valorentrada) * 100;
    return percentualLucro.toFixed(2);
}

export function PercentualLoss(valorentrada, valorstop) {
    var percentualLucro = 0;
    percentualLucro = ((valorentrada - valorstop) / valorentrada) * 100;
    return percentualLucro.toFixed(2);
}

export function WebppAppRedirect(data) {
    if (data.status == 401) {
        window.location.href = "/usuario/login/index";
    }
}