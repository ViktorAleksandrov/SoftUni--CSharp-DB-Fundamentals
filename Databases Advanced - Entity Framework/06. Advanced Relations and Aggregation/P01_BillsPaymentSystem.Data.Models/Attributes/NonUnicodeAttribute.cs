using System.ComponentModel.DataAnnotations;

namespace P01_BillsPaymentSystem.Data.Models.Attributes
{
    public class NonUnicodeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string nullErrorMessage = "Value can not be null";

            if (value == null)
            {
                return new ValidationResult(nullErrorMessage);
            }

            string text = (string)value;

            string unicodeErrorMessage = "Value can not contain unicode characters";

            for (int index = 0; index < text.Length; index++)
            {
                if (text[index] > 255)
                {
                    return new ValidationResult(unicodeErrorMessage);
                }
            }

            return ValidationResult.Success;
        }
    }
}
