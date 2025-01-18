using BusinessLayer.Contracks;
using BusinessLayer.Dtos;
using BusinessLayer.Exceptions;
using Microsoft.Extensions.Logging;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Servicese
{
    public class StripeService : IStripeService
    {
        private readonly ILogger<StripeService> _logger;

        public StripeService(ILogger<StripeService> logger)
        {
            this._logger = logger;
        }
        public async Task<bool> CreateStripeChargeAsync(CardInfoDto cardInfoDto, string tokenId, long amount,string currency
            ,string Description)
        {
            ParamaterException.CheckIfObjectIfNotNull(cardInfoDto, nameof(cardInfoDto));
            ParamaterException.CheckIfLongIsBiggerThanZero(amount, nameof(amount));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(currency, nameof(currency));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(Description, nameof(Description));

            try
            {

                var chargeOptions = new ChargeCreateOptions
                {
                    Amount = amount,
                    Currency = currency,
                    Description = Description,
                    Source = tokenId// ال token هو ال source
                };

                var chargeService = new ChargeService();
                var charge = await chargeService.CreateAsync(chargeOptions);

                return charge is null ? false : true;

            }
            catch (Exception ex)
            {
                _logger.LogError("Cannot create stripe charge,eroor = {Error}", ex.Message);

                return false;
            }
        }

        public async Task<Token> CreateStripeTokenAsync(CardInfoDto cardInfoDto)
        {
            ParamaterException.CheckIfObjectIfNotNull(cardInfoDto, nameof(cardInfoDto));

            try
            {
                //var tokenOptions = new TokenCreateOptions
                //{
                //    Card = new TokenCardOptions()
                //    {
                //        Number = cardInfoDto.CardNumber,
                //        ExpYear =cardInfoDto.ExpireYear.ToString(),
                //        ExpMonth = cardInfoDto.ExpireMonth.ToString(),
                //        Cvc = cardInfoDto.Cvc,
                //    }
                //};

                var tokenService = new Stripe.TokenService();
                var token = await tokenService.GetAsync("tok_visa");

                return token;
            }
            catch (Exception ex)
            {
                _logger.LogError("Cannot create stripe token,eroor = {Error}", ex.Message);
                return null;
            }
        }
    }
}
