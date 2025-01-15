
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

        public async Task<ActionResult<IEnumerable<ApplicationDto>>> GetAllUserApplicationsAsync()
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

    }
}

