using ApiLayer.Help;
using BusinessLayer.Contracks;
using BusinessLayer.Dtos;
using BusinessLayer.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiLayer.Controllers
{
    [Route("api/applications")]
    [ApiController]
    [Authorize(Roles = Role.Customer)]
    public class ApplicationOrdersController : ControllerBase
    {
        private readonly IApplicationOrderService _applicationOrderService;

        public ApplicationOrdersController(IApplicationOrderService applicationOrderService)
        {
            this._applicationOrderService = applicationOrderService;
        }

        [HttpGet("{ApplicationId}/active-application-orders", Name = "GetActiveApplicationOrderbyApplicationId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ApplicationOrderDto>> GetActiveApplicationOrderbyApplicationId(long ApplicationId)
        {
            if (ApplicationId < 1) return BadRequest("Id must be bigger than zero.");

            var userId = Helper.GetIdFromClaimsPrincipal(User);
            if (userId == null) return Unauthorized();


            try
            {
                var applicationOrder = await _applicationOrderService.FindActiveApplicationOrderByApplicationIdAndUserIdAsync(ApplicationId, userId);

                if (applicationOrder == null) return NotFound($"Not found any active application order. Id = {ApplicationId}");

                return Ok(applicationOrder);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



        [HttpGet("{ApplicationId}/track-application-orders", Name = "TrackApplicationOrderByApplicatonIdAndUserId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<ApplicationOrderDto>>> TrackApplicationOrderByApplicatonIdAndUserId(long ApplicationId)
        {
            if (ApplicationId < 1) return BadRequest("ApplicationId must be bigger than zero.");

            var userId = Helper.GetIdFromClaimsPrincipal(User);
            if (userId == null) return Unauthorized();

            try
            {
                var applicationOrderDtosList = await _applicationOrderService.TrackApplicationOrderByApplicatonIdAndUserIdAsync(ApplicationId, userId);

                if (applicationOrderDtosList == null || !applicationOrderDtosList.Any())
                    return NotFound($"Not found any application order. ApplicationId = {ApplicationId}");

                return Ok(applicationOrderDtosList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    } 
}
