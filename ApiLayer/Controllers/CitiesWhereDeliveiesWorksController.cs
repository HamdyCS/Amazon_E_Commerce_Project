using ApiLayer.Help;
using BusinessLayer.Contracks;
using BusinessLayer.Dtos;
using BusinessLayer.Exceptions;
using BusinessLayer.Roles;
using BusinessLayer.Servicese;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ApiLayer.Controllers
{
    [Route("api/deliveries")]
    [ApiController]
    public class CitiesWhereDeliveiesWorksController : ControllerBase
    {
        private readonly ICityWhereDeliveyWorkService _CityWhereDeliveyWorkService;

        public CitiesWhereDeliveiesWorksController(ICityWhereDeliveyWorkService cityWhereDeliveyWorkService)
        {
            _CityWhereDeliveyWorkService = cityWhereDeliveyWorkService;
        }


        [HttpGet("cities/{citiesWhereDeliveryWorkId}", Name = "GetCityWhereDeliveyWorkById")]
        [Authorize(Roles = Role.DeliveryAgent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<CityWhereDeliveryWorkDto>> GetById(long citiesWhereDeliveryWorkId)
        {
            if (citiesWhereDeliveryWorkId < 1) return BadRequest("Id must be bigger than zero.");

            try
            {
                var UserId = Helper.GetIdFromClaimsPrincipal(User);
                if (UserId == null) return Unauthorized();

                var cityWhereDeliveryWorkDto = await _CityWhereDeliveyWorkService.FindByIdAndDeliveryIdAsync(citiesWhereDeliveryWorkId, UserId);

                if (cityWhereDeliveryWorkDto == null) return NotFound($"Not found city where delivery work. Id = {citiesWhereDeliveryWorkId}");

                return Ok(cityWhereDeliveryWorkDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("cities/admin/{citiesWhereDeliveryWorkId}", Name = "GetCityWhereDeliveyWorkServiceByIdAdmin")]
        [Authorize(Roles = Role.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<CityWhereDeliveryWorkDto>> GetByIdAdmin(long citiesWhereDeliveryWorkId)
        {
            if (citiesWhereDeliveryWorkId < 1) return BadRequest("Id must be bigger than zero.");

            try
            {


                var cityWhereDeliveryWorkDto = await _CityWhereDeliveyWorkService.FindByIdAsync(citiesWhereDeliveryWorkId);

                if (cityWhereDeliveryWorkDto == null) return NotFound($"Not found city where delivery work. Id = {citiesWhereDeliveryWorkId}");

                return Ok(cityWhereDeliveryWorkDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("cities", Name = "GetAllCitiesWhereDeliveryWork")]
        [Authorize(Roles = Role.DeliveryAgent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<CityWhereDeliveryWorkDto>> GetAllCitiesWhereDeliveryWork()
        {

            try
            {
                var UserId = Helper.GetIdFromClaimsPrincipal(User);
                if (UserId == null) return Unauthorized();

                var citiesWhereDeliveryworkDtoslist = await _CityWhereDeliveyWorkService.GetAllCitiesWhereDeliveryWorkByDeliveryId(UserId);

                if (citiesWhereDeliveryworkDtoslist == null || !citiesWhereDeliveryworkDtoslist.Any())
                    return NotFound($"Didnot find any cites where delivery work.");

                return Ok(citiesWhereDeliveryworkDtoslist);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("{DeliveryId}/cities", Name = "GetAllCitiesWhereDeliveryWorkByDeliveryId")]
        [Authorize(Roles = Role.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<CityWhereDeliveryWorkDto>> GetAllCitiesWhereDeliveryWorkByDeliveryId(string DeliveryId)
        {
            try
            {
                var citiesWhereDeliveryworkDtoslist = await _CityWhereDeliveyWorkService.GetAllCitiesWhereDeliveryWorkByDeliveryId(DeliveryId);

                if (citiesWhereDeliveryworkDtoslist == null || !citiesWhereDeliveryworkDtoslist.Any())
                    return NotFound($"Didnot find any cites where delivery work.");

                return Ok(citiesWhereDeliveryworkDtoslist);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



        [HttpPost("cities", Name = "AddNewCityWhereDeliveryWork")]
        [Authorize(Roles = Role.DeliveryAgent)]
        [ProducesResponseType(201)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<CityWhereDeliveryWorkDto>> AddNewCityWhereDeliveryWork([FromBody] long CityId)
        {
            if (CityId < 1) return BadRequest(ModelState);

            try
            {
                var UserId = Helper.GetIdFromClaimsPrincipal(User);
                if (UserId == null) return Unauthorized();

                var NewCityWhereDeliveryWorkDto = await _CityWhereDeliveyWorkService.AddByDeliveryIdAsync(CityId, UserId);

                if (NewCityWhereDeliveryWorkDto == null) return BadRequest("Cannot add new city where delivery work.");

                return CreatedAtRoute("GetCityWhereDeliveyWorkById", new { citiesWhereDeliveryWorkId = NewCityWhereDeliveryWorkDto.Id }, NewCityWhereDeliveryWorkDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPost("cities/range", Name = "AddNewCitiesWhereDeliveryWork")]
        [Authorize(Roles = Role.DeliveryAgent)]
        [ProducesResponseType(201)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<IEnumerable<CityWhereDeliveryWorkDto>>> AddNewCitiesWhereDeliveryWork([FromBody] IEnumerable<long> CitiesIdsList)
        {
            if (CitiesIdsList is null || !CitiesIdsList.Any()) return BadRequest("CitiesIdsList cannot be empty");

            try
            {
                var UserId = Helper.GetIdFromClaimsPrincipal(User);
                if (UserId == null) return Unauthorized();

                var NewCitiesWhereDeliveryWorkDtosList = await _CityWhereDeliveyWorkService.AddRangeByDeliveryIdAsync(CitiesIdsList, UserId);

                if (NewCitiesWhereDeliveryWorkDtosList == null) return BadRequest("Cannot add new cities where delivery work.");

                return Ok(NewCitiesWhereDeliveryWorkDtosList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



        [HttpPut("cities/{citiesWhereDeliveryWorkId}", Name = "UpdateCityWhereDeliveryWorkById")]
        [Authorize(Roles = Role.DeliveryAgent)]
        [ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> UpdateCityWhereDeliveryWorkById([FromRoute] long citiesWhereDeliveryWorkId, [FromBody] long cityId)
        {
            if (cityId < 1) return BadRequest("cityId must be bigger than zero.");


            try
            {
                var UserId = Helper.GetIdFromClaimsPrincipal(User);
                if (UserId == null) return Unauthorized();

                var IsCityWhereDeliveryWorkUpdated = await _CityWhereDeliveyWorkService.UpdateByIdAndDeliveryIdAsync(citiesWhereDeliveryWorkId, UserId, cityId);

                if (!IsCityWhereDeliveryWorkUpdated) return BadRequest("Cannot Update city where delivery work.");

                return Ok("Updated city where delivery work successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpDelete("cities/{citiesWhereDeliveryWorkId}", Name = "DeleteCityWhereDeliveryWorkById")]
        [Authorize(Roles = Role.DeliveryAgent)]
        [ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> DeleteCityWhereDeliveryWorkById([FromRoute] long citiesWhereDeliveryWorkId)
        {
            if (citiesWhereDeliveryWorkId < 1) return BadRequest("citiesWhereDeliveryWorkId must be bigger than zero.");

            try
            {
                var UserId = Helper.GetIdFromClaimsPrincipal(User);
                if (UserId == null) return Unauthorized();
                var IsCityWhereDeliveryDeleted = await _CityWhereDeliveyWorkService.DeleteByIdAndDeliveryIdAsync(citiesWhereDeliveryWorkId, UserId);

                if (!IsCityWhereDeliveryDeleted) return BadRequest("Cannot Delete city where delivery work.");

                return Ok("Deleted city where delivery work successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



        //range مش شغال مع
        // post man
        // بسبب انه بيستقبل بيانات في 
        //body
        // وهي http delete
        //[HttpDelete("cities/range", Name = "DeleteRangeOfCitiesWhereDeliveryWorkById")]
        //[Authorize(Roles = Role.DeliveryAgent)]
        //[ProducesResponseType(200)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<ActionResult<string>> DeleteRangeOfCitiesWhereDeliveryWorkById([FromBody] IEnumerable<long> citiesWhereDeliveryWorkIdsList)
        //{
        //    if (citiesWhereDeliveryWorkIdsList is null || !citiesWhereDeliveryWorkIdsList.Any()) return BadRequest("citiesWhereDeliveryWorkIdsList cannot be empty");

        //    try
        //    {
        //        var UserId = Helper.GetIdFromClaimsPrincipal(User);
        //        if (UserId == null) return Unauthorized();
        //        var IsCityWhereDeliveryDeleted = await _CityWhereDeliveyWorkService.DeleteRangeByIdAndDeliveryIdAsync(citiesWhereDeliveryWorkIdsList, UserId);

        //        if (!IsCityWhereDeliveryDeleted) return BadRequest("Cannot Delete cities where delivery work.");

        //        return Ok("Deleted cities where delivery work successfully.");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex.Message);
        //    }
        //}



    }

}
