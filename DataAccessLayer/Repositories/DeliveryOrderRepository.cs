using DataAccessLayer.Contracks;
using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using DataAccessLayer.Enums;
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
    public class DeliveryOrderRepository : IDeliveryOrderRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<DeliveryOrderRepository> _logger;

        public DeliveryOrderRepository(AppDbContext context,ILogger<DeliveryOrderRepository> logger)
        {
            this._context = context;
            this._logger = logger;
        }

        private Exception _HandleDatabaseException(Exception ex)
        {
            _logger.LogError(ex, "Database error occurred while getting data from database. Error: {ErrorMessage}", ex.Message);
            return new Exception($"Database error occurred while getting data from database. Error: {ex.Message}");
        }

        public async Task<IEnumerable<DeliveryOrder>> GetDeliveryOrdersNeedsDeliveryByDeliveryIdAsync(string deliveryId)
        {
           ParamaterException.CheckIfStringIsNotNullOrEmpty(deliveryId,nameof(deliveryId));

            try
            {
                var DeliveryOrdersList = await _context.ApplicationOrders
                    .Where(x => x.DeliveryId == deliveryId && x.ApplicationOrderTypeId == (long)EnApplicationOrderType.Shipped
                    && !_context.ApplicationOrders
                    .Any(y => y.ApplicationId == x.ApplicationId &&
                    y.ApplicationOrderTypeId == (long)EnApplicationOrderType.Delivered)).Select(z => new DeliveryOrder()
                    {
                        CityId = z.Payment.UserAddress.CityId,
                        CityNameAr = z.Payment.UserAddress.City.NameEn,
                        Address = z.Payment.UserAddress.Address,
                        TotalPrice = z.Payment.TotalPrice,
                        ApplicationId = z.ApplicationId
                    }).ToListAsync();


               return DeliveryOrdersList;
            }
            catch (Exception ex)
            {
                throw _HandleDatabaseException(ex);
            }
        }

        public async Task<IEnumerable<DeliveryOrder>> GetDeliveryOrdersThatDeliveriedByDeliveryIdAsync(string deliveryId)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(deliveryId, nameof(deliveryId));

            try
            {
                var DeliveryOrdersList = await _context.ApplicationOrders
                    .Where(x => x.DeliveryId == deliveryId && x.ApplicationOrderTypeId == (long)EnApplicationOrderType.Delivered)
                   .Select(z => new DeliveryOrder()
                    {
                        CityId = z.Payment.UserAddress.CityId,
                        CityNameAr = z.Payment.UserAddress.City.NameAr,
                        Address = z.Payment.UserAddress.Address,
                        TotalPrice = z.Payment.TotalPrice,
                        ApplicationId = z.ApplicationId
                    }).ToListAsync();


                return DeliveryOrdersList;
            }
            catch (Exception ex)
            {
                throw _HandleDatabaseException(ex);
            }
        }
    }
}
