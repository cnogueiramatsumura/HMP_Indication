﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Analista.Models
{
    public class LoginViewModel
    {
        [Required]     
        public string email { get; set; } 
        [Required]
        public string password { get; set; }
    }
}