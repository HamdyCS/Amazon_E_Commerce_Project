using BusinessLayer.Contracks;
using BusinessLayer.Dtos;
using BusinessLayer.Exceptions;
using BusinessLayer.Options;
using DataAccessLayer.UnitOfWork.Contracks;
using Microsoft.Extensions.Logging;
using Stripe;

using Stripe.Checkout;

namespace BusinessLayer.Servicese
{
    public class StripeService : IStripeService
    {
        private readonly ILogger<StripeService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly StripeOptions _stripeOptions;

        public StripeService(ILogger<StripeService> logger, IUnitOfWork unitOfWork, StripeOptions stripeOptions)
        {
            this._logger = logger;
            _unitOfWork = unitOfWork;
            _stripeOptions = stripeOptions;
        }

        public async Task<StripeDto> CreateSessionAsync(CreateSessionDto createSessionDto)
        {
            ParamaterException.CheckIfObjectIfNotNull(createSessionDto, nameof(createSessionDto));
            try
            {
                //get products in shopping cart
                var SellerProductsInCart = await _unitOfWork.SellerProductsInShoppingCartRepository.GetAllSellerProductsInShoppingCartByShoppingCartIdAsync(createSessionDto.paymentDto.ShoppingCartId);
                if (!SellerProductsInCart.Any())
                {
                    throw new InvalidOperationException("No products found in the shopping cart.");
                }

                //create line items
                var LineItems = new List<SessionLineItemOptions>();
                foreach (var item in SellerProductsInCart)
                {
                    var lineItem = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(item.SellerProduct.Price * 100), // Amount in cents
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.SellerProduct.Product.NameEn,
                            },
                        },
                        Quantity = item.Number,
                    };
                    LineItems.Add(lineItem);
                }

                // Add shipping cost as a separate line item if applicable
                var shippingLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(createSessionDto.ShippingCost * 100),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Shipping Cost"
                        }
                    },
                    Quantity = 1
                };
                LineItems.Add(shippingLineItem);


                //create stripe options
                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string> { "card" },
                    LineItems = LineItems,
                    Mode = "payment",
                    SuccessUrl = createSessionDto.SuccessUrl,
                    CancelUrl = createSessionDto.CancelUrl,
                    Metadata = new Dictionary<string, string>
                    {
                        { "PaymentId", createSessionDto.PaymentId.ToString() },
                        { "ShoppingCartId", createSessionDto.paymentDto.ShoppingCartId.ToString() }
                    }
                };

                //create session
                var service = new SessionService();
                Session session = await service.CreateAsync(options);

                if (session == null)
                    throw new Exception("Failed to create Stripe session.");

                return new StripeDto { SessionId = session.Id, SessionUrl = session.Url };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while creating Stripe session. message: {ex.Message}");
                throw;
            }
        }

        public Event GetStripeEvent(string json, string signature)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(json, nameof(json));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(signature, nameof(signature));

            try
            {
                Event stripeEvent = Stripe.EventUtility.ConstructEvent(
                    json,
                    signature,
                    _stripeOptions.WebHookSecret,
                     throwOnApiVersionMismatch: false
                    );

                return stripeEvent;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while verifying Stripe signature. message: {ex.Message}");
                throw;
            }
        }


    }
}
