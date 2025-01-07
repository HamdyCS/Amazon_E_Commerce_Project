using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Dtos
{
    public class SellerProductDto
    {
        public long Id { get; set; }

        [Required,Range(0.01,double.MaxValue,ErrorMessage = "Price must be bigger than 0")]

        public decimal Price { get; set; }

        [Required, Range(1, double.MaxValue, ErrorMessage = "NumberInStock must be bigger than 0")]
        public int NumberInStock { get; set; }

        [Required]

        public long ProductId { get; set; }
    }
}
