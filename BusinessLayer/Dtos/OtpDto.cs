using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Dtos
{
    public class OtpDto
    {
        public long Id { get; set; }

        [Required]
        public string Code { get; set; }

        [Required,EmailAddress]
        public string Email { get; set; }
    }
}
