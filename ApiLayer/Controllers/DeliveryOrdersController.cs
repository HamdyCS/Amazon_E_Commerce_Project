using ApiLayer.Help;
using BusinessLayer.Contracks;
using BusinessLayer.Dtos;
using BusinessLayer.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace ApiLayer.Controllers
{

    [Route("api/applications/application-order/delivery-orders")]
    [ApiController]
    [Authorize(Roles = Role.DeliveryAgent)]
    [EnableRateLimiting("FixedWindowPolicyByUserIpAddress")]

    public class DeliveryOrdersController : ControllerBase
    {
        public IDeliveryOrderService _DeliveryOrderService { get; }

        public DeliveryOrdersController(IDeliveryOrderService deliveryOrderService)
        {
            _DeliveryOrderService = deliveryOrderService;
        }

        [HttpGet("need-to-delivery", Name = "GetDeliveryOrdersNeedsDelivery")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]

        public async Task<ActionResult<DeliveryOrderDto>> GetDeliveryOrdersNeedsDelivery()
        {
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


        [HttpGet("deliveried", Name = "GetDeliveryOrdersThatDeliveried")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]

        public async Task<ActionResult<DeliveryOrderDto>> GetDeliveryOrdersThatDeliveried()
        {
            try
            {
                var UserId = Helper.GetIdFromClaimsPrincipal(User);
                if (UserId is null) return Unauthorized();

                var deliveryOrdersDtosList = await _DeliveryOrderService.GetDeliveryOrdersThatDeliveriedByDeliveryIdAsync(UserId);

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
