using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;

namespace WebApp.CustomDataAnnotations
{
    public class ValidateGain : ValidationAttribute
    {    
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var partialproperty = validationContext.ObjectType.GetProperty("PrecoEntrada");
            var partialValue = (decimal)partialproperty.GetValue(validationContext.ObjectInstance, null);
           
            if (value == null)
                return new ValidationResult(ErrorMessage);
                                          
            if ((decimal)value < partialValue)
            {
                ErrorMessage = "O Valor do Gain precisa ser maior que Preço Entrada";
                return new ValidationResult(ErrorMessage);
            }
            return ValidationResult.Success;
        }
    }
}