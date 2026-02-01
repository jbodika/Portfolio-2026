using System;
using System.ComponentModel.DataAnnotations;

namespace jbH60Store.Models
{
    public class PositiveDecimalAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is decimal decimalValue && decimalValue < 0)
            {
                return new ValidationResult("Value must be a positive number.");
            }

            return ValidationResult.Success;
        }
    }

    public class SellPriceGreaterOrEqualAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is decimal sellPrice && validationContext.ObjectInstance is Product product)
            {
                if (sellPrice < product.BuyPrice)
                {
                    return new ValidationResult("Sell price must be greater than or equal to buy price.");
                }
            }

            return ValidationResult.Success;
        }




        public class NegativePricesAttribute : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                if (value is decimal price && price < 0)
                {
                    return new ValidationResult("Prices cannot be negative.");
                }

                return ValidationResult.Success;
            }
        }
    }

    public class ValidDecimalAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
               
                return ValidationResult.Success; 
            }

            if (decimal.TryParse(value.ToString(), out decimal decimalValue))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(ErrorMessage ?? "The value is not a valid decimal.");
        }
    }
}
