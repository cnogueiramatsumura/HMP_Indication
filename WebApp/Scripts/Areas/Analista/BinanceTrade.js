export const BinanceTrade = (function () {
    var ws = null;
    var lastvalue;
    return {
        connect: function (symbol, digits) {
            ws = new WebSocket("wss://stream.binance.com:9443/stream?streams=" + symbol + "@trade");
            ws.onopen = function (msg) {
                console.log("conectado Binance Trade. => Symbol => " + symbol + " =>  hora => " + new Date());
            }

            ws.onmessage = function (obj) {
                var marketvalue = parseFloat(JSON.parse(obj.data).data.p).toFixed(digits);            
                $("#v-mercado").text(marketvalue)
                if (marketvalue > lastvalue) {
                    $("#v-mercado").addClass("marketupper");
                    $("#v-mercado").removeClass("marketdown");
                }
                else if (marketvalue < lastvalue) {
                    $("#v-mercado").addClass("marketdown");
                    $("#v-mercado").removeClass("marketupper");
                }
                else if (marketvalue == lastvalue) {
                    $("#v-mercado").removeClass("marketdown");
                    $("#v-mercado").removeClass("marketupper");
                }
                $("#ValorMercado").val(marketvalue);
                lastvalue = marketvalue;
            }

            ws.onerror = function (error) {
                console.log(error);
            }

            ws.onclose = function (msg) {
                console.log("Trader Disconect. hora => " + new Date());
            }
        },
        disconnect: function () {
            if (ws != null) {
                ws.close(1000, "Closing Connection Normally");
            }
        },
        IsConnected: function () {
            if (ws != null && ws.readyState == 1) {
                return true;
            }
        },
        currentState: function () {
            if (ws != null) {
                return ws.readyState;
            }
            else {
                return ws;
            }
        },
        currentUrl: function () {
            if (ws != null) {
                return ws.url;
            }
        }
    }
})()