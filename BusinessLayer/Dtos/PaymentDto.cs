using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Dtos
{
    public class PaymentDto
    {
        public long Id { get; set; }

        public decimal TotalPrice { get; set; }

        [Required,Range(1,double.MaxValue,ErrorMessage = "UserAddressId must be bigger than zero")]
        public long UserAddressId { get; set; }


        [Required, Range(1, double.MaxValue, ErrorMessage = "ShoppingCartId must be bigger than zero")]
        public long ShoppingCartId { get; set; }

    }
}
