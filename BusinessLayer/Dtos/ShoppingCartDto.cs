using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Dtos
{
    public class ShoppingCartDto 
    {
        [Required]
        public long Id { get; set; }
        public List<SellerProductInShoppingCartDto> ProductsInShoppingCartsDtosList { get; set; }
    }
}
