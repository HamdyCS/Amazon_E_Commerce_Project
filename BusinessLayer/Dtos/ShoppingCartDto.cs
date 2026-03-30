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
        public long Id { get; set; }

        public string UserId { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<SellerProductInShoppingCartDto> SellerProducts { get; set; }
    }
}
