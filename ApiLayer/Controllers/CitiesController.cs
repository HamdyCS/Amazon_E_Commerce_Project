using ApiLayer.Help;
using BusinessLayer.Contracks;
using BusinessLayer.Dtos;
using BusinessLayer.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace ApiLayer.Controllers
{
    [Route("api/cities")]
    [ApiController]
    [Authorize]
    [EnableRateLimiting("FixedWindowPolicyByUserIpAddress")]

    public class CitiesController : ControllerBase
    {
        private readonly ICityService _cityService;

        public CitiesController(ICityService cityService)
        {
            this._cityService = cityService;
        }

        [HttpGet("all-paged", Name = "GetPagedData")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<CityDto>>> GetPagedData([FromQuery]int page, [FromQuery]int pageSize)
        {
            if (page < 1 || pageSize < 1)
                return BadRequest("Page or pageSize must be bigger than 0");
            try
            {
                var citiesDtos = await _cityService.GetPagedDataAsync(page, pageSize);

                if(!citiesDtos.Any()) return NotFound("Not found any city");
                return Ok(citiesDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("all", Name = "GetAll")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<CityDto>>> GetAll()
        {
            
            try
            {
                var citiesDtos = await _cityService.GetAllAsync();

                if (!citiesDtos.Any()) return NotFound("Not found any city");
                return Ok(citiesDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



        [HttpGet("{Id}", Name = "GetInfoById")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<CityDto>> GetInfoById([FromRoute]long Id)
        {
            if (Id < 1) return BadRequest("Id must be bigger than 0");
            try
            {
                var cityDto = await _cityService.FindByIdAsync(Id);

                if (cityDto is null) return NotFound("Not found city");
                
                return Ok(cityDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPost("", Name = "AddNewCity")]
        [Authorize(Roles = Role.Admin)]
        [ProducesResponseType(201)]
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<CityDto>> AddNewCity([FromBody] CityDto cityDto)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var userId = Helper.GetIdFromClaimsPrincipal(User);
                if(userId is null) return Unauthorized("UserId not found");
                
                var NewCityDto = await _cityService.AddAsync(cityDto,userId);

                if (NewCityDto is null) return BadRequest("cannot add new city");

                return CreatedAtRoute("GetInfoById",new { Id = userId},NewCityDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPut("{Id}", Name = "UpdateCity")]
        [Authorize(Roles = Role.Admin)]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateCity([FromRoute] long Id,[FromBody] CityDto NewcityDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (Id < 1) return BadRequest("Id must be bigger than 1");

            try
            {

                var cityDto = await _cityService.FindByIdAsync(Id);
                if (cityDto is null) return NotFound("city not found");

                var IsUpdated = await _cityService.UpdateByIdAsync(Id, NewcityDto);

                if (!IsUpdated) return BadRequest("Cannot updated city");

                return Ok("Updated city successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



        [HttpDelete("{Id}", Name = "DeleteCity")]
        [Authorize(Roles = Role.Admin)]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateCity([FromRoute] long Id)
        {
            if (Id < 1) return BadRequest("Id must be bigger than 1");

            try
            {

                var cityDto = await _cityService.FindByIdAsync(Id);
                if (cityDto is null) return NotFound("city not found");

                var IsDeleted = await _cityService.DeleteByIdAsync(Id);

                if (!IsDeleted) return BadRequest("Cannot deleted city");

                return Ok("Deleted city successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }

}
