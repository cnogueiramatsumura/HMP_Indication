export var SignalRLogs = (function () {
    var ShowLogs = true;
    return {
        DisplayConsoleLogs: function (nome_metodo, obj) {
            if (ShowLogs) {
                var objtype = typeof obj;
                if (objtype == "object") {
                    console.log(nome_metodo + " => ");
                    console.log(obj);
                }
                else {
                    console.log(nome_metodo + " => " + obj);
                }
            }
        }
    }
})();