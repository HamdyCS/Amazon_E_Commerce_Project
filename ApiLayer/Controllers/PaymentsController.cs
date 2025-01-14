using ApiLayer.Help;
using BusinessLayer.Contracks;
using BusinessLayer.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace ApiLayer.Controllers
{

    [Route("api/shopping-carts/payments")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService) 
        {
            this._paymentService = paymentService;
        }

        [HttpGet("total-price",Name ="GetTotalPriceOfShoppingCartPlusShippingPrice")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]

        public async Task<ActionResult<decimal>> GetTotalPriceOfShoppingCartPlusShippingPrice(PaymentDto paymentDto)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var UserId = Helper.GetIdFromClaimsPrincipal(User);
                if(UserId is null) return Unauthorized();

                var totalPrice = await _paymentService.GetTotalPriceAsync(paymentDto, UserId);

                if (totalPrice == -1) return BadRequest("Cannot get total price");

                return Ok(totalPrice);
            }
            catch (Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }

    }

}
