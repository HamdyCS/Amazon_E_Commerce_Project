using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Validitions
{
    public class PersonValidtion
    {
        public static ValidationResult DateOfBirthValidtion(DateTime DateOfBirth,ValidationContext validationContext)
        {
            if ((DateTime.UtcNow.Year - DateOfBirth.Year) >= 18)
                return ValidationResult.Success;

            return new ValidationResult("Age must be greater than or equal to 18");

        }
    }
}
