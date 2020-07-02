using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entidades
{
    public class ServerConfig
    {
        [JsonIgnore]
        public int Id { get; set; }
        [JsonIgnore]
        public string BitpayToken { get; set; }
        [JsonIgnore]
        public string BitpayIdentity { get; set; }
        [JsonIgnore]
        public string PagseguroToken { get; set; }
        public string OneSignalAppId { get; set; }
        [JsonIgnore]
        public string OneSignalToken { get; set; }
        public string ApiServer { get; set; }
        public string AppServer { get; set; }
        public decimal PrecoLicenca { get; set; }
        [JsonIgnore]
        public string SmtpAdress { get; set; }
        [JsonIgnore]
        public Nullable<int> SmtpPort { get; set; }
        [JsonIgnore]
        public string SmtpUsername { get; set; }
        [JsonIgnore]
        public string SmtpPassword { get; set; }
    }
}
