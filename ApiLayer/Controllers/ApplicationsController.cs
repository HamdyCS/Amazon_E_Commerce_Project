
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
    public class ApplicationsController : ControllerBase
    {
        private readonly IApplicationService _applicationService;

        public ApplicationsController(IApplicationService applicationService)
        {
            this._applicationService = applicationService;
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



    }
}

