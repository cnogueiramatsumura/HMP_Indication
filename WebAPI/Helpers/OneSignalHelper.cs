using DataAccess.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Helpers
{
    public class OneSignalHelper
    {    
        private readonly string appid;
        private readonly string ServerAdress;

        public OneSignalHelper(string apiID, string Domain)
        {
            appid = apiID;
            ServerAdress = Domain;
        }

        public object SenderObj(List<string> listclients, string symbol, NotificationType TipoNotificacao)
        {
            return new
            {
                app_id = appid,
                headings = new { en = headingMsg(symbol, TipoNotificacao) },
                contents = new { en = contentsMsg(symbol, TipoNotificacao) },
                include_player_ids = listclients,
                url = "",
                chrome_web_icon = ServerAdress + "/images/icons/OneSignalIcon.png",
                chrome_web_image = "",
                chrome_web_badge = ""
            };
        }

        private static string contentsMsg(string symbol, NotificationType TipoNotificacao)
        {
            switch (TipoNotificacao)
            {
                case NotificationType.Entrada:
                    return "Você recebeu uma Indicação para o symbol " + symbol;
                case NotificationType.CancelamentoEntrada:
                    return "Você recebeu uma Indicação para cancelar entrada do symbol " + symbol;
                case NotificationType.Edicao:
                    return "Você recebeu uma Indicação para editar o symbol " + symbol;
                case NotificationType.Gain:
                    return "Gain Realizado para o symbol " + symbol;
                case NotificationType.Loss:
                    return "Loss Realizado para o symbol " + symbol;
                default:
                    return "";
            }
        }

        private static string headingMsg(string symbol, NotificationType TipoNotificacao)
        {
            switch (TipoNotificacao)
            {
                case NotificationType.Entrada:
                    return "Chamada " + symbol;
                case NotificationType.CancelamentoEntrada:
                    return "Cancelamento " + symbol;
                case NotificationType.Edicao:
                    return "Edição " + symbol;
                case NotificationType.Gain:
                    return "Gain " + symbol;
                case NotificationType.Loss:
                    return "Loss " + symbol;
                default:
                    return "";
            }
        }
    }

    public enum NotificationType
    {
        Entrada = 1,
        CancelamentoEntrada = 2,
        Edicao = 3,
        Gain = 4,
        Loss = 5
    }
}