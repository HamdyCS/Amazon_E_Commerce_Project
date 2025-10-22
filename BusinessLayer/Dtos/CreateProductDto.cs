using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.Dtos
{
    public class CreateProductDto
    {

        [Required]
        public string NameEn { get; set; }

        [Required]

        public string NameAr { get; set; }

        public string? DescriptionEn { get; set; }

        public string? DescriptionAr { get; set; }

        [Required]

        public string Size { get; set; }

        [Required]

        public string Color { get; set; }

        [Required]

        public decimal Height { get; set; }

        [Required]

        public decimal Length { get; set; }

        [Required]

        public long ProductSubCategoryId { get; set; }

        [Required]

        public long BrandId { get; set; }

        [Required]
        public List<IFormFile> Images { get; set; }

    }
}
