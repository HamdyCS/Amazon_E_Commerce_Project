
using ApiLayer.Help;
using BusinessLayer.Contracks;
using BusinessLayer.Dtos;
using BusinessLayer.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace ApiLayer.Controllers
{
    [Route("api/applications")]
    [ApiController]
    [EnableRateLimiting("FixedWindowPolicyByUserIpAddress")]

    public class ApplicationsController : ControllerBase
    {
        private readonly IApplicationService _applicationService;
        private readonly IOrderApplicationSummaryService _orderApplicationSummaryService;

        public ApplicationsController(IApplicationService applicationService,IOrderApplicationSummaryService orderApplicationSummaryService)
        {
            this._applicationService = applicationService;
            this._orderApplicationSummaryService = orderApplicationSummaryService;
        }

        [HttpGet("all",Name = "ApplicationsController")]
        [Authorize(Roles = Role.Customer)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<ApplicationDto>>> GetAllUserApplications()
        {
            try
            {
                var UserId = Helper.GetIdFromClaimsPrincipal(User);
                if(UserId == null) return Unauthorized();

                var applicationDtosList = await _applicationService.GetAllUserApplicationsByUserIdAsync(UserId);
                if (applicationDtosList is null || !applicationDtosList.Any())
                    return NotFound("Not found any application.");

                return Ok(applicationDtosList);
            }
            catch (Exception ex)
            {
                
                return StatusCode(500,ex.Message);
            }
        }


        [HttpGet("all-return", Name = "GetAllReturnApplications")]
        [Authorize(Roles = Role.Admin)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<ApplicationDto>>> GetAllReturnApplications()
        {
            try
            {
               

                var applicationDtosList = await _applicationService.GetAllReturnApplicationsAsync();
                if (applicationDtosList is null || !applicationDtosList.Any())
                    return NotFound("Not found any return application.");

                return Ok(applicationDtosList);
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("all-user-return", Name = "GetAllUserReturnApplications")]
        [Authorize(Roles = Role.Admin)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<ApplicationDto>>> GetAllUserReturnApplications([FromBody]string UserId)
        {
            try
            {
             
                var applicationDtosList = await _applicationService.GetAllUserReturnApplicationsByUserIdAsync(UserId);
                if (applicationDtosList is null || !applicationDtosList.Any())
                    return NotFound("Not found any return application.");

                return Ok(applicationDtosList);
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("{ApplcationId}/shopping-cart", Name = "GetShoppingCartbyApplicationId")]
        [Authorize(Roles = Role.Admin)]
        [ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<ShoppingCartDto>>> GetShoppingCartbyApplicationId(long ApplcationId)
        {
            try
            {
                var userId = Helper.GetIdFromClaimsPrincipal(User);
                if(userId is null) return Unauthorized();

                var shoppingCartDto = await _applicationService.FindShoppingCartByApplicationIdAndUserIdAsync(ApplcationId,userId);
                if (shoppingCartDto is null)
                    return NotFound($"Not found any shopping cart. ApplicationId = {ApplcationId}");

                return Ok(shoppingCartDto);
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }



        [HttpPost("{ApplcationId}/return", Name = "AddNewReturnApplicationByUserId")]
        [Authorize(Roles = Role.Admin)]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<ApplicationDto>>> AddNewReturnApplicationByUserId(long ApplcationId,[FromBody]string UserId)
        {
            if (string.IsNullOrEmpty(UserId)) return BadRequest("UserId cannot be null or empty");
            try
            {
           
                var ReturnApplicatonDto = await _applicationService.AddNewReturnApplicationAsync(UserId,ApplcationId);
                if (ReturnApplicatonDto is null)
                    return BadRequest("cannot add new return application.");

                return Ok(ReturnApplicatonDto);
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{ApplcationId}/order-application-summary", Name = "GetUserOrderApplicationSummaryByApplicationId")]
        [Authorize(Roles = Role.Customer)]
        [ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<OrderApplicationSummaryDto>> GetUserOrderApplicationSummaryByApplicationId(long ApplcationId)
        {
            try
            {
                var userId = Helper.GetIdFromClaimsPrincipal(User);
                if (userId is null) return Unauthorized();

                var orderApplicationSummaryDto = await _orderApplicationSummaryService.GetUserOrderApplicationSummaryByUserIdAndApplicationIdAsync(ApplcationId, userId);
                if (orderApplicationSummaryDto is null)
                    return NotFound($"Not found order application summary. ApplicationId = {ApplcationId}");

                return Ok(orderApplicationSummaryDto);
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("order-application-summaries", Name = "GetAllUserOrderApplicationSummaries")]
        [Authorize(Roles = Role.Customer)]
        [ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<OrderApplicationSummaryDto>>> GetAllUserOrderApplicationSummaries()
        {
            try
            {
                var userId = Helper.GetIdFromClaimsPrincipal(User);
                if (userId is null) return Unauthorized();

                var orderApplicationSummaryDtosList = await _orderApplicationSummaryService.GetAllUserOrderApplicationSummariesByUserIdAsync(userId);
                if (orderApplicationSummaryDtosList is null || !orderApplicationSummaryDtosList.Any())
                    return NotFound($"Not found any order application summary.");

                return Ok(orderApplicationSummaryDtosList);
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }


    }
}

