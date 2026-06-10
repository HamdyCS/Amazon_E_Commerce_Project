using BusinessLayer.Dtos;
using Stripe;

namespace BusinessLayer.Contracks
{
    public interface IStripeService
    {
        Task<StripeDto> CreateSessionAsync(CreateSessionDto createSessionDto);

        Task<Refund> CreateRefundAsync(string paymentIntentId,long paymentId ,decimal amount);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="json">request body</param>
        /// <returns></returns>
        Event GetStripeEvent(string json, string signature);
    }
}
