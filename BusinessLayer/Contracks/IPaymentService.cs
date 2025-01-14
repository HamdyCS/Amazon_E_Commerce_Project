using BusinessLayer.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Contracks
{
    public interface IPaymentService
    {
        Task<decimal> GetTotalPriceAsync(PaymentDto paymentDto, string UserId);
    }
}
