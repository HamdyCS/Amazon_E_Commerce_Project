using BusinessLayer.Contracks;
using BusinessLayer.Dtos;
using BusinessLayer.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace ApiLayer.Controllers
{
    [Route("api/payment-types")]
    [ApiController]
    [EnableRateLimiting("FixedWindowPolicyByUserIpAddress")]

    public class PaymentTypesController : ControllerBase
    {
        private readonly IPaymentTypeService _paymentTypeService;

        public PaymentTypesController(IPaymentTypeService paymentTypeService)
        {
            this._paymentTypeService = paymentTypeService;
        }

        [HttpGet("{PaymentTypeId}", Name = "GetPaymentTypesById")]
        [Authorize(Roles = Role.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<PaymentTypeDto>> GetPaymentTypesById(int PaymentTypeId)
        {
            if (PaymentTypeId < 1) return BadRequest("PaymentTypeId must be bigger than 0");

            try
            {

                var paymentTypeDto = await _paymentTypeService.FindByIdAsync(PaymentTypeId);

                if (paymentTypeDto == null) return NotFound($"Not found payment type. Id = {PaymentTypeId}");

                return Ok(paymentTypeDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("all", Name = "GetAllPaymentsTypes")]
        [Authorize(Roles = Role.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<PaymentTypeDto>> GetAllPaymentsTypes()
        {
            try
            {

                var paymentsTypesDtosList = await _paymentTypeService.GetAllAsync();

                if (paymentsTypesDtosList == null || !paymentsTypesDtosList.Any()) return NotFound($"Not found any payment type.");

                return Ok(paymentsTypesDtosList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPut("{PaymentTypeId}", Name = "UpdatePaymentTypeById")]
        [Authorize(Roles = Role.Admin)]
        [ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> UpdatePaymentTypeById([FromRoute] long PaymentTypeId, [FromBody] PaymentTypeDto paymentTypeDto)
        {
            if (PaymentTypeId < 1) return BadRequest("PaymentTypeId must be bigger than zero.");


            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {

                var IsPaymentTypeUpdated = await _paymentTypeService.UpdateByIdAsync(PaymentTypeId, paymentTypeDto);

                if (!IsPaymentTypeUpdated) return BadRequest("Cannot Update payment type.");

                return Ok("Updated payment type successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


    }
}