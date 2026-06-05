using ApiLayer.Filters;
using ApiLayer.Help;
using BusinessLayer.Contracks;
using BusinessLayer.Dtos;
using BusinessLayer.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace ApiLayer.Controllers
{

    [Route("api/shopping-carts/payments")]
    [ApiController]
    [Authorize(Roles = Role.Customer)]
    [EnableRateLimiting("FixedWindowPolicyByUserIpAddress")]

    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            this._paymentService = paymentService;
        }

        [HttpGet("total-price", Name = "GetTotalPriceOfShoppingCartPlusShippingPrice")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]

        public async Task<ActionResult<decimal>> GetTotalPriceOfShoppingCartPlusShippingPrice(PaymentDto paymentDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var UserId = Helper.GetIdFromClaimsPrincipal(User);
                if (UserId is null) return Unauthorized();

                var totalPrice = await _paymentService.GetTotalPriceAsync(paymentDto, UserId);

                if (totalPrice == -1) return BadRequest("Cannot get total price");

                return Ok(totalPrice);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("application-orders/{applicationOrderId:min(1)}", Name = "GetPaymentByApplicationOrderId")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PaymentDto>> GetPaymentByApplicationOrderId(long applicationOrderId)
        {

            var userId = Helper.GetIdFromClaimsPrincipal(User);
            if (userId is null) return Unauthorized();

            var payment = await _paymentService.GetPaymentByApplicationOrderIdAndUserIdAsync(applicationOrderId, userId);
            if (payment == null) return NotFound($"Not found any payment for application order id = {applicationOrderId}");

            return Ok(payment);

        }


        [HttpPost("pre-paid", Name = "PaymentPrePaid")]
        [Idempotency]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<string>> PaymentPrePaid(PaymentPrePaidDto paymentPrePaidDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);


            var userId = Helper.GetIdFromClaimsPrincipal(User);
            if (userId is null) return Unauthorized();

            var prePaidResultDto = await _paymentService.PaymentPrePaidAsync(paymentPrePaidDto, userId);

            if (prePaidResultDto == null)
                return BadRequest("Payment did not complete successfully.");

            return Ok(prePaidResultDto);

        }


        [HttpPost("cash-on-delivery", Name = "PaymentCashOnDelivery")]
        [Idempotency]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<string>> PaymentCashOnDelivery(PaymentCashOnDeliveryDto paymentCashOnDeliveryDto)
        {


            var userId = Helper.GetIdFromClaimsPrincipal(User);
            if (userId is null) return Unauthorized();

            var cashOnDeliveryResultDto = await _paymentService.PaymentCashOnDeliveryAsync(paymentCashOnDeliveryDto, userId);

            if (cashOnDeliveryResultDto == null)
                return BadRequest("Payment didnot complet Successfully.");

            return Ok(cashOnDeliveryResultDto);

        }


        [HttpGet("session/{sessionId}", Name = "GetPaymentBySessionId")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PaymentDto>> GetPaymentBySessionId(string sessionId)
        {

            var userId = Helper.GetIdFromClaimsPrincipal(User);
            if (userId is null) return Unauthorized();

            var payment = await _paymentService.GetBySessionIdAndUserIdAsync(sessionId, userId);
            if (payment == null) return NotFound($"Not found any payment for session id = {sessionId}");

            return Ok(payment);

        }
    }
}
