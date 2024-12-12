using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Dtos
{
    public class BrandDto
    {
        public long Id { get; set; }

        [Required]
        public string NameEn { get; set; }

        [Required]
        public string NameAr { get; set; }

        public string CreatedBy { get; set; }

        [Required]
        public byte[] Image { get; set; }

    }
}
