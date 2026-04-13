using BusinessLayer.Contracks;
using BusinessLayer.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;

namespace ApiLayer.Controllers
{
    [Route("api/user-addresses")]
    [ApiController]
    [Authorize]
    [EnableRateLimiting("FixedWindowPolicyByUserIpAddress")]

    public class UsersAddressesController : ControllerBase
    {
        private readonly IUserAddressService _userAddressService;

        public UsersAddressesController(IUserAddressService userAddressService)
        {
            this._userAddressService = userAddressService;
        }

        [HttpGet("all", Name = "GetAllUserAddresses")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<UserAddressDto>>> GetAllUserAddresses()
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (userId is null) return Unauthorized("UserId not found");

            var userAddressesDtos = await _userAddressService.GetAllUserAddressesByUserIdAsync(userId);
            return Ok(userAddressesDtos);

        }


        [HttpGet("count", Name = "GetCountOfUserAddresses")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> GetCountOfUserAddresses()
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (userId is null) return Unauthorized("UserId not found");

            var userAddressesDtos = await _userAddressService.GetCountOfUserAddressesByUserId(userId);
            return Ok(userAddressesDtos);

        }



        [HttpGet("{Id}", Name = "GetAddress")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<UserAddressDto>> GetUserAddress(long Id)
        {

            if (Id < 1) return BadRequest("Id must be bigger than 1");

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (userId is null) return Unauthorized("UserId not found");

            var userAddressDtos = await _userAddressService.FindByIdAndUserIdAsync(Id, userId);

            if (userAddressDtos is null) return NotFound("Not found!");

            return Ok(userAddressDtos);

        }


        [HttpPost("", Name = "AddNewUserAddress")]
        [ProducesResponseType(201)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserAddressDto>> AddNewUserAddress([FromBody] UserAddressDto userAddressDto)
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (userId is null) return Unauthorized("UserId not found");

            var NewUserAddressDto = await _userAddressService.AddAsync(userId, userAddressDto);
            if (NewUserAddressDto is null) return BadRequest("Cannot add new user address");

            return CreatedAtRoute("GetAddress", new { Id = NewUserAddressDto.Id }, NewUserAddressDto);

        }



        [HttpPut("{Id}", Name = "UpdateUserAddress")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUserAddress([FromRoute] long Id, [FromBody] UserAddressDto userAddressDto)
        {

            if (Id < 1) return BadRequest("Id must be bigger than 1");

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (userId is null) return Unauthorized("UserId not found");

            var IsUpdated = await _userAddressService.UpdateByIdAndUserIdAsync(Id, userId, userAddressDto);
            if (!IsUpdated) return BadRequest("Cannot update user address");

            return Ok(new
            {
                Message = "Updated user address successfully"
            });

        }


        [HttpDelete("{Id}", Name = "DeleteUserAddress")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteUserAddress([FromRoute] long Id)
        {
            if (Id < 1) return BadRequest("Id must be bigger than Zero");

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (userId is null) return Unauthorized("UserId not found");

            var IsUpdated = await _userAddressService.DeleteByIdAndUserIdAsync(Id, userId);
            if (!IsUpdated) return BadRequest("Cannot update user address");

            return Ok(
            new
            {
                Message = "Deleted user address successfully"
            } );

        }
    }

}
