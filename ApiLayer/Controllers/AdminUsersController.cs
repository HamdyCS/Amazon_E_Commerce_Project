using ApiLayer.Help;
using BusinessLayer.Contracks;
using BusinessLayer.Dtos;
using BusinessLayer.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace ApiLayer.Controllers
{

    [Route("api/admin/users")]
    [ApiController]
    [Authorize(Roles = Role.Admin)]
    [EnableRateLimiting("FixedWindowPolicyByUserIpAddress")]

    public class AdminUsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public AdminUsersController(IUserService userService)
        {
            this._userService = userService;
        }

        [HttpGet("get-by-email", Name = "GetUserbyEmail")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]

        public async Task<ActionResult<UserDto>> GetUserbyEmail([FromBody] string Email)
        {
            if (string.IsNullOrEmpty(Email)) return BadRequest("Email cannot be null or empty");

            try
            {


                var userDto = await _userService.FindByEmailAsync(Email);

                if (userDto is null) return NotFound("Didnot find user by this email.");

                return Ok(userDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}