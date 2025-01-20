using BusinessLayer.Contracks;
using BusinessLayer.Dtos;
using BusinessLayer.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace ApiLayer.Controllers
{
    [Route("api/application-order-types")]
    [ApiController]
    [EnableRateLimiting("FixedWindowPolicyByUserIpAddress")]

    public class ApplicationOrderTypesController : ControllerBase
    {
        private readonly IApplicationOrderTypeService _applicationOrderTypeService;

        public ApplicationOrderTypesController(IApplicationOrderTypeService applicationOrderTypeService)
        {
            this._applicationOrderTypeService = applicationOrderTypeService;
        }

        [HttpGet("{ApplicationOrderTypeId}", Name = "GetApplicationOrderTypeById")]
        [Authorize(Roles = Role.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ApplicationOrderTypeDto>> GetApplicationOrderTypeById(long ApplicationOrderTypeId)
        {
            if (ApplicationOrderTypeId < 1) return BadRequest("ApplicationOrderTypeId must be bigger than 0");

            try
            {

                var applicationOrderTypeDto = await _applicationOrderTypeService.FindByIdAsync(ApplicationOrderTypeId);

                if (applicationOrderTypeDto == null) return NotFound($"Not found application type. Id = {ApplicationOrderTypeId}");

                return Ok(applicationOrderTypeDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("all", Name = "GetAllApplicationOrderTypes")]
        [Authorize(Roles = Role.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ApplicationOrderTypeDto>> GetAllApplicationOrderTypes()
        {
            try
            {

                var applicationOrderTypesDtosList = await _applicationOrderTypeService.GetAllAsync();

                if (applicationOrderTypesDtosList == null || !applicationOrderTypesDtosList.Any()) return NotFound($"Not found any application order type.");

                return Ok(applicationOrderTypesDtosList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPut("{ApplicationOrderTypeId}", Name = "UpdateApplicationOrderTypeById")]
        [Authorize(Roles = Role.Admin)]
        [ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> UpdateApplicationOrderTypeById([FromRoute] long ApplicationOrderTypeId, [FromBody] ApplicationOrderTypeDto applicationOrderTypeDto)
        {
            if (ApplicationOrderTypeId < 1) return BadRequest("ApplicationOrderTypeId must be bigger than zero.");


            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {

                var IsApplicationOrderTypeUpdated = await _applicationOrderTypeService.UpdateByIdAsync(ApplicationOrderTypeId, applicationOrderTypeDto);

                if (!IsApplicationOrderTypeUpdated) return BadRequest("Cannot Update application order type.");

                return Ok("Updated application order type successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


    }
}