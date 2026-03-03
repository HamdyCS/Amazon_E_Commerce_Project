using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Dtos
{
    public class ResetPasswordDto
    {
        [Required]
        public string Password { get; set; }

        [Required,EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Otp { get; set; }
    }
    
}
