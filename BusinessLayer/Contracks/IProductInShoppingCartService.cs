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
        Task<Dtos.ProductInShoppingCartDto> FindByIdAsync(long productInShoppingCartId);

        Task<IEnumerable<Dtos.ProductInShoppingCartDto>> GetAllProductsInShoppingCartByShoppingCartIdAsync(long shoppingCartId);

        Task<Dtos.ProductInShoppingCartDto> AddAsync(ProductInShoppingCartDto productInShoppingCartDto,long ShoppingCartId, string UserId);

        Task<IEnumerable<Dtos.ProductInShoppingCartDto>> AddRangeAsync(IEnumerable<Dtos.ProductInShoppingCartDto> productsInShoppingCartsDtoList, long ShoppingCartId, string UserId);

        Task<bool> UpdateAsync(long ProductInShoppingCartId, ProductInShoppingCartDto productInShoppingCartDto , long ShoppingCartId, string UserId);

        Task<bool> DeleteAsync(long productInShopingCartId, long ShoppingCartId, string UserId);

        Task<bool> DeleteRangeAsync(IEnumerable< long> productsInShopingCartIdsList, long ShoppingCartId, string UserId);

    }
}
