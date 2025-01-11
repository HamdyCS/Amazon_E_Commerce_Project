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
        Task<ProductInShoppingCartDto> FindByIdAndShoppingCartIdAndUserIdAsync(long productInShoppingCartId, long ShoppingCartId,string UserId);

        Task<IEnumerable<ProductInShoppingCartDto>> GetAllProductsInShoppingCartByShoppingCartIdAsync(long shoppingCartId);

        Task<ProductInShoppingCartDto> AddAsync(ProductInShoppingCartDto productInShoppingCartDto,long ShoppingCartId, string UserId);

        Task<IEnumerable<ProductInShoppingCartDto>> AddRangeAsync(IEnumerable<ProductInShoppingCartDto> productsInShoppingCartsDtoList, long ShoppingCartId, string UserId);

        Task<bool> UpdateAsync(long ProductInShoppingCartId, ProductInShoppingCartDto productInShoppingCartDto , long ShoppingCartId, string UserId);

        Task<bool> DeleteAsync(long productInShopingCartId, long ShoppingCartId, string UserId);

        Task<bool> DeleteRangeAsync(IEnumerable< long> productsInShopingCartIdsList, long ShoppingCartId, string UserId);

    }
}
