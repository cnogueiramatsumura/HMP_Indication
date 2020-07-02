using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.CustomDataAnnotations
{
    public class ValidadeEntrada : ValidationAttribute
    {       
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {   
            var partialproperty = validationContext.ObjectType.GetProperty("ValorMercado");
            var partialValue = (decimal)partialproperty.GetValue(validationContext.ObjectInstance, null);

            if (value == null)
                return new ValidationResult(ErrorMessage);

            if ((decimal)value == partialValue)
            {
                ErrorMessage = "O preço de Entrada não pode ser igual ao valor do mercado";
                return new ValidationResult(ErrorMessage);
            }
            return ValidationResult.Success;
        }
    }
}