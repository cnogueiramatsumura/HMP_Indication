using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Usuario.Models.Register
{
    public class RegisterViewModel
    {
        [Required]
        public string email { get; set; }
        [Required]
        public string nome { get; set; }
        [Required]        
        public string sobrenome { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string password { get; set; }
        [Display(Name = "Confirm Password")]
        [Compare("password",ErrorMessage = "Confirmação de Password Incorreta")]
        [DataType(DataType.Password)]
        public string confirmPassword { get; set; }
    }
}