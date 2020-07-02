using DataAccess.Serialized_Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Helpers
{
    public class ApiBinanceErrorMsgs
    {
        public static BinanceErrors GetError(BinanceErrors error)
        {          
            error.motivo = ServerError(error);
            return error;
        }

        private static string ServerError(BinanceErrors error)
        {
            switch (error.code)
            {
                case -1000:
                    return "Erro Desconhecido na APi.";
                case -1001:
                    return "O servidor se desconectou.";
                case -1002:
                    return "Autorização negada para este recurso.";
                case -1003:
                    if (error.msg.Contains("banned"))
                    {
                        return "Seu Ip esta banido por exeço de requisições.";
                    }
                    return "Exeço de requisições exedida.";
                case -1006:
                    return "Uma resposta inesperada foi recebida pelo servidor.";
                case -1007:
                    return "Tempo de requisição excedido.";
                case -1014:
                    return "Combinação de ordem não suportada.";
                case -1015:
                    return "Número de Ordem por segundo foi exedida.";
                case -1016:
                    return "Este Serviço foi desativado.";
                case -1020:
                    return "Operaçao nao Suportada.";
                case -1021:
                    return "Horario do Servidor não esta sincronizado.";
                case -1022:
                    return "Assinatura de solicitação inválida.";
                case -1010:
                    return "Erro nao tratado código 1010";
                case -1100:
                    return "Parametro com Caracteres Ilegais encontrados.";
                case -1101:
                    return "Muitos Parâmetros Enviados.";
                case -1102:
                    return "Parâmetro Nullo ou Ausente";
                case -1103:
                    return "Parâmetro Desconhecido enviado.";
                case -1104:
                    return "Não foi possivel Ler o parâmetro enviado.";
                case -1105:
                    return "O parâmetro estava vazio.";
                case -1106:
                    return "Parametro desnecessário enviado.";
                case -1111:
                    return "Precisão acima do permitido para este symbol.";
                case -1112:
                    return "Book de ofertas em falta.";
                case -1114:
                    return "Parâmetro TimeInForce enviado quando não necessário.";
                case -1115:
                    return "TimeInforce Inválido.";
                case -1116:
                    return "Tipo de Ordem Inválida.";
                case -1117:
                    return "Side Invalido.";
                case -1118:
                    return "newClientOrderId estava vazio.";
                case -1119:
                    return "origClientOrderId estava vazio.";
                case -1120:
                    return "Intervalo Inválido.";
                case -1121:
                    return "Symbol Inválido.";
                case -1125:
                    return "Listen Key Inválida.";
                case -1127:
                    return "Intervalo de Tempo Inválido.";
                case -1128:
                    return "Combinação de Parâmetros Inválidos.";
                case -1130:
                    return "Data Inválida.";
                case -2010:
                    return "Ordem Rejeitada.";
                case -2011:
                    return Translate2011code(error);
                case -2013:
                    return "Ordem Inexistente.";
                case -2014:
                    return "Chave API inválida.";
                case -2015:
                    return "Api Kei, Ip ou Permissão bloqueados.";
                case -2016:
                    return "Nenhuma Janela encontrada para este symbol.";
                default:
                    return ErrorByMsg(error);
            }
        }

        private static string ErrorByMsg(BinanceErrors error)
        {
            switch (error.msg)
            {
                case "Filter failure: PRICE_FILTER":
                    return "Preço da Ordem Muito alto ou muito baixo.";

                case "Filter failure: PERCENT_PRICE":
                    return "Preço muito alto ou muito baixo sobre a media nos ultimos minutos";

                case "Filter failure: LOT_SIZE":
                    return "Quantidade muito alta ou muito baixa ou não acopanha o step size";

                case "Filter failure: MIN_NOTIONAL":
                    return "quantidade é muito baixo.";

                case "Filter failure: ICEBERG_PARTS":
                    return "Iceberg irá quebrar em muitos pedaços e a quantidade é muito pequena.";

                case "Filter failure: MARKET_LOT_SIZE":
                    return "Quantidade a mercado muito alta ou muito baixa ou não acopanha o step size";

                case "Filter failure: MAX_NUM_ORDERS":
                    return "Muitas Ordens Abertas para este symbol";

                case "Filter failure: MAX_ALGO_ORDERS":
                    return "Muitas Ordens Stoloss e Take Profit abertas para este symbol";

                case "Filter failure: MAX_NUM_ICEBERG_ORDERS":
                    return "Muitos Icebertg ordems abertas para este symbol";

                case "Filter failure: EXCHANGE_MAX_NUM_ORDERS":
                    return "Muitas Ordens abertas para esta conta";

                case "Filter failure: EXCHANGE_MAX_ALGO_ORDERS":
                    return "A conta possui muitas perdas em abert ou recebe ordens de lucro na bolsa";

                case "Unknown order sent.":
                    return "Ordem Desconhecida. orderId, clOrdId, origClOrdId não encontrado.";

                case "Duplicate order sent.":
                    return "Ordem Duplicada,clOrdId  já utilizado.";

                case "Account has insufficient balance for requested action.":
                    return "Fundos Insuficientes";

                case "Market orders are not supported for this symbol.":
                    return "Ordem a Mercado não permitidas para este symbol";

                case "Iceberg orders are not supported for this symbol.":
                    return "icebergQty nao permitidas para este symbol";

                case "Stop loss orders are not supported for this symbol.":
                    return "Stop Loss nao suportado para este symbol";

                case "Stop loss limit orders are not supported for this symbol.":
                    return "Stop Loss Limit nao suportado para este symbol";

                case "Take profit orders are not supported for this symbol":
                    return "Take Profit nao suportado para este symbol";

                case "Take profit limit orders are not supported for this symbol.":
                    return "Take Profit Limit nao suportado para este symbol";

                case "Price * QTY is zero or less":
                    return "Valor da ordem muito pequeno";

                case "IcebergQty exceeds QTY.":
                    return "icebergQty  precisa ser menor que a quantidade.";

                case "This action disabled is on this account.":
                    return "Ação Esta desabilitada para esta conta, contate o administrador.";

                case "Unsupported order combination.":
                    return "orderType, timeInForce, stopPrice, ou icebergQty combinaçao nao permitida.";

                case "Order would trigger immediately.":
                    return "Valor de Entrada Ultrapassado.";

                case "Cancel order is invalid. Check origClOrdId and orderId.":
                    return "origClOrdId or orderId não enviados.";

                case "Order would immediately match and take.":
                    return "Ordem do tipo de pedido LIMIT_MAKER seria negociada imediatamente.";

                case "The relationship of the prices for the orders is not correct.":
                    return "Regras para envio de Ordem Oco Inválidas";

                case "OCO orders are not supported for this symbol":
                    return "Ordems OCO nao liberadas para este symbol.";

                default:
                    return "Erro API Binance Contate a Administração.";
            }
        }

        private static string Translate2011code(BinanceErrors error)
        {
            switch (error.msg)
            {
                case "Unknown order list sent.":
                    return "Ordem Desconhecida.";                   

                default:
                    return "Erro API Binance Contate a Administração.";
            }
        }
    }
}