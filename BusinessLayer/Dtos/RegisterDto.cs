using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Dtos
{
    public class RegisterDto
    {
        [Required]
        public string Otp { get; set; }

        [Required]
        public UserDto userDto { get; set; }
    }
}
