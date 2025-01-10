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
    public class ProductsInShoppingCartRepository : GenericRepository<ProductInShoppingCart>, IProductsInShoppingCartRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ProductsInShoppingCartRepository> _logger;

        public ProductsInShoppingCartRepository(AppDbContext context, ILogger<ProductsInShoppingCartRepository> logger) : base(context, logger, "ProductsInShoppingCarts")
        {
            this._context = context;
            this._logger = logger;
        }

        public async Task<IEnumerable<ProductInShoppingCart>> GetAllProductsInShoppingCartByShoppingCartIdAsync(long shoppingCartId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(shoppingCartId, nameof(shoppingCartId));

            try
            {
                var productsInShoppingCart = await _context.ShoppingCarts.Include(e=>e.ProductsInShoppingCarts)
                    .FirstOrDefaultAsync(e=>e.Id == shoppingCartId);

                return productsInShoppingCart is null ? null : productsInShoppingCart.ProductsInShoppingCarts;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }

        public async Task<ProductInShoppingCart> GetByIdAndShoppingCartIdAndUserIdAsync(long Id, long shoppingCartId, string userId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(Id, nameof(Id));
            ParamaterException.CheckIfLongIsBiggerThanZero(shoppingCartId, nameof(shoppingCartId));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(userId, nameof(userId));

            try
            {
                var productInShoppinCart = await _context.ProductsInShoppingCarts.Include(e => e.ShoppingCart).FirstOrDefaultAsync(e =>
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
