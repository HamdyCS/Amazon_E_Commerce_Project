using BusinessLayer.Dtos;
using DataAccessLayer.Enums;

namespace BusinessLayer.Contracks
{
    public interface IPaymentService
    {
        Task<decimal> GetTotalPriceAsync(PaymentDto paymentDto, string UserId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paymentPrePaidDto"></param>
        /// <param name="UserId"></param>
        /// <returns>Session url</returns>
        Task<PrePaidResultDto> PaymentPrePaidAsync(PaymentPrePaidDto paymentPrePaidDto, string UserId);// عن طريق فيزا
        Task<CashOnDeliveryResultDto> PaymentCashOnDeliveryAsync(PaymentCashOnDeliveryDto paymentCashOnDeliveryDto, string UserId);// عند الاستلام

        Task<bool> UpdatePaymentStatusByIdAsync(long paymentId, EnPaymentStatus enPaymentStatus);

        Task<bool> UpdatePaymentStatusAndPaymentIntentIdByIdAsync(long paymentId, EnPaymentStatus enPaymentStatus, string invoiceId,long shoppingCartId);

        Task<bool> UpdateRefundStatusAsync(long paymentId, EnRefundStatus enRefundStatus);
        Task<PaymentDto> GetPaymentByApplicationOrderIdAndUserIdAsync(long applicationOrderId, string userId);

        Task<PaymentDto> GetBySessionIdAndUserIdAsync(string sessionId, string userId);
    }
}
