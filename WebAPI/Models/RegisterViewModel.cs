using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebAPI.CustomDataAnnotation;

namespace WebAPI.Models
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [CkeckEmail(ErrorMessage = "Esse email já esta cadastrado")]
        public string email { get; set; }
        [Required]
        public string nome { get; set; }
        [Required]
        public string sobrenome { get; set; }
        [Required]
        public string password { get; set; }
        [Display(Name = "Confirm Password")]
        [Compare("password", ErrorMessage = "Confirmação de Password Incorreta")]      
        public string confirmPassword { get; set; }
    }
}