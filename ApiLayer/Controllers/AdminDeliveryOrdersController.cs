using ApiLayer.Help;
using BusinessLayer.Contracks;
using BusinessLayer.Dtos;
using BusinessLayer.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace ApiLayer.Controllers
{

    [Route("api/admin/applications/application-order/delivery-orders")]
    [ApiController]
    [Authorize(Roles =Role.Admin)]
    [EnableRateLimiting("FixedWindowPolicyByUserIpAddress")]

    public class AdminDeliveryOrdersController : ControllerBase
    {
        public IDeliveryOrderService _DeliveryOrderService { get; }

        public AdminDeliveryOrdersController(IDeliveryOrderService deliveryOrderService)
        {
            _DeliveryOrderService = deliveryOrderService;
        }

        [HttpGet("need-to-delivery", Name = "GetDeliveryOrdersNeedsDeliveryByDeliveryId")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]

        public async Task<ActionResult<DeliveryOrderDto>> GetDeliveryOrdersNeedsDeliveryByDeliveryId([FromBody] string DeliveryId)
        {
            if (string.IsNullOrEmpty(DeliveryId)) return BadRequest("DeliveryId cannot be null or empty");

            try
            {
                var UserId = Helper.GetIdFromClaimsPrincipal(User);
                if (UserId is null) return Unauthorized();

                var deliveryOrdersDtosList = await _DeliveryOrderService.GetDeliveryOrdersNeedsDeliveryByDeliveryIdAsync(UserId);

                if (deliveryOrdersDtosList is null || !deliveryOrdersDtosList.Any()) return NotFound("Didnot find any delivery order.");

                return Ok(deliveryOrdersDtosList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("deliveried", Name = "GetDeliveryOrdersThatDeliveriedByDeliveryId")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]

        public async Task<ActionResult<DeliveryOrderDto>> GetDeliveryOrdersThatDeliveriedByDeliveryId([FromBody] string DeliveryId)
        {
            if (string.IsNullOrEmpty(DeliveryId)) return BadRequest("DeliveryId cannot be null or empty");
            try
            {

                var deliveryOrdersDtosList = await _DeliveryOrderService.GetDeliveryOrdersThatDeliveriedByDeliveryIdAsync(DeliveryId);

                if (deliveryOrdersDtosList is null || !deliveryOrdersDtosList.Any()) return NotFound("Didnot find any delivery order.");

                return Ok(deliveryOrdersDtosList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



    }
}
