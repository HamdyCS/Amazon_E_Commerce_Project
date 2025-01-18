using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Dtos
{
    public class CardInfoDto
    {
        [Required,CreditCard]
        public string CardNumber {  get; set; }

        public int ExpireYear { get; set; }

        [Required, Range(1, 12, ErrorMessage = "ExpireMonth Must be between 1 and 12.")]
        public int ExpireMonth { get; set; }

        [Required]
        public string Cvc { get; set; }
    }
}
