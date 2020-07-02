using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAPI.Models
{
    public class RecuperarSenhaViewModel
    {
        [DisplayName("Email")]
        [Required(ErrorMessage = "O campo Email é obrigatorio")]
        [EmailAddress(ErrorMessage = "Email com formato inválido")]
        public string Email { get; set; }
    }
}