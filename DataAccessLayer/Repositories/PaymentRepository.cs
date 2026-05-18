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
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<PaymentRepository> _logger;

        public PaymentRepository(AppDbContext context, ILogger<PaymentRepository> logger) : base(context, logger, "Payments")
        {
            this._context = context;
            this._logger = logger;
        }

        public async Task<Payment> FindByApplicationOrderIdAndUserIdAsync(long applicationOrderId, string userId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(applicationOrderId, nameof(applicationOrderId));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(userId, nameof(userId));

            try
            {
                var payment = await _context.Payments
                    .FirstOrDefaultAsync(p => p.ApplicationOrders.
                    Any(ao => ao.Id == applicationOrderId && ao.CreatedBy == userId));

                return payment;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }

        public async Task<Payment> GetByIdAndInvoiceIdAsync(long paymentId, string invoiceId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(paymentId, nameof(paymentId));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(invoiceId, nameof(invoiceId));

            try
            {
                var payment = await _context.Payments.FirstOrDefaultAsync(p => p.Id == paymentId && p.InvoiceId == invoiceId);
                return payment;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }

        }
    }
}
