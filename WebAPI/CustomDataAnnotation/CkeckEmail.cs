using DataAccess.Interfaces;
using DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebAPI.CustomDataAnnotation
{
    public class CkeckEmail : ValidationAttribute
    {
        public readonly IUsuarioRepository _UserRepo;
        public CkeckEmail()
        {
            _UserRepo = new UsuarioRepository();
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var partialproperty = validationContext.ObjectType.GetProperty("email");
            var partialValue = (string)partialproperty.GetValue(validationContext.ObjectInstance, null);

            var user = _UserRepo.GetByEmail(partialValue);

            if (user != null)
            {
                ErrorMessage = "Esse email já esta cadastrado";
                return new ValidationResult(ErrorMessage);
            }
            return ValidationResult.Success;
        }
    }
}