using ApiLayer.Help;
using BusinessLayer.Contracks;
using DataAccessLayer.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;


namespace ApiLayer.Controllers
{
    [Route("api/stripe")]
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
        /* public async Task<IActionResult> HandleStripeWebhook()
         {
             //get payload
             string json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
             if (string.IsNullOrEmpty(json)) return BadRequest("Invalid payload");

             try
             {
                 var StripeEvent = _stripeService.GetStripeEvent(json, Request.Headers["Stripe-Signature"]);
                 if (StripeEvent is null) return BadRequest("Invalid event");//unauthorized

                 // Handle the event

                 //payment succeeded
                 if (StripeEvent.Type == "payment_intent.succeeded")
                 {
                     var paymentIntent = StripeEvent.Data.Object as PaymentIntent;
                     if (paymentIntent is null) return BadRequest("Invalid paymentIntent data");

                     if (string.IsNullOrEmpty(paymentIntent.Id))
                         return BadRequest("PaymentIntentId is null or empty");

                     // get paymentId from metadata
                     var paymentId = long.TryParse(paymentIntent.Metadata["PaymentId"], out var parsedPaymentId) ? parsedPaymentId : 0;
                     //get shopping cart id from metadata
                     var shoppingCartId = long.TryParse(paymentIntent.Metadata["ShoppingCartId"], out var parsedShoppingCartId) ? parsedShoppingCartId : 0;

                     if (shoppingCartId <= 0 || paymentId <= 0)
                         return BadRequest("Invalid shopping cart id or payment id");

                     //update payment status to Succeeded
                     var IsPaymentUpdated = await _paymentService.UpdatePaymentStatusAndInvoiceIdByIdAsync(paymentId, EnPaymentStatus.Succeeded, paymentIntent.Id, shoppingCartId);

                     if (!IsPaymentUpdated)
                         throw new Exception("Failed to update payment.");

                     return Ok();

                 }

                 //payment failed
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
         }*/
        public async Task<IActionResult> HandleStripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            if (string.IsNullOrWhiteSpace(json))
                return BadRequest("Empty payload");

            try
            {
                var stripeEvent = _stripeService.GetStripeEvent(json, Request.Headers["Stripe-Signature"]);

                if (stripeEvent == null)
                    return BadRequest("Invalid signature");

                switch (stripeEvent.Type)
                {
                    // =========================
                    // PAYMENT SUCCESS
                    // =========================
                    case "payment_intent.succeeded":
                        await HandlePaymentSucceeded(stripeEvent);
                        break;

                    // =========================
                    // PAYMENT FAILED
                    // =========================
                    case "payment_intent.payment_failed":
                        await HandlePaymentFailed(stripeEvent);
                        break;

                    // =========================
                    // CHECKOUT COMPLETED
                    // =========================
                    case "checkout.session.completed":
                        await HandleCheckoutCompleted(stripeEvent);
                        break;

                    // =========================
                    // SESSION EXPIRED
                    // =========================
                    case "checkout.session.expired":
                        await HandleSessionExpired(stripeEvent);
                        break;

                    // =========================
                    // Refund Updated
                    // =========================
                    case "refund.updated":
                        await HandelRefundUpdated(stripeEvent);
                        break;

                    default:
                        return Ok(); // ignore unhandled events
                }

                return Ok();
            }
            catch (StripeException ex)
            {
                return BadRequest($"Stripe error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server error: {ex.Message}");
            }
        }

        private async Task HandlePaymentSucceeded(Event stripeEvent)
        {
            var paymentIntent = stripeEvent.Data.Object as PaymentIntent;

            if (paymentIntent == null || string.IsNullOrEmpty(paymentIntent.Id))
                throw new Exception("Invalid PaymentIntent");

            var metadata = paymentIntent.Metadata;

            var paymentId = Helper.GetMetadataLong(metadata, "PaymentId");
            var cartId = Helper.GetMetadataLong(metadata, "ShoppingCartId");

            if (paymentId <= 0 || cartId <= 0)
                throw new Exception("Invalid metadata");

            await _paymentService.UpdatePaymentStatusAndPaymentIntentIdByIdAsync(
                paymentId,
                EnPaymentStatus.Succeeded,
                paymentIntent.Id,
                cartId
            );
        }

        private async Task HandlePaymentFailed(Event stripeEvent)
        {
            var paymentIntent = stripeEvent.Data.Object as PaymentIntent;

            if (paymentIntent == null)
                throw new Exception("Invalid PaymentIntent");

            var paymentId = Helper.GetMetadataLong(paymentIntent.Metadata, "PaymentId");
            var cartId = Helper.GetMetadataLong(paymentIntent.Metadata, "ShoppingCartId");

            if (paymentId <= 0)
                throw new Exception("Invalid PaymentId");

            await _paymentService.UpdatePaymentStatusAndPaymentIntentIdByIdAsync(
                paymentId,
                EnPaymentStatus.Failed,
                paymentIntent.Id,
                cartId
            );
        }

        private async Task HandleCheckoutCompleted(Event stripeEvent)
        {
            var session = stripeEvent.Data.Object as Session;

            if (session == null)
                throw new Exception("Invalid Session");

            var paymentIntentId = session.PaymentIntentId;

            var paymentId = Helper.GetMetadataLong(session.Metadata, "PaymentId");
            var cartId = Helper.GetMetadataLong(session.Metadata, "ShoppingCartId");

            if (paymentId <= 0 || cartId <= 0)
                return;
        }

        private async Task HandleSessionExpired(Event stripeEvent)
        {
            var session = stripeEvent.Data.Object as Session;

            if (session == null)
                return;

            var paymentId = Helper.GetMetadataLong(session.Metadata, "PaymentId");
            var cartId = Helper.GetMetadataLong(session.Metadata, "ShoppingCartId");

            if (paymentId <= 0)
                return;

            await _paymentService.UpdatePaymentStatusAndPaymentIntentIdByIdAsync(
                paymentId,
                EnPaymentStatus.Failed,
                session.PaymentIntentId,
                cartId
            );
        }

        private async Task HandelRefundUpdated(Event stripeEvent)
        {
            var refund = stripeEvent.Data.Object as Refund;
            if (refund == null) return;


            var paymentId = Helper.GetMetadataLong(refund.Metadata, "PaymentId");
            if (paymentId <= 0) return;

            if (refund.Status == "succeeded")
            {
                await _paymentService.UpdateRefundStatusAsync(paymentId, EnRefundStatus.Succeeded);
            }

            if (refund.Status == "failed")
            {
                await _paymentService.UpdateRefundStatusAsync(paymentId, EnRefundStatus.Failed);
            }
        }
    }
}
