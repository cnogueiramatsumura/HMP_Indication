using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Analista.Models
{
    public class UpdateLicencaViewModel
    {
        [Required]
        [DisplayName("Preço da Licença")]   
        [DataType(DataType.Currency)]
        public decimal PrecoLicenca { get; set; }
    }
}