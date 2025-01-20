using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Dtos
{
    public class SellerProductInShoppingCartDto
    {

        public long Id { get; set; }

        [Required,Range(1,double.MaxValue,ErrorMessage = "Number must be bigger than 0")]
        public int Number { get; set; }

        [Required, Range(1, double.MaxValue, ErrorMessage = "SellerProductId must be bigger than 0")]

        public long SellerProductId { get; set; }

        [Required, Range(1, double.MaxValue, ErrorMessage = "ShoppingCartId must be bigger than 0")]

        public long ShoppingCartId { get; set; }
    }
}
