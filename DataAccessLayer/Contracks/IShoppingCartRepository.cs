using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Contracks
{
    public interface IShoppingCartRepository : IGenericRepository<ShoppingCart>
    {
        Task<ShoppingCart> GetActiveShoppingCartByUserIdAsync(string userId);

        Task<IEnumerable<ShoppingCart>> GetAllUserShoppingCartByUserIdAsync(string userId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shoppingCartId"></param>
        /// <returns>return null if not found</returns>
        Task<decimal?> GetTotalPriceInShoppingCartByShoppingCartIdAsync(long shoppingCartId);
    }
}
