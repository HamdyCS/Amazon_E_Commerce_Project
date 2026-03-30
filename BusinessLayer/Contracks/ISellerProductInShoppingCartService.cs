using BusinessLayer.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Contracks
{
    public interface ISellerProductInShoppingCartService
    {
        Task<SellerProductInShoppingCartDto> FindByIdAndShoppingCartIdAndUserIdAsync(long productInShoppingCartId, long ShoppingCartId,string UserId);

        Task<IEnumerable<SellerProductInShoppingCartDto>> GetAllSellerProductsInShoppingCartByShoppingCartIdAsync(long shoppingCartId);

        Task<ShoppingCartDto> AddAsync(AddSellerProductToShoppingCartDto productInShoppingCartDto,long ShoppingCartId, string UserId);

        Task<ShoppingCartDto> AddRangeAsync(IEnumerable<AddSellerProductToShoppingCartDto> productsInShoppingCartsDtoList, long ShoppingCartId, string UserId);

        Task<ShoppingCartDto> UpdateAsync(long ProductInShoppingCartId, AddSellerProductToShoppingCartDto productInShoppingCartDto , string UserId);

        Task<ShoppingCartDto> DeleteAsync(DeleteProductFromShoppingCartDto deleteProductFromShoppingCartDto, string UserId);

        Task<ShoppingCartDto> DeleteRangeAsync(IEnumerable<DeleteProductFromShoppingCartDto> deleteProductFromShoppingCartDtos, string UserId);

    }
}
