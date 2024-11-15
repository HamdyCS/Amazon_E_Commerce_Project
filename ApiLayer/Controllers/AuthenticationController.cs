using BusinessLayer.Contracks;
using BusinessLayer.Dtos;
using BusinessLayer.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiLayer.Controllers
{
    [Route("api/Authentication")]
    [ApiController]
    [Authorize]
    public class AuthenticationController : ControllerBase
    {
        private readonly IPendingUserService _pendingUserService;
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public AuthenticationController(IPendingUserService pendingUser,IUserService userService, ITokenService tokenService)
        {
            _pendingUserService = pendingUser;
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpPost("RegisterAdminUser", Name = "RegisterAdminUser")]
        //[Authorize(Roles = Role.Admin)]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> RegisterAdminUser([FromBody] UserDto userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var IsAdded = await _pendingUserService.AddNewPendingUserAsync(userDto, Role.Admin);

                if (IsAdded) return Ok("Add new pending user Successfully");
                else
                    return BadRequest();

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



        [HttpPost("Login")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TokenDto>> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var userDto = await _userService.GetUserByEmailAndPasswordAsync(loginDto);
                if (userDto is null) return BadRequest("Invaild email or password");


                var userRoles = await _userService.GetAllUserRolesByIdAsync(userDto.Id);
                if (userRoles is null) return BadRequest("Invaild get user roles");

                var userRolesString = new List<string>();
                foreach (var role in userRoles) userRolesString.Add(role.Name);


                var token = _tokenService.GenerateJwtToken(userDto.Id, userDto.Email, userRolesString);
                var refreshToken = await _tokenService.AddNewRefreshTokenByUserIdAsync(userDto.Id);


                var tokenDto = new TokenDto { Token = token, RefreshToken = refreshToken };

                return Ok(tokenDto);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);

            }
        }



        [HttpPost("ConfirmEmail")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<TokenDto>> ConfirmEmail(ConfirmEmailDto confirmEmailDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var userDto = await _userService.AddNewUserByEmailAndCodeAsync(confirmEmailDto.Email, confirmEmailDto.Code);

                if(userDto is null) return BadRequest("Pending user not found");
                var PendingUserRoleName = await  _pendingUserService.GetPendingUserRoleNameByEmailAndCode(confirmEmailDto.Email, confirmEmailDto.Code);

                if(PendingUserRoleName is null) return BadRequest("Pending user role not found");

                var IsAddedToRole = await _userService.AddToRoleByIdAsync(userDto.Id, new RoleDto { Name = PendingUserRoleName });
                if (!IsAddedToRole) return BadRequest("Failed to add user to role.");


                var PendingUserId = await _pendingUserService.GetIdByEmailAndCodeAsync(confirmEmailDto.Email, confirmEmailDto.Code);
                if (PendingUserId is null) return NotFound("Not found pending user by id");

                var pendingUserIsDeleted = await _pendingUserService.RemoveByIdAsync(PendingUserId);
                if (!pendingUserIsDeleted) return null;



                var userRoleslist = new List<string> { PendingUserRoleName };
                var token = _tokenService.GenerateJwtToken(userDto.Id, userDto.Email, userRoleslist);


                var refreshToken = await _tokenService.AddNewRefreshTokenByUserIdAsync(userDto.Id);
                var tokenDto = new TokenDto { Token = token, RefreshToken = refreshToken };

                return Ok(tokenDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }


    }
}
