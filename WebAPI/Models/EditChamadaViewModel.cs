using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebAPI.CustomDataAnnotation;

namespace WebAPI.Models
{
    public class EditChamadasViewModel
    {
        [Required]
        public int Chamada_Id { get; set; }
        public string symbol { get; set; }

        public decimal Gain { get; set; }
        public decimal Entrada { get; set; }
        public decimal Loss { get; set; }
        [Required]
        [DisplayName("Gain")]
        [ValidateGainEdit]
        public decimal NewGain { get; set; }
        [Required]
        [DisplayName("Loss")]
        [ValidateLossEdit]
        public decimal NewLoss { get; set; }
    }
}