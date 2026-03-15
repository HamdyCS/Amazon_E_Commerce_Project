using DataAccessLayer.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Dtos
{
    public class UpdateUserDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }


        [Required, CustomValidation(typeof(PersonValidtion), "DateOfBirthValidtion")]
        public DateTime DateOfBirth { get; set; }

        [Required, Phone]
        public string PhoneNumber { get; set; }
    }
}
