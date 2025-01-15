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
    public class ApplicationOrderRepository : GenericRepository<ApplicationOrder>, IApplicationOrderRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ApplicationOrderRepository> _logger;

        public ApplicationOrderRepository(AppDbContext context, ILogger<ApplicationOrderRepository> logger) : base(context, logger, "ApplicationOrders")
        {
            this._context = context;
            this._logger = logger;
        }

        public async Task<ApplicationOrder> GetActiveApplicationOrderByApplicationIdAsync(long ApplicationId)
        {
           ParamaterException.CheckIfLongIsBiggerThanZero(ApplicationId, nameof(ApplicationId));

            try
            {
                var applicationOrders = await _context.ApplicationOrders.
                    Where(x=>x.ApplicationId == ApplicationId).OrderByDescending(x=>x.ApplicationOrderTypeId).ToListAsync();

                if (applicationOrders is null || !applicationOrders.Any()) return null;

                return applicationOrders[0];

            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }

        }

        public async Task<IEnumerable<ApplicationOrder>> GetActiveDeliveredApplicationOrdersAsync()
        {
            try
            {
                var ActiveDeliveredApplicationOrdersList = await _context.ApplicationOrders.
                    Where(x => x.Id == (long)EnApplicationOrderType.Delivered).ToListAsync();

                return ActiveDeliveredApplicationOrdersList;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }

        public async Task<IEnumerable<ApplicationOrder>> GetActiveShippedApplicationOrdersAsync()
        {
            try
            {
                var ActiveShippingApplicationOrdersList = await _context.ApplicationOrders.
                    Where(x => x.Id == (long)EnApplicationOrderType.Shipped&&!
                    _context.ApplicationOrders.Any(y=>(y.Id==x.Id&&
                    y.ApplicationOrderTypeId== (long)EnApplicationOrderType.Delivered ))).ToListAsync();

                

                return ActiveShippingApplicationOrdersList;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }

        public async Task<IEnumerable<ApplicationOrder>> GetActiveUnderProcessingApplicationOrdersAsync()
        {
            try
            {
                var ActiveUnderProcessingApplicationOrdersList = await _context.ApplicationOrders.
                    Where(x => x.Id == (long)EnApplicationOrderType.UnderProcessing && !
                    _context.ApplicationOrders.Any(y => (y.Id == x.Id &&
                    y.ApplicationOrderTypeId == (long)EnApplicationOrderType.Shipped))).ToListAsync();


                return ActiveUnderProcessingApplicationOrdersList;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }

        public async Task<IEnumerable<ApplicationOrder>> GetAllApplicationOrdersByApplicatonIdAsync(long ApplicationId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(ApplicationId, nameof(ApplicationId));

            try
            {
                var applicationOrders = await _context.ApplicationOrders.
                    Where(x => x.ApplicationId == ApplicationId).ToListAsync();
     
                return applicationOrders;

            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }
    }
}