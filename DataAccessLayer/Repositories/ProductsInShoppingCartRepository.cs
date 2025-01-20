using DataAccessLayer.Contracks;
using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using DataAccessLayer.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class ProductsInShoppingCartRepository : GenericRepository<SellerProductInShoppingCart>, IProductsInShoppingCartRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ProductsInShoppingCartRepository> _logger;

        public ProductsInShoppingCartRepository(AppDbContext context, ILogger<ProductsInShoppingCartRepository> logger) : base(context, logger, "SellerProductsInShoppingCarts")
        {
            this._context = context;
            this._logger = logger;
        }

        public async Task<IEnumerable<SellerProductInShoppingCart>> GetAllSellerProductsInShoppingCartByShoppingCartIdAsync(long shoppingCartId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(shoppingCartId, nameof(shoppingCartId));

            try
            {
                var shoppingCart = await _context.ShoppingCarts.Include(e=>e.SellerProductsInShoppingCart)
                    .FirstOrDefaultAsync(e=>e.Id == shoppingCartId);

                return shoppingCart is null ? null : shoppingCart.SellerProductsInShoppingCart;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }

        public async Task<SellerProductInShoppingCart> GetByIdAndShoppingCartIdAndUserIdAsync(long Id, long shoppingCartId, string userId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(Id, nameof(Id));
            ParamaterException.CheckIfLongIsBiggerThanZero(shoppingCartId, nameof(shoppingCartId));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(userId, nameof(userId));

            try
            {
                var productInShoppinCart = await _context.SellerProductsInShoppingCarts.Include(e => e.ShoppingCart).FirstOrDefaultAsync(e =>
                e.Id == Id && e.ShoppingCartId == shoppingCartId && e.ShoppingCart != null && e.ShoppingCart.Id == shoppingCartId);

                return productInShoppinCart;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }
    }
}
