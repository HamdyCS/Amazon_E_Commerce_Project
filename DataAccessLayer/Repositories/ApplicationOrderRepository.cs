using DataAccessLayer.Contracks;
using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using DataAccessLayer.Enums;
using DataAccessLayer.Exceptions;
using DataAccessLayer.Identity.Entities;
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

        public async Task<ApplicationOrder> GetActiveApplicationOrderByApplicationIdAndUserIdAsync(long ApplicationId,string UserId)
        {
           ParamaterException.CheckIfLongIsBiggerThanZero(ApplicationId, nameof(ApplicationId));
           ParamaterException.CheckIfStringIsNotNullOrEmpty(UserId, nameof(UserId));

            try
            {
                var applicationOrders = await _context.ApplicationOrders.
                    Where(x=>x.ApplicationId == ApplicationId
                    &&x.CreatedBy==UserId).OrderByDescending(x=>x.ApplicationOrderTypeId).ToListAsync();

                if (applicationOrders is null || !applicationOrders.Any()) return null;

                return applicationOrders[0];

            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }

        }

        public async Task<ApplicationOrder> GetActiveApplicationOrderByApplicationIdAsync(long applicationId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(applicationId, nameof(applicationId));

            try
            {
                var applicationOrders = await _context.ApplicationOrders.
                    Where(x => x.ApplicationId == applicationId
                   ).OrderByDescending(x => x.ApplicationOrderTypeId).ToListAsync();

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
                    Where(x => x.ApplicationOrderTypeId == (long)EnApplicationOrderType.Delivered).ToListAsync();

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
                    Where(x => x.ApplicationOrderTypeId == (long)EnApplicationOrderType.Shipped&&!
                    _context.ApplicationOrders.Any(y=>(y.ApplicationId==x.ApplicationId&&
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
                    Where(x => x.ApplicationOrderTypeId == (long)EnApplicationOrderType.UnderProcessing && !
                    _context.ApplicationOrders.Any(y => (y.ApplicationId == x.ApplicationId &&
                    y.ApplicationOrderTypeId == (long)EnApplicationOrderType.Shipped))).ToListAsync();


                return ActiveUnderProcessingApplicationOrdersList;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }

        public async Task<IEnumerable<ApplicationOrder>> GetAllApplicationOrdersByApplicatonIdAndUserIdAsync(long ApplicationId, string UserId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(ApplicationId, nameof(ApplicationId));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(UserId, nameof(UserId));

            try
            {
                var applicationOrders = await _context.ApplicationOrders.
                    Where(x => x.ApplicationId == ApplicationId && 
                    x.CreatedBy==UserId).OrderBy(x => x.ApplicationOrderTypeId).ToListAsync();

                return applicationOrders;

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
                    Where(x => x.ApplicationId == ApplicationId).OrderBy(x=>x.ApplicationOrderTypeId).ToListAsync();
     
                return applicationOrders;

            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }
    }
}