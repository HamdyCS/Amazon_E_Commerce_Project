using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.Dtos
{
    public class ConfirmEmailDto
    {
        [Required,EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Code { get; set; }
        
    }
}
