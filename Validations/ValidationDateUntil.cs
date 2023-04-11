using System.ComponentModel.DataAnnotations;

namespace Cemsa_BackEnd.Validations
{
    public class ValidationDateUntil: ValidationAttribute
    {
        public override bool IsValid(object value)
    {
        DateTime d = Convert.ToDateTime(value);
        if (value != null)

        {
            return d >= DateTime.Now;
        }
        return true;
    }
}
}

