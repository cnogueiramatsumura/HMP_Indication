using DataAccess.Entidades;
using DataAccess.Repository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;


namespace WebAPI.Helpers
{
    public class OneSignalAPI
    {
        private readonly string _baseUri = "https://onesignal.com/api/v1/";
        private readonly string _token;
        private readonly string _NotifyDomain;
        private readonly string _AppId;

        public OneSignalAPI(string token, string Domain, string AppId)
        {
            _token = token;
            _NotifyDomain = Domain;
            _AppId = AppId;
        }

        public void NotificarWeb(List<Usuario> listUsuario, string Symbol, NotificationType tipoNotificacao)
        {
            var listclients = listUsuario.Where(x => x.OneSignalIDWeb != null).Select(x => x.OneSignalIDWeb).ToList();
            if (listclients.Count >= 1)
            {
                var helperOnesignal = new OneSignalHelper(_AppId, _NotifyDomain);
                var sendeobj = helperOnesignal.SenderObj(listclients, Symbol, tipoNotificacao);
                var param = JsonConvert.SerializeObject(sendeobj);
                var contentString = new StringContent(param, Encoding.UTF8, "application/json");
                //contentString.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                using (HttpClient _HttpClient = new HttpClient())
                {
                    _HttpClient.DefaultRequestHeaders.Add("ContentType", "application/json; charset=utf-8");
                    _HttpClient.DefaultRequestHeaders.Add("authorization", "Basic " + _token);
                    var resultado = _HttpClient.PostAsync(_baseUri + "notifications", contentString).Result;
                }
            }
        }

        public void NotificarApp(List<Usuario> listUsuario, string Symbol, NotificationType tipoNotificacao)
        {
            var listclients = listUsuario.Where(x => x.OneSignalIDApp != null).Select(x => x.OneSignalIDApp).ToList();
            if (listclients.Count >= 1)
            {
                var helperOnesignal = new OneSignalHelper(_AppId, _NotifyDomain);
                var sendeobj = helperOnesignal.SenderObj(listclients, Symbol, tipoNotificacao);
                var param = JsonConvert.SerializeObject(sendeobj);
                var contentString = new StringContent(param, Encoding.UTF8, "application/json");
                //contentString.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                using (HttpClient _HttpClient = new HttpClient())
                {
                    _HttpClient.DefaultRequestHeaders.Add("ContentType", "application/json; charset=utf-8");
                    _HttpClient.DefaultRequestHeaders.Add("authorization", "Basic " + _token);
                    var resultado = _HttpClient.PostAsync(_baseUri + "notifications", contentString).Result;
                }
            }
        }

        public void NotificarUsuario(Usuario User, string Symbol, NotificationType tipoNotificacao)
        {          
            if (User != null)
            {
                var ListIds = new List<string>();
                ListIds.Add(User.OneSignalIDApp);
                ListIds.Add(User.OneSignalIDWeb);

                var helperOnesignal = new OneSignalHelper(_AppId, _NotifyDomain);
                var sendeobj = helperOnesignal.SenderObj(ListIds, Symbol, tipoNotificacao);
                var param = JsonConvert.SerializeObject(sendeobj);
                var contentString = new StringContent(param, Encoding.UTF8, "application/json");
                //contentString.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                using (HttpClient _HttpClient = new HttpClient())
                {
                    _HttpClient.DefaultRequestHeaders.Add("ContentType", "application/json; charset=utf-8");
                    _HttpClient.DefaultRequestHeaders.Add("authorization", "Basic " + _token);
                    var resultado = _HttpClient.PostAsync(_baseUri + "notifications", contentString).Result;
                }
            }
        }
    }
}