using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Dtos
{
    public class DeleteProductFromShoppingCartDto
    {
        [Required, Range(1, long.MaxValue, ErrorMessage = "SellerProductInShoppingCartId must be greater than or equal 1")]
        public long SellerProductInShoppingCartId { get; set; }

        [Required,Range(1,long.MaxValue,ErrorMessage = "ShoppingCartId must be greater than or equal 1")]
        public long ShoppingCartId { get; set; }
    }
}
