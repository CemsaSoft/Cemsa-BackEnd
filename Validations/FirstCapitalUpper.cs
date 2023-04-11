using System.ComponentModel.DataAnnotations;

namespace Cemsa_BackEnd.Validations
{
    public class FirstCapitalUpper : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;

            }

            var primeraLetra = value.ToString()[0].ToString();

            if (primeraLetra != primeraLetra.ToUpper())
            {
                return new ValidationResult("Primera letra debe ser mayúscula");
            }

            return ValidationResult.Success;
        }
    }
}