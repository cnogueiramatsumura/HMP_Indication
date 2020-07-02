using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class AlterarSenhaViewModel
    {
        [Required]
        [DisplayName("Senha Antiga")]
        public string SenhaAntiga { get; set; }
        [Required]
        [DisplayName("Senha Nova")]
        public string NovaSenha { get; set; }
        [Required]
        [Compare("NovaSenha", ErrorMessage = "Confirmação de Senha Inválida")]
        [DisplayName("Confirmar Senha")]
        public string ConfirmarSenha { get; set; }
    }
}
