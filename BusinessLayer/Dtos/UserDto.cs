using DataAccessLayer.Validitions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Dtos
{
    public class UserDto
    {
        [Required]
        public string ID { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }


        [Required, CustomValidation(typeof(PersonValidtion), "DateOfBirthValidtion")]
        public DateTime DateOfBirth { get; set; }

        [Required,Phone]
        public string PhoneNumber { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
