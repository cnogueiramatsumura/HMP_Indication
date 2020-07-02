using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DataAccess.ViewModels.Analista
{
    public class UpdateSmtpViewModel
    {
        [Required]
        [DisplayName("SMTP")]
        public string SmtpAdress { get; set; }
        [Required]
        [DisplayName("Nº Porta")]
        public int SmtpPort { get; set; }
        [Required]
        [EmailAddress]
        [DisplayName("Email")]
        public string SmtpUsername { get; set; }
        [Required]
        [DisplayName("Password")]
        [DataType(DataType.Password)]
        public string SmtpPassword { get; set; }
    }
}