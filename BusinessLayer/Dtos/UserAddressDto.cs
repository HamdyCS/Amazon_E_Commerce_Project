using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Dtos
{
    public class UserAddressDto
    {
        public long Id { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public long CityId { get; set; }
    }
}
