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
    public class ShoppingCartRepository : GenericRepository<ShoppingCart>, IShoppingCartRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ShoppingCartRepository> _logger;

        public ShoppingCartRepository(AppDbContext context, ILogger<ShoppingCartRepository> logger) : base(context, logger, "ShoppingCarts")
        {
            this._context = context;
            this._logger = logger;
        }

        public async Task<ShoppingCart> GetActiveShoppingCartByUserIdAsync(string userId)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(userId, nameof(userId));

            try
            {
                // ممكن نضيف userId in ApplicationOrdersTable
                // كدا كدا اليوزر اللي عامل ال application order
                // هو هو اللي عامل shopping cart
                //var applicationOrder = await _context.ApplicationOrders.Include(e => e.ShoppingCart).
                //    FirstOrDefaultAsync(e =>e.ShoppingCart != null && e.ShoppingCart.UserId == userId);

                //if (applicationOrder == null) return null;

                //return applicationOrder.ShoppingCart;

                var ActiveShoppingCart = await _context.ShoppingCarts.Include(e => e.ApplicationOrders)
                    .FirstOrDefaultAsync(e => !e.ApplicationOrders.Any());

                return ActiveShoppingCart;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }

        public async Task<IEnumerable<ShoppingCart>> GetAllUserShoppingCartByUserIdAsync(string userId)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(userId, nameof(userId));

            try
            {
                var shoppingCartsList = await _context.ShoppingCarts.Where(e => e.UserId == userId)
                    .ToListAsync();

                return shoppingCartsList;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }

        
        public async Task<decimal?> GetTotalPriceInShoppingCartByShoppingCartIdAsync(long shoppingCartId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(shoppingCartId, nameof(shoppingCartId));

            try
            {
                var TotalPrice = await _context.ProductsInShoppingCarts
                    .Where(e => e.ShoppingCartId == shoppingCartId).SumAsync(e => e.TotalPrice);
                

                return TotalPrice;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }
    }
}
