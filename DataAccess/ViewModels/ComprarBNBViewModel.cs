using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class ComprarBNBViewModel
    {
        [Required(ErrorMessage = "O campo qtd é obrigatorio")]
        public decimal qtd { get; set; }
    }
}
