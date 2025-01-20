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


        [HttpPost("pre-paid", Name = "PaymentPrePaid")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<string>> PaymentPrePaid(PaymentPrePaidDto paymentPrePaidDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var userId = Helper.GetIdFromClaimsPrincipal(User);
                if (userId is null) return Unauthorized();

                var IsPaymentCompletedSuccessfuly = await _paymentService.PaymentPrePaidAsync(paymentPrePaidDto,userId);

                if (!IsPaymentCompletedSuccessfuly)
                    return BadRequest("Payment didnot complet Successfully.");

                return Ok("Payment completed Successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }


        [HttpPost("cash-on-delivery", Name = "PaymentCashOnDelivery")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<string>> PaymentCashOnDelivery(PaymentDto paymentDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var userId = Helper.GetIdFromClaimsPrincipal(User);
                if (userId is null) return Unauthorized();

                var IsPaymentCompletedSuccessfuly = await _paymentService.PaymentCashOnDeliveryAsync(paymentDto, userId);

                if (!IsPaymentCompletedSuccessfuly)
                    return BadRequest("Payment didnot complet Successfully.");

                return Ok("Payment completed Successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }


    }
}
