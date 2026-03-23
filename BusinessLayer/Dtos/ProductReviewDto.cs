using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Dtos
{
    public class ProductReviewDto
    {
        public long Id { get; set; }

        [Required,Range(1,5)]
        public int NumberOfStars { get; set; }

        [Required,MinLength(1,ErrorMessage = "Cannot message be empty")]
        public string Message { get; set; }

        public DateTime CreatedAt { get; set; }

        [Required, MinLength(1, ErrorMessage = "Cannot Name be empty")]

        public string Name { get; set; }

        [Required,Range(1,5,ErrorMessage = "SellerProductId must be bigger than zero")]
        public long ProductId { get; set; }
    }
}
