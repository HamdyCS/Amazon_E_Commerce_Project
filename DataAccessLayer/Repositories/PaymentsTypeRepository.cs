using DataAccessLayer.Contracks;
using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using DataAccessLayer.Enums;
using DataAccessLayer.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataAccessLayer.Repositories
{
    public class PaymentsTypeRepository : GenericRepository<PaymentsType>, IPaymentTypeRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<PaymentsTypeRepository> _logger;

        public PaymentsTypeRepository(AppDbContext context, ILogger<PaymentsTypeRepository> logger) : base(context, logger, "PaymentsTyps")
        {
            this._context = context;
            this._logger = logger;
        }

        public async Task<PaymentsType> GetByEnPaymentTypeAsync(EnPaymentType enPaymentType)
        {
            ParamaterException.CheckIfObjectIfNotNull(enPaymentType, nameof(enPaymentType));
            try
            {
                var paymentTypeId = (long)enPaymentType;

                var paymentType = await _context.PaymentsTypes.FirstOrDefaultAsync(e => e.Id == paymentTypeId);
                return paymentType;

            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }
    }
}
