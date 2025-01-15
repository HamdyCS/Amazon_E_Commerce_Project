using BusinessLayer.Contracks;
using BusinessLayer.Dtos;
using BusinessLayer.Roles;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiLayer.Controllers
{
    [Route("api/admin/applications")]
    [ApiController]
    [Authorize(Roles =Role.Admin)]
    public class AdminApplicationOrdersController : ControllerBase
    {
        private readonly IApplicationOrderService _applicationOrderService;

        public AdminApplicationOrdersController(IApplicationOrderService applicationOrderService)
        {
            this._applicationOrderService = applicationOrderService;
        }

        [HttpGet("application-orders/{ApplicationOrderId}", Name = "GetApplicationOrderById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ApplicationOrderDto>> GetApplicationOrderById(long ApplicationOrderId)
        {
            if (ApplicationOrderId < 1) return BadRequest("Id must be bigger than zero.");

            try
            {
                var applicationOrderDto = await _applicationOrderService.FindByIdAsync(ApplicationOrderId);

                if (applicationOrderDto == null) return NotFound($"Not found application order. Id = {ApplicationOrderId}");

                return Ok(applicationOrderDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



        [HttpGet("{ApplicationId}/application-orders", Name = "TrackApplicationOrderByApplicatonId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<ApplicationOrderDto>>> TrackApplicationOrdersByApplicatonId(long ApplicationId)
        {
            if (ApplicationId < 1) return BadRequest("ApplicationId must be bigger than zero.");

            try
            {
                var applicationOrderDtosList = await _applicationOrderService.TrackApplicationOrderByApplicatonIdAsync(ApplicationId);

                if (applicationOrderDtosList == null || !applicationOrderDtosList.Any() ) 
                    return NotFound($"Not found any application order. ApplicationId = {ApplicationId}");

                return Ok(applicationOrderDtosList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



        [HttpGet("application-orders/active-under-processing", Name = "GetActiveUnderProcessingApplicationOrder")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<ApplicationOrderDto>>> GetActiveUnderProcessingApplicationOrder()
        {
            
            try
            {
                var applicationOrderDtosList = await _applicationOrderService.GetActiveUnderProcessingApplicationOrdersAsync();

                if (applicationOrderDtosList == null || !applicationOrderDtosList.Any())
                    return NotFound($"Not found any active under processing application order.");

                return Ok(applicationOrderDtosList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



        [HttpGet("application-orders/active-shipping", Name = "GetActiveShippingApplicationOrder")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<ApplicationOrderDto>>> GetActiveShippingApplicationOrder()
        {

            try
            {
                var applicationOrderDtosList = await _applicationOrderService.GetActiveShippedApplicationOrdersAsync();

                if (applicationOrderDtosList == null || !applicationOrderDtosList.Any())
                    return NotFound($"Not found any active shipping application order.");

                return Ok(applicationOrderDtosList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



        [HttpGet("application-orders/active-delivered", Name = "GetActiveDeliveredApplicationOrder")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<ApplicationOrderDto>>> GetActiveDeliveredApplicationOrder()
        {

            try
            {
                var applicationOrderDtosList = await _applicationOrderService.GetActiveDeliveredApplicationOrdersAsync();

                if (applicationOrderDtosList == null || !applicationOrderDtosList.Any())
                    return NotFound($"Not found any active delivered application order.");

                return Ok(applicationOrderDtosList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPost("{ApplicationId}/shipping-application-orders", Name = "AddNewShippingApplicationOrderByApplicationId")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ApplicationOrderDto>> AddNewShippingApplicationOrderByApplicationId(long ApplicationId)
        {
            if (ApplicationId < 1) return BadRequest("ApplicationId must be bigger than zero.");

            try
            {
                var NewApplicationOrderDto = await _applicationOrderService.AddNewShippedApplicationOrderAsync(ApplicationId);

                if (NewApplicationOrderDto == null) return BadRequest($"Cannot add new shipping application order.");

                return CreatedAtRoute("GetApplicationOrderById",new { ApplicationOrderId  = NewApplicationOrderDto.Id},NewApplicationOrderDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



        [HttpPost("{ApplicationId}/delivered-application-orders", Name = "AddNewDeliveredApplicationOrderByApplicationId")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ApplicationOrderDto>> AddNewDeliveredApplicationOrderByApplicationId(long ApplicationId, [FromBody] string  DeliveredId)
        {
            if (ApplicationId < 1) return BadRequest("ApplicationId must be bigger than zero.");
            if (string.IsNullOrEmpty(DeliveredId)) return BadRequest("DeliveredId cannot be null or empty.");

            try
            {
                var NewApplicationOrderDto = await _applicationOrderService.AddNewDeliveredApplicationOrderAsync(ApplicationId,DeliveredId);

                if (NewApplicationOrderDto == null) return BadRequest($"Cannot add new delivered application order.");

                return CreatedAtRoute("GetApplicationOrderById", new { ApplicationOrderId = NewApplicationOrderDto.Id }, NewApplicationOrderDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
