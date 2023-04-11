using System.ComponentModel.DataAnnotations;

namespace Cemsa_BackEnd.Validations
{
    public class PositiveORNull : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is null)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("El campo es nulo o mayor e igual a 18");
        }
    }

}
