using BusinessLayer.Contracks;
using DataAccessLayer.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiLayer.Controllers
{
    [Route("api/Stripe")]
    [ApiController]
    public class StripeController : ControllerBase
    {
        private readonly IStripeService _stripeService;
        private readonly IPaymentService _paymentService;

        public StripeController(IStripeService stripeService, IPaymentService paymentService)
        {
            _stripeService = stripeService;
            _paymentService = paymentService;
        }


        [AllowAnonymous]
        [HttpPost(Name = "HandleStripeWebhook")]
        public async Task<IActionResult> HandleStripeWebhook()
        {
            //get payload
            string json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            if (string.IsNullOrEmpty(json)) return BadRequest("Invalid payload");

            try
            {
                var StripeEvent = _stripeService.GetStripeEvent(json, Request.Headers["Stripe-Signature"]);
                if (StripeEvent is null) return BadRequest("Invalid event");//unauthorized

                // Handle the event

                //session completed
                if (StripeEvent.Type == "checkout.session.completed")
                {
                    var session = StripeEvent.Data.Object as Session;
                    if (session is null) return BadRequest("Invalid session data");

                    if (string.IsNullOrEmpty(session.PaymentIntentId))
                        return BadRequest("PaymentIntentId is null or empty");

                    // get paymentId from metadata
                    var paymentId = long.TryParse(session.Metadata["PaymentId"], out var parsedPaymentId) ? parsedPaymentId : 0;

                    //get shopping cart id from metadata
                    var shoppingCartId = long.TryParse(session.Metadata["ShoppingCartId"], out var parsedShoppingCartId) ? parsedShoppingCartId : 0;

                    if (shoppingCartId <= 0 || paymentId <= 0)
                        return BadRequest("Invalid shopping cart id or payment id");

                    //update payment status to Succeeded
                    var IsPaymentUpdated = await _paymentService.UpdatePaymentStatusAndInvoiceIdByIdAsync(paymentId, EnPaymentStatus.Succeeded, session.PaymentIntentId, shoppingCartId);

                    if (!IsPaymentUpdated)
                        throw new Exception("Failed to update payment.");

                    return Ok();

                }

                //session expired
                if (StripeEvent.Type == "checkout.session.expired")
                {
                    var session = StripeEvent.Data.Object as Session;
                    if (session is null) return BadRequest("Invalid session data");

                    if (string.IsNullOrEmpty(session.PaymentIntentId))
                        return BadRequest("PaymentIntentId is null or empty");

                    // get paymentId from metadata
                    var paymentId = long.TryParse(session.Metadata["PaymentId"], out var parsedPaymentId) ? parsedPaymentId : 0;

                    if (paymentId <= 0)
                        return BadRequest("Invalid payment id");

                    //get shopping cart id from metadata
                    var shoppingCartId = long.TryParse(session.Metadata["ShoppingCartId"], out var parsedShoppingCartId)
                        ? parsedShoppingCartId : 0;


                    //update payment status to Succeeded
                    var IsPaymentUpdated = await _paymentService.UpdatePaymentStatusAndInvoiceIdByIdAsync(paymentId, EnPaymentStatus.Failed, session.PaymentIntentId, shoppingCartId);

                    if (!IsPaymentUpdated)
                        throw new Exception("Failed to update payment.");

                    return Ok();

                }

                //failed
                if (StripeEvent.Type == "payment_intent.payment_failed")
                {
                    var paymentIntent = StripeEvent.Data.Object as PaymentIntent;
                    if (paymentIntent is null) return BadRequest("Invalid paymentIntent data");

                    if (string.IsNullOrEmpty(paymentIntent.Id))
                        return BadRequest("PaymentIntentId is null or empty");

                    // get paymentId from metadata
                    var paymentId = long.TryParse(paymentIntent.Metadata["PaymentId"], out var parsedPaymentId) ? parsedPaymentId : 0;

                    if (paymentId <= 0)
                        return BadRequest("Invalid payment id");

                    //get shopping cart id from metadata
                    var shoppingCartId = long.TryParse(paymentIntent.Metadata["ShoppingCartId"], out var parsedShoppingCartId)
                        ? parsedShoppingCartId : 0;


                    //update payment status to Succeeded
                    var IsPaymentUpdated = await _paymentService.UpdatePaymentStatusAndInvoiceIdByIdAsync(paymentId, EnPaymentStatus.Failed, paymentIntent.Id, shoppingCartId);
                    if (!IsPaymentUpdated)
                        throw new Exception("Failed to update payment.");

                    return Ok();

                }

                return BadRequest("Unhandled event type");
            }
            catch (StripeException stripeEx)
            {
                return BadRequest($"Stripe error: {stripeEx.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error processing webhook: {ex.Message}");
            }
        }

    }
}
