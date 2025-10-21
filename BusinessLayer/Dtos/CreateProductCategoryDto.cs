using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.Dtos
{
    public class CreateProductCategoryDto
    {
        public long Id { get; set; }

        [Required]
        public string NameEn { get; set; }

        [Required]
        public string NameAr { get; set; }

        public string? DescriptionEn { get; set; }

        public string? DescriptionAr { get; set; }

        [Required]
        public List<IFormFile> Images { get; set; }
    }
}
