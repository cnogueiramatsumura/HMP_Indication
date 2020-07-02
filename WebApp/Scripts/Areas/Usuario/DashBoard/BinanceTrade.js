export var BinanceTrade = (function () {
    var ws = null;
    var _entrada = null;
    var _gain = null;
    var _loss = null;
    var changecolor = false;
    return {
        connect: function (symbol, entrada, gain, loss, Posicionado) {
            $.ajax({
                url: '/usuario/marketvalue/GetMarketPriece',
                data: { "symbol": symbol },
                success: function (marketvalue) {
                    $("#valor-mercado").text(marketvalue);
                    if (marketvalue > _entrada) {
                        var maxvalue = _gain - _entrada;
                        var qtd_na_escala = marketvalue - _entrada;
                        var variacaopercentual = parseInt((qtd_na_escala / maxvalue) * 100);
                        $("#progress-gain").css("width", (variacaopercentual >= 100) ? "100%" : variacaopercentual + "%");
                        $("#progress-loss").css("width", 0);
                        if (changecolor) {
                            $(".chamadas-header").css("background-color", "#75CB7C");
                        }

                    }
                    else if (marketvalue < _entrada) {
                        var maxvalue = _entrada - _loss;
                        var qtd_na_escala = _entrada - marketvalue;
                        var variacaopercentual = parseInt((qtd_na_escala / maxvalue) * 100);
                        $("#progress-loss").css("width", (variacaopercentual >= 100) ? "100%" : variacaopercentual + "%");
                        $("#progress-gain").css("width", 0);
                        if (changecolor) {
                            $(".chamadas-header").css("background-color", "red");
                        }

                    }
                    else if (marketvalue == _entrada) {
                        $("#progress-gain").css("width", 0);
                        $("#progress-loss").css("width", 0);
                    }
                },
                error: function (data) {
                    if (data.status == 401) {
                        window.location.href = data.responseJSON.url;
                    }
                }
            })

            ws = new WebSocket("wss://stream.binance.com:9443/stream?streams=" + symbol.toLowerCase() + "@trade");          
            _entrada = parseFloat(entrada).toFixed(8);
            _gain = parseFloat(gain).toFixed(8);
            _loss = parseFloat(loss).toFixed(8);

            $("#progress-gain").css("width", 0);
            $("#progress-loss").css("width", 0);

            if (Posicionado) {
                $("#progress-gain").css("background-color", "#75CB7C");
                $("#progress-loss").css("background-color", "#CE8A8A");
            }
            else {
                $(".chamadas-header").css("background-color", "#1db9c5");
                $("#progress-gain").css("background-color", "#1db9c5");
                $("#progress-loss").css("background-color", "#1db9c5");
            }

            ws.onopen = function (msg) {
                //console.log("conectado Binance Trade. hora => " + new Date());
                //console.log(msg);
            }

            ws.onmessage = function (obj) {
                var marketvalue = parseFloat(JSON.parse(obj.data).data.p).toFixed(8);
                if (marketvalue > _entrada) {
                    var maxvalue = _gain - _entrada;
                    var qtd_na_escala = marketvalue - _entrada;
                    var variacaopercentual = parseInt((qtd_na_escala / maxvalue) * 100);
                    $("#progress-gain").css("width", (variacaopercentual >= 100) ? "100%" : variacaopercentual + "%");
                    $("#progress-loss").css("width", 0);
                    if (changecolor) {
                        $(".chamadas-header").css("background-color", "#75CB7C");
                    }

                }
                else if (marketvalue < _entrada) {
                    var maxvalue = _entrada - _loss;
                    var qtd_na_escala = _entrada - marketvalue;
                    var variacaopercentual = parseInt((qtd_na_escala / maxvalue) * 100);
                    $("#progress-loss").css("width", (variacaopercentual >= 100) ? "100%" : variacaopercentual + "%");
                    $("#progress-gain").css("width", 0);
                    if (changecolor) {
                        $(".chamadas-header").css("background-color", "red");
                    }
                }
                else if (marketvalue == _entrada) {
                    $("#progress-gain").css("width", 0);
                    $("#progress-loss").css("width", 0);
                }
                $("#valor-mercado").text(marketvalue)
            }

            ws.onerror = function (error) {
                //console.log(error);        
            }

            ws.onclose = function (event) {

            }
        },

        disconnect: function () {
            if (ws != null) {
                ws.close(1000, "Closing Connection Normally");
            }
        },

        clearProgressBar: function () {
            $("#progress-gain").css("background-color", "#75CB7C");
            $("#progress-loss").css("background-color", "#CE8A8A");

            $("#progress-gain").css("width", 0);
            $("#progress-loss").css("width", 0);
        },

        currentState: function () {
            if (ws != null) {
                return ws.readyState;
            }
            else {
                return ws;
            }
        },

        startMonitor: function (symbol, entrada, gain, loss, Posicionado) {
            this.disconnect();
            this.clearProgressBar();
            this.connect(symbol, entrada, gain, loss, Posicionado);
        },

        showVendaMercado: function (precoMercado, entrada, gain, loss) {
            $("#progress-gain").css("background-color", "#75CB7C");
            $("#progress-loss").css("background-color", "#CE8A8A");
            $("#valor-mercado").text("Vendido a =>  " + precoMercado);

            if (precoMercado > entrada) {
                var maxvalue = gain - entrada;
                var qtd_na_escala = precoMercado - entrada;
                var variacaopercentual = parseInt((qtd_na_escala / maxvalue) * 100);
                $("#progress-gain").css("width", (variacaopercentual >= 100) ? "100%" : variacaopercentual + "%");
                $("#progress-loss").css("width", 0);
                $(".chamadas-header").css("background-color", "#75CB7C");
            }
            else if (precoMercado < entrada) {
                var maxvalue = entrada - loss;
                var qtd_na_escala = entrada - precoMercado;
                var variacaopercentual = parseInt((qtd_na_escala / maxvalue) * 100);
                $("#progress-loss").css("width", (variacaopercentual >= 100) ? "100%" : variacaopercentual + "%");
                $("#progress-gain").css("width", 0);
                $(".chamadas-header").css("background-color", "red");
            }
            else if (precoMercado == entrada) {
                $("#progress-gain").css("width", 0);
                $("#progress-loss").css("width", 0);
            }
        },

        updaChangeColor: function (value) {
            if (value == false) {
                $(".chamadas-header").css("background-color", "#1db9c5");
            }
            changecolor = value;
        },

        GainRealizado: function () {
            $("#progress-gain").css("width", "100%");
            $("#progress-loss").css("width", 0);
            $("#progress-gain").css("background-color", "#75CB7C");
            $(".chamadas-header").css("background-color", "#75CB7C");
            $("#valor-mercado").text("");
        },

        LossRealizado: function () {
            $("#progress-gain").css("width", 0);
            $("#progress-loss").css("width", "100%");
            $("#progress-loss").css("background-color", "#CE8A8A");
            $(".chamadas-header").css("background-color", "red");
            $("#valor-mercado").text("");
        }
    }
})()