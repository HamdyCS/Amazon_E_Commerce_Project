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
    public class ShippingCostRepository : GenericRepository<ShippingCost>, IShippingCostRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ShippingCostRepository> _logger;

        public ShippingCostRepository(AppDbContext context, ILogger<ShippingCostRepository> logger) : base(context, logger, "ShippingCost")
        {
            this._context = context;
            this._logger = logger;
        }

        public async Task<ShippingCost> GetByCityIdAsync(long cityId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(cityId, nameof(cityId));

            try
            {
                var shippingCost = await _context.ShippingCosts.FirstOrDefaultAsync(e => e.CityId == cityId);
                return shippingCost;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }

        public async Task<decimal> GetPriceOfShippingCostByCityIdAsync(long cityId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(cityId, nameof(cityId));

            try
            {
                ShippingCost shippingCost = await _context.ShippingCosts.FirstOrDefaultAsync(e => e.CityId == cityId);

                return shippingCost is null ? -1 : shippingCost.Price;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }
    }
}
