using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.Dtos
{
    public class CreateBrandDto
    {
        [Required]
        public string NameEn { get; set; }

        [Required]
        public string NameAr { get; set; }

        [Required]
        public IFormFile Image { get; set; }
    }
}
