using BusinessLayer.Dtos;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Contracks
{
    public interface IStripeService
    {
        Task<Token> CreateStripeTokenAsync(CardInfoDto cardInfoDto);
        Task<bool> CreateStripeChargeAsync(CardInfoDto cardInfoDto, string tokenId, long amount, string currency
            , string Description);
    }
}
