using BusinessLayer.Contracks;
using BusinessLayer.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ApiLayer.Controllers
{
    [Route("api/UsersAddresses")]
    [ApiController]
    [Authorize]
    public class UsersAddressesController : ControllerBase
    {
        private readonly IUserAddressService _userAddressService;

        public UsersAddressesController(IUserAddressService userAddressService)
        {
            this._userAddressService = userAddressService;
        }

        [HttpGet("all",Name = "GetAllUserAddresses")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<UserAddressDto>>> GetAllUserAddresses()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                if (userId is null) return Unauthorized("UserId not found");

                var userAddressesDtos = await _userAddressService.GetAllUserAddressesByUserIdAsync(userId);
                return Ok(userAddressesDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("count", Name = "GetCountOfUserAddresses")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> GetCountOfUserAddresses()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                if (userId is null) return Unauthorized("UserId not found");

                var userAddressesDtos = await _userAddressService.GetCountOfUserAddressesByUserId(userId);
                return Ok(userAddressesDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



        [HttpGet("{Id}", Name = "GetInfo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<UserAddressDto>> GetInfo(long Id)
        {
            try
            {
                if (Id < 1) return BadRequest("Id must be bigger than 1");

                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                if (userId is null) return Unauthorized("UserId not found");

                var userAddressDtos = await _userAddressService.FindByIdAndUserIdAsync(Id,userId);

                if (userAddressDtos is null) return NotFound("Not found!");

                return Ok(userAddressDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPost("",Name = "AddNewUserAddress")]
        [ProducesResponseType(201)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserAddressDto>> AddNewUserAddress([FromBody] UserAddressDto userAddressDto)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                if (userId is null) return Unauthorized("UserId not found");

                var NewUserAddressDto = await _userAddressService.AddAsync(userId,userAddressDto);
                if (NewUserAddressDto is null) return BadRequest("Cannot add new user address");

                return CreatedAtRoute("GetInfo", new { Id = NewUserAddressDto.Id }, NewUserAddressDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



        [HttpPut("{Id}", Name = "UpdateUserAddress")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUserAAddress([FromRoute] long Id,[FromBody] UserAddressDto NewUserAddressDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                if (userId is null) return Unauthorized("UserId not found");

                var userAddressDto = await _userAddressService.FindByIdAndUserIdAsync(Id, userId);
                if (userAddressDto is null) return NotFound("Not found user address");

                var IsUpdated = await _userAddressService.UpdateByIdAndUserIdAsync(Id,userId,NewUserAddressDto);
                if (!IsUpdated) return BadRequest("Cannot update user address");

                return Ok("Updated user address successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpDelete("{Id}", Name = "DeleteUserAddress")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteUserAddress([FromRoute] long Id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                if (userId is null) return Unauthorized("UserId not found");

                var userAddressDto = await _userAddressService.FindByIdAndUserIdAsync(Id, userId);
                if (userAddressDto is null) return NotFound("Not found user address");

                var IsUpdated = await _userAddressService.DeleteByIdAndUserIdAsync(Id, userId);
                if (!IsUpdated) return BadRequest("Cannot update user address");

                return Ok("Deleted user address successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }

}
