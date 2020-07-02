using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DataAccess.ViewModels
{
    public class AceitarEdicaoViewModel
    {
        [Required]
        public int EdicaoId { get; set; }
        [Required]
        public int ChamadaId { get; set; }
    }
}