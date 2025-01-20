using BusinessLayer.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Contracks
{
    public interface IProductInShoppingCartService
    {
        Task<SellerProductInShoppingCartDto> FindByIdAndShoppingCartIdAndUserIdAsync(long productInShoppingCartId, long ShoppingCartId,string UserId);

        Task<IEnumerable<SellerProductInShoppingCartDto>> GetAllSellerProductsInShoppingCartByShoppingCartIdAsync(long shoppingCartId);

        Task<SellerProductInShoppingCartDto> AddAsync(SellerProductInShoppingCartDto productInShoppingCartDto,long ShoppingCartId, string UserId);

        Task<IEnumerable<SellerProductInShoppingCartDto>> AddRangeAsync(IEnumerable<SellerProductInShoppingCartDto> productsInShoppingCartsDtoList, long ShoppingCartId, string UserId);

        Task<bool> UpdateAsync(long ProductInShoppingCartId, SellerProductInShoppingCartDto productInShoppingCartDto , long ShoppingCartId, string UserId);

        Task<bool> DeleteAsync(long productInShopingCartId, long ShoppingCartId, string UserId);

        Task<bool> DeleteRangeAsync(IEnumerable< long> productsInShopingCartIdsList, long ShoppingCartId, string UserId);

    }
}
