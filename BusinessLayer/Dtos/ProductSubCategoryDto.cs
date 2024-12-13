using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Dtos
{
    public class ProductSubCategoryDto
    {
        public long Id { get; set; }

        [Required]
        public string NameAr { get; set; }

        [Required]
        public string NameEn { get; set; }

        public string? DescriptionEn { get; set; }

        public string? DescriptionAr { get; set; }

        [Required]

        public byte[] Image { get; set; }

        [Required]
        public long ProductCategoryId { get; set; }
    }
}
