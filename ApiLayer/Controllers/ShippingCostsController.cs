using ApiLayer.Help;
using BusinessLayer.Contracks;
using BusinessLayer.Dtos;
using BusinessLayer.Exceptions;
using BusinessLayer.Roles;
using BusinessLayer.Servicese;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;

namespace ApiLayer.Controllers
{
    [Route("api/cities/shipping-costs")]
    [ApiController]
    [EnableRateLimiting("FixedWindowPolicyByUserIpAddress")]

    public class ShippingCostsController : ControllerBase
    {
        private readonly IShippingCostService _shippingCostService;

        public ShippingCostsController(IShippingCostService shippingCostService)
        {
            this._shippingCostService = shippingCostService;
        }

        [HttpGet("{shippingCostId}", Name = "GetShippingCostById")]
        [Authorize(Roles = Role.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ShippingCostDto>> GetShippingCostById(long shippingCostId)
        {
            if (shippingCostId < 1) return BadRequest("shippingCostId must be bigger than zero.");

            try
            {
                var shippingCostDto = await _shippingCostService.FindByIdAsync(shippingCostId);

                if (shippingCostDto == null) return NotFound($"Not found shippin gCost. Id = {shippingCostId}");

                return Ok(shippingCostDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("all", Name = "GetAllShippingCosts")]
        [Authorize(Roles = Role.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ShippingCostDto>> GetAllShippingCosts()
        {
           
            try
            {
                var shippingCostsDtosList = await _shippingCostService.GetAllAsync();

                if (shippingCostsDtosList == null || !shippingCostsDtosList.Any())
                    return NotFound($"Didnot find any shipping cost.");

                return Ok(shippingCostsDtosList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("paged", Name = "GetPagedOfShippingCosts")]
        [Authorize(Roles = Role.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<ShippingCostDto>>> GetPagedOfShippingCosts([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1) return BadRequest("pagenumber and pagesize must be bigger than 0.");

            try
            {
                var shippingCostsDtosList = await _shippingCostService.GetPagedDataAsync(pageNumber, pageSize);

                if (shippingCostsDtosList == null || !shippingCostsDtosList.Any())
                    return NotFound($"Didnot find any shipping cost review.");

                return Ok(shippingCostsDtosList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }


        [HttpPost("", Name = "AddNewofShippingCost")]
        [Authorize(Roles = Role.Admin)]
        [ProducesResponseType(201)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<ShippingCostDto>> AddNewofShippingCost(ShippingCostDto shippingCostDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var UserId = Helper.GetIdFromClaimsPrincipal(User);
                if (UserId == null) return Unauthorized();

                var NewShippingCostDto = await _shippingCostService.AddAsync(shippingCostDto,UserId);

                if (NewShippingCostDto == null) return BadRequest("Cannot add new shipping cost.");

                return CreatedAtRoute("GetShippingCostById", new { shippingCostId = shippingCostDto.Id }, NewShippingCostDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPost("range", Name = "AddRangeOfShippingCost")]
        [Authorize(Roles = Role.Admin)]
        [ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<IEnumerable<ShippingCostDto>>> AddRangeOfShippingCost(IEnumerable<ShippingCostDto> shippingCostsDtosList)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var UserId = Helper.GetIdFromClaimsPrincipal(User);
                if (UserId == null) return Unauthorized();

                var NewShippingCostsDtosList = await _shippingCostService.AddRangeAsync(shippingCostsDtosList, UserId);

                if (shippingCostsDtosList == null || !shippingCostsDtosList.Any()) return BadRequest("Cannot add new shipping costs.");

                return Ok(NewShippingCostsDtosList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPut("{shippingCostId}", Name = "UpdateShippingCostById")]
        [Authorize(Roles = Role.Admin)]
        [ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> UpdateShippingCostById([FromRoute] long shippingCostId, [FromBody] ShippingCostDto shippingCostDto)
        {
            if (shippingCostId < 1) return BadRequest("shippingCostId must be bigger than zero.");


            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var UserId = Helper.GetIdFromClaimsPrincipal(User);
                if (UserId == null) return Unauthorized();

                var IsShippingCostUpdated = await _shippingCostService.UpdateByIdAsync(shippingCostId, shippingCostDto);

                if (!IsShippingCostUpdated) return BadRequest("Cannot Update shipping cost.");

                return Ok("Updated shipping cost successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpDelete("{shippingCostId}", Name = "DeleteShippingCostById")]
        [Authorize(Roles = Role.Admin)]
        [ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> DeleteShippingCostById([FromRoute] long shippingCostId)
        {
            if (shippingCostId < 1) return BadRequest("shippingCostId must be bigger than zero.");

            try
            {
                var IsShippingCostDeleted = await _shippingCostService.DeleteByIdAsync(shippingCostId);

                if (!IsShippingCostDeleted) return BadRequest("Cannot Delete shipping cost.");

                return Ok("Deleted shipping cost successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}