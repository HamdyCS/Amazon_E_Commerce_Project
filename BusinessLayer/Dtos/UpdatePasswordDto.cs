using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.Dtos
{
    public class UpdatePasswordDto
    {
        [Required]
        public string OldPassword { get; set; }

        [Required, MinLength(6), MaxLength(10)]
        public string NewPassword { get; set; }
    }


}
