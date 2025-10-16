using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Contracks
{
    public interface ISellerProductInShoppingCartRepository : IGenericRepository<SellerProductInShoppingCart>
    {
        Task<IEnumerable<SellerProductInShoppingCart>> GetAllSellerProductsInShoppingCartByShoppingCartIdAsync(long shoppingCartId);

        Task<SellerProductInShoppingCart> GetByIdAndShoppingCartIdAndUserIdAsync(long Id,long shoppingCartId, string userId);

    }
}
