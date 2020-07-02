using DataAccess.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Analista.Models
{
    public class DashboardViewModel
    {
        public DateTime DataExpiracaoSSL { get; set; }   
        public ServerConfig Serverconfig { get; set; }
    }
}