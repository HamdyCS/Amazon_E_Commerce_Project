using ApiLayer.Help;
using BusinessLayer.Contracks;
using BusinessLayer.Dtos;
using BusinessLayer.Roles;
using BusinessLayer.Servicese;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiLayer.Controllers
{
    [Route("api/application-types")]
    [ApiController]
    public class ApplicationTypesController : ControllerBase
    {
        private readonly IApplicationTypeService _applicationTypeService;

        public ApplicationTypesController(IApplicationTypeService applicationTypeService)
        {
            this._applicationTypeService = applicationTypeService;
        }

        [HttpGet("{ApplicationTypeId}", Name = "GetApplicationTypesById")]
        [Authorize(Roles = Role.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ApplicationTypeDto>> GetApplicationTypesById(int ApplicationTypeId)
        {
            if (ApplicationTypeId < 1) return BadRequest("ApplicationTypeId must be bigger than 0");

            try
            {
               
                var applicationTypeDto = await _applicationTypeService.FindByIdAsync(ApplicationTypeId);

                if (applicationTypeDto == null) return NotFound($"Not found application type. Id = {ApplicationTypeId}");

                return Ok(applicationTypeDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("all", Name = "GetAllApplicationTypes")]
        [Authorize(Roles = Role.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ApplicationTypeDto>> GetAllApplicationTypes()
        {
            try
            {
               
                var applicationTypesDtosList = await _applicationTypeService.GetAllAsync();

                if (applicationTypesDtosList == null || !applicationTypesDtosList.Any()) return NotFound($"Not found any application type.");

                return Ok(applicationTypesDtosList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPut("{ApplicationTypeId}", Name = "UpdateApplicationTypeById")]
        [Authorize(Roles = Role.Admin)]
        [ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> UpdateApplicationTypeById([FromRoute] long ApplicationTypeId, [FromBody] ApplicationTypeDto applicationTypeDto)
        {
            if (ApplicationTypeId < 1) return BadRequest("ApplicationTypeId must be bigger than zero.");


            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                
                var IsApplicationTypeUpdated = await _applicationTypeService.UpdateByIdAsync(ApplicationTypeId, applicationTypeDto);

                if (!IsApplicationTypeUpdated) return BadRequest("Cannot Update application type.");

                return Ok("Updated application type successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


    }
}