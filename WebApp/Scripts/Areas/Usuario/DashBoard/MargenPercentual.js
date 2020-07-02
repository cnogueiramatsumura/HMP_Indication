export var MargenPercentual = (function () {
    var listobj = [];

    return {
        init: function (ListOrdens) {
            for (var i = 0; i < ListOrdens.length; i++) {
                var ws;
                var obj = { "symbol": ListOrdens[i].symbol, "ws": ws };
                if (ListOrdens[i].OrdemStatus_Id == 3) {
                    this.Add(obj);
                }
            }
        },

        Add: function (obj) {
            var hasmonitor = this.checkSymbol(obj.symbol);
            if (!hasmonitor) {
                obj.ws = new WebSocket("wss://stream.binance.com:9443/stream?streams=" + obj.symbol.toLowerCase() + "@trade");

                obj.ws.onopen = function () {
                    console.log("%cMonitoramento Margen Conectado => " + obj.symbol.toLowerCase(), "background: #3F51B5;color:#FFF;padding:5px;border-radius: 8px;line-height: 15px;")
                }

                obj.ws.onmessage = function (res) {
                    var marketvalue = parseFloat(JSON.parse(res.data).data.p).toFixed(8);
                    obj.marketvalue = marketvalue;
                    var OrdensPosicionadas = document.querySelectorAll(".OrdensAtivas[data-symbol='" + obj.symbol + "'][data-status='3']");
                    OrdensPosicionadas.forEach(function (element, index) {
                        var entrada = element.getAttribute("data-entrada");
                        var percentual = (((marketvalue - entrada) / entrada) * 100).toFixed(2);
                        element.cells[7].innerText = percentual + " %";
                        if (marketvalue > entrada) {
                            element.cells[7].classList.add("color-gain");
                            element.cells[7].classList.remove("color-loss");
                        }
                        else {
                            element.cells[7].classList.remove("color-gain");
                            element.cells[7].classList.add("color-loss");
                        }
                    })
                }

                obj.ws.onerror = function (error) {
                }

                obj.ws.onclose = function (event) {
                    console.log("%cMonitoramento Margen Desconectado => " + obj.symbol + ".", "background:red;color:#FFF;padding:5px;border-radius: 8px;line-height: 15px;")
                }
                listobj.push(obj);
            }
        },

        Remove: function (symbol) {
            var obj = this.GetObjBySymbol(symbol);
            var hasOrder = this.hasOrder(symbol);
            if (obj.ws != null && !hasOrder) {
                obj.ws.close(1000, "Closing Connection Normally");
                listobj.splice(this.GetObjIndex(symbol), 1);
            }
        },

        checkSymbol: function (symbol) {
            var obj = this.GetObjBySymbol(symbol);
            return obj != null;
        },

        GetObjBySymbol: function (symbol) {
            var margen = listobj.filter(function (element, index) {
                return element.symbol == symbol;
            })
            return margen[0];
        },

        GetObjIndex: function (symbol) {
            return listobj.findIndex(x => x.symbol == symbol);
        },

        hasOrder: function (symbol) {
            var Order = document.querySelectorAll(".OrdensAtivas[data-symbol='" + symbol + "'][data-status='3']");
            return Order.length > 0;
        },

        showMonitors: function () {
            console.log(listobj);
        },

        updaterows: function () {
            listobj.forEach(function (obj, index) {
                var OrdensPosicionadas = document.querySelectorAll(".OrdensAtivas[data-symbol='" + obj.symbol + "'][data-status='3']");
                OrdensPosicionadas.forEach(function (element, index) {
                    var entrada = element.getAttribute("data-entrada");
                    var percentual = (((obj.marketvalue - entrada) / entrada) * 100).toFixed(2);
                    element.cells[7].innerText = percentual + " %";
                    if (obj.marketvalue > entrada) {
                        element.cells[7].classList.add("color-gain");
                        element.cells[7].classList.remove("color-loss");
                    }
                    else {
                        element.cells[7].classList.remove("color-gain");
                        element.cells[7].classList.add("color-loss");
                    }
                })
            })
        }
    }
})();