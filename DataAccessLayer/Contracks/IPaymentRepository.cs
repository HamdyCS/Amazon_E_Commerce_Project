using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Contracks
{
    public interface IPaymentRepository : IGenericRepository<Payment>
    {
        Task<Payment> FindByApplicationOrderIdAndUserIdAsync(long applicationOrderId, string userId);
        Task<Payment> GetByIdAndInvoiceIdAsync(long paymentId, string invoiceId);
        Task<Payment> GetBySessionIdAndUserIdAsync(string sessionId, string userId);
        Task<Payment> GetByShoppingCartIdAndUserIdAsync(long shoppingCartId, string userId);
    }
}
