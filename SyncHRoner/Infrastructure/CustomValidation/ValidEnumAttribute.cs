using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SyncHRoner.Infrastructure.CustomValidation
{
    public class ValidEnumAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return new ValidationResult($"Value of {value.GetType().Name} is required");

            if (value is IEnumerable)
            {
                if ((value as IEnumerable).GetEnumerator().MoveNext() == false)
                    return new ValidationResult($"At least one value is required");

                foreach (var item in (value as IEnumerable))
                {
                    Type t = item.GetType();
                    if (!Enum.IsDefined(t, item))
                        return new ValidationResult($"Value: {value.ToString()} is not valid");
                }

                return ValidationResult.Success;
            }

            bool isEnumDefined = Enum.IsDefined(value.GetType(), value);

            return isEnumDefined ? ValidationResult.Success :
                new ValidationResult($"Value: {value.ToString()} is not valid");
        }
    }
}
