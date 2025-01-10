using BusinessLayer.Dtos;
using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Contracks
{
    public interface IShoppingCartService
    {
        Task<ShoppingCartDto> FindActiveShoppingCartByUserIdAsync(string userId);

        Task<ShoppingCartDto> FindByIdAsync(long shoppingCartId);

        Task<ShoppingCartDto> AddNewShoppingCart(string UserId);

        Task<IEnumerable<ShoppingCartDto>> GetAllUserShoppingCartsByUserIdAsync(string userId);

        Task<decimal> GetTotalPriceInShoppingCartByShoppingCartIdAsync(long shoppingCartId);
    }
}
