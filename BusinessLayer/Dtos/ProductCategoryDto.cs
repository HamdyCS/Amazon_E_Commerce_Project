using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Dtos
{
    public class ProductCategoryDto
    {
        public long Id { get; set; }

        public string NameEn { get; set; }

        public string NameAr { get; set; }

        public string? DescriptionEn { get; set; }

        public string? DescriptionAr { get; set; }

        [Required]

        public List<byte[]> Images { get; set; } 

    }
}
