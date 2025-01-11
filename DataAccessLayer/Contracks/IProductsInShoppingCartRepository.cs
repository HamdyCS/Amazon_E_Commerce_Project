using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Contracks
{
    public interface IProductsInShoppingCartRepository : IGenericRepository<ProductInShoppingCart>
    {
        Task<IEnumerable<ProductInShoppingCart>> GetAllProductsInShoppingCartByShoppingCartIdAsync(long shoppingCartId);

        Task<ProductInShoppingCart> GetByIdAndShoppingCartIdAndUserIdAsync(long Id,long shoppingCartId, string userId);

    }
}
