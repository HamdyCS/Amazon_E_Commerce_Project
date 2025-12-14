using ApiLayer.Help;
using BusinessLayer.Contracks;
using BusinessLayer.Dtos;
using BusinessLayer.Enums;
using BusinessLayer.Roles;
using DataAccessLayer.Identity.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;

namespace ApiLayer.Controllers
{
    [Route("api/Authentication")]
    [ApiController]
    [Authorize]
    [EnableRateLimiting("FixedWindowPolicyByUserIpAddress")]

    public class AuthenticationController : ControllerBase
    {
        private readonly IOtpService _otpService;
        private readonly IMailService _mailService;
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<User> _signInManager;

        public AuthenticationController(IOtpService otpService, IMailService mailService,
            IUserService userService, ITokenService tokenService, SignInManager<User> signInManager)
        {
            _otpService = otpService;
            _mailService = mailService;
            _userService = userService;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }


        [HttpGet("", Name = "GetUserInfo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public async Task<ActionResult<bool>> GetUserInfo()
        {


            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId is null)
                    return Unauthorized();

                var userDto = await _userService.FindByIdAsync(userId);
                return Ok(userDto);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("is-email-exist", Name = "IsEmailExist")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> IsEmailExist(string email)
        {
            if (string.IsNullOrEmpty(email)) return BadRequest("Email cannot be null or empty");

            try
            {
                var IsExist = await _userService.IsEmailExistAsync(email);
                return Ok(IsExist);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPost("send-otp", Name = "SendOtpToEmail")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> SendOtpToEmail([FromBody] string Email)
        {
            if (string.IsNullOrEmpty(Email)) return BadRequest("Email cannot be null or empty");

            try
            {
                var code = BusinessLayer.Help.Helper.GenerateRandomSixDigitNumber();
                var otpDto = new OtpDto { Email = Email, Code = code.ToString() };

                var NewOptDto = await _otpService.AddNewOtpAsync(otpDto);
                if (NewOptDto is null) return BadRequest("Cannot add new opt");

                await _mailService.SendEmailAsync(otpDto.Email, "Confirmation code", otpDto.Code);

                return Ok("Send otp to email successfuly");

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPost("register-admin", Name = "RegisterAdminUser")]
        [Authorize(Roles = Role.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public async Task<ActionResult<TokenDto>> RegisterAdminUser([FromBody] UserDto userDto, [FromQuery] string Code)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var NewUserDto = await _userService.AddNewUserByEmailAndCodeAsync(userDto, userDto.Email, Code);
                if (NewUserDto is null) return BadRequest("Code is wrong or not active or user info is Eligible");

                var IsAddedToRole = await _userService.AddToRoleByIdAsync(NewUserDto.Id, new RoleDto { Name = Role.Admin });
                if (!IsAddedToRole) return BadRequest("Cannot add to admin role");

                var refreshToken = await _tokenService.AddNewRefreshTokenByUserIdAsync(NewUserDto.Id);
                var token = _tokenService.GenerateJwtToken(userDto.Id, userDto.Email, new List<string> { Role.Admin });

                var tokenDto = new TokenDto { Token = token, RefreshToken = refreshToken };

                return Ok(tokenDto);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPost("register-customer", Name = "RegisterCustomerUser")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TokenDto>> RegisterCustomerUser([FromBody] UserDto userDto, [FromQuery] string Code)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var NewUserDto = await _userService.AddNewUserByEmailAndCodeAsync(userDto, userDto.Email, Code);
                if (NewUserDto is null) return BadRequest("Code is wrong or not active or user info is Eligible");

                var IsAddedToRole = await _userService.AddToRoleByIdAsync(NewUserDto.Id, new RoleDto { Name = Role.Customer });
                if (!IsAddedToRole) return BadRequest("Cannot add to admin role");

                var refreshToken = await _tokenService.AddNewRefreshTokenByUserIdAsync(NewUserDto.Id);
                var token = _tokenService.GenerateJwtToken(userDto.Id, userDto.Email, new List<string> { Role.Customer });

                var tokenDto = new TokenDto { Token = token, RefreshToken = refreshToken };

                return Ok(tokenDto);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



        [HttpPost("register-seller", Name = "RegisterSellerUser")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TokenDto>> RegisterSellerUser([FromBody] UserDto userDto, [FromQuery] string Code)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var NewUserDto = await _userService.AddNewUserByEmailAndCodeAsync(userDto, userDto.Email, Code);
                if (NewUserDto is null) return BadRequest("Code is wrong or not active or user info is Eligible");

                var IsAddedToRole = await _userService.AddToRoleByIdAsync(NewUserDto.Id, new RoleDto { Name = Role.Seller });
                if (!IsAddedToRole) return BadRequest("Cannot add to admin role");

                var refreshToken = await _tokenService.AddNewRefreshTokenByUserIdAsync(NewUserDto.Id);
                var token = _tokenService.GenerateJwtToken(userDto.Id, userDto.Email, new List<string> { Role.Seller });

                var tokenDto = new TokenDto { Token = token, RefreshToken = refreshToken };

                return Ok(tokenDto);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPost("register-deliveryagent", Name = "RegisterDeliveryAgentUser")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TokenDto>> RegisterDeliveryAgentUser([FromBody] UserDto userDto, [FromQuery] string Code)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var NewUserDto = await _userService.AddNewUserByEmailAndCodeAsync(userDto, userDto.Email, Code);
                if (NewUserDto is null) return BadRequest("Code is wrong or not active or user info is Eligible");

                var IsAddedToRole = await _userService.AddToRoleByIdAsync(NewUserDto.Id, new RoleDto { Name = Role.DeliveryAgent });
                if (!IsAddedToRole) return BadRequest("Cannot add to admin role");

                var refreshToken = await _tokenService.AddNewRefreshTokenByUserIdAsync(NewUserDto.Id);
                var token = _tokenService.GenerateJwtToken(userDto.Id, userDto.Email, new List<string> { Role.DeliveryAgent });

                var tokenDto = new TokenDto { Token = token, RefreshToken = refreshToken };

                return Ok(tokenDto);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPost("login", Name = "Login")]
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
                if (userDto is null) return Unauthorized("Invaild email or password");


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



        [HttpPut("reset-password", Name = "ResetPassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public async Task<ActionResult<TokenDto>> ResetPassword([FromBody] string NewPassword, [FromQuery] string Code)
        {
            if (string.IsNullOrEmpty(NewPassword) || string.IsNullOrEmpty(Code))
                return BadRequest("code or Newpassword cannot be null or empty");

            try
            {

                var Email = User.FindFirst(ClaimTypes.Email).Value;
                if (Email is null) return Unauthorized("Email not found");

                var IsResetedPassword = await _userService.ResetPasswordByEmailAsync(Email, NewPassword, Code);
                if (!IsResetedPassword) return BadRequest("Cannot reseted password");

                return Ok("Changed password successfuly");

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



        [HttpPut("update-password", Name = "UpdatePassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public async Task<ActionResult<TokenDto>> UpdatePassword([FromBody] string NewPassword)
        {
            if (string.IsNullOrEmpty(NewPassword))
                return BadRequest("Newpassword cannot be null or empty");

            try
            {
                if (User is null) return StatusCode(StatusCodes.Status403Forbidden);

                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                if (userId == null) return Unauthorized();

                var IsUpdatedPassword = await _userService.UpdatePasswordAsync(userId, NewPassword);
                if (!IsUpdatedPassword) return BadRequest("Cannot updated password");

                return Ok("Changed password successfuly");

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("all", Name = "GetUsersPaged")]
        [Authorize(Roles = Role.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsersPaged([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1) return BadRequest("pagenumber and pagesize must be bigger than 0");

            try
            {
                var listOfUserDto = await _userService.GetPagedDataAsync(pageNumber, pageSize);
                return Ok(listOfUserDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }


        [HttpGet("count", Name = "GetCountOfUsers")]
        [Authorize(Roles = Role.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public async Task<ActionResult<long>> GetCountOfUsers()
        {

            try
            {
                var CountOfUsers = await _userService.GetCountOfUsersAsync();
                return Ok(CountOfUsers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }


        [HttpDelete(Name = "DeleteAccount")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> DeleteAccount()
        {

            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId is null) return Unauthorized("User id is null");

                var IsUserDeleted = await _userService.DeleteByIdAsync(userId);

                if (IsUserDeleted) return Ok("Deleted accont successfuly");
                else
                    return BadRequest("Cannot delete user");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }


        [HttpDelete("{Id}", Name = "DeleteAccountById")]
        [Authorize(Roles = Role.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public async Task<ActionResult> DeleteAccountById([FromRoute] string Id)
        {
            if (string.IsNullOrEmpty(Id)) return BadRequest("Id cannot be null");
            try
            {

                var IsUserDeleted = await _userService.DeleteByIdAsync(Id);

                if (IsUserDeleted) return Ok("Deleted accont successfuly");
                else
                    return BadRequest("Cannot delete user or user not found");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }


        [HttpPut("update-email", Name = "UpdateEmail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public async Task<ActionResult> UpdateEmail([FromBody] string NewEmail, [FromQuery] string Code)
        {
            if (string.IsNullOrEmpty(NewEmail)) return BadRequest("New Email Cannot be null or empty");
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId is null) return BadRequest("User id is null");

                bool IsEmailUpdated = await _userService.UpdateEmailAsync(userId, NewEmail, Code);

                if
                    (IsEmailUpdated) return Ok("Updated email successfuly");
                else
                    return BadRequest("Cannot update email");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }


        }


        [HttpPut(Name = "UpdateUserInfo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public async Task<ActionResult> UpdateUserInfo([FromBody] UserDto userDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId is null) return Unauthorized("UserId not found");

                bool IsUserUpdated = await _userService.UpdateUserByIdAsync(userId, userDto);

                if
                    (IsUserUpdated) return Ok("Updated user info successfuly");
                else
                    return BadRequest("Cannot update user info");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }


        [HttpGet("is-otp-valid", Name = "CheckIfOtpValid")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> CheckIfOtpValid([FromQuery] string Code, [FromQuery] string Email)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {

                if (Email is null) return Unauthorized("Email not found");

                bool IsOtpActiveAndNotUsed = await _otpService.CheckIfOtpActiveAndNotUsedAsync(new OtpDto { Email = Email, Code = Code });

                if
                    (IsOtpActiveAndNotUsed) return Ok(true);
                else
                    return Ok(false);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }


        [HttpPost("refresh-token", Name = "RefreshToken")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public async Task<ActionResult<string>> RefreshToken([FromBody] string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken)) return BadRequest("Refresh token cannot be null or empty");

            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var Email = User.FindFirst(ClaimTypes.Email).Value;
                if (userId is null || Email is null) return Unauthorized("Email or userId cannot be null");

                var IsRefreshTokenActive = await _tokenService.CheckIfRefreshTokenIsActiveByUserIdAsync(userId, refreshToken);
                if (!IsRefreshTokenActive) return Unauthorized("Refresh token is not active");

                var userRolesDto = await _userService.GetAllUserRolesByIdAsync(userId);
                if (userRolesDto is null) return Unauthorized("User didnot has any role");

                var userRoles = userRolesDto.Select(r => r.Name).ToArray();
                var token = _tokenService.GenerateJwtToken(userId, Email, userRoles);

                return Ok(token);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("login/customar/github", Name = "LoginCustomarWithGithub")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(302)]
        public IActionResult LoginCustomarWithGithub([FromQuery] string returnUrl)
        {

            var redirectUrl = Url.Action("ExternalLoginCallback", "Authentication", new { returnUrl }, Request.Scheme);

            if (redirectUrl == null)
                return BadRequest("Cannot create redirect url");

            var propertes = _userService.CreateAuthenticationProperties(EnProvider.GitHub.ToString(), redirectUrl);
            //HTTP 302 Redirect توجيه المستخدم لموقع تسجيل الدخول الخاص بال provider
            return Challenge(propertes, EnProvider.GitHub.ToString());
        }


        [HttpGet("login/customar/google", Name = "LoginCustomarWithGoogle")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(302)]
        public IActionResult LoginCustomarWithGoogle([FromQuery] string returnUrl)
        {

            var redirectUrl = Url.Action("ExternalLoginCallback", "Authentication", new { returnUrl }, Request.Scheme);

            if (redirectUrl == null)
                return BadRequest("Cannot create redirect url");

            AuthenticationProperties propertes = _signInManager.ConfigureExternalAuthenticationProperties(EnProvider.Google.ToString(), redirectUrl);

            //HTTP 302 Redirect توجيه المستخدم لموقع تسجيل الدخول الخاص بال provider
            return Challenge(propertes, EnProvider.Google.ToString());
        }



        [HttpGet("external-login-callback", Name = "ExternalLoginCallback")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<ExternalLoginCallbackDto>> ExternalLoginCallback([FromQuery] string returnUrl, [FromQuery] string? remoteError)
        {
            if (string.IsNullOrEmpty(returnUrl))
                return BadRequest("ReturnUrl cannot be null or empty");

            if (!string.IsNullOrEmpty(remoteError))
                return BadRequest($"Error from external provider: {remoteError}");

            try
            {
                //Login
                var userDto = await _userService.LoginByProviderAsync(Role.Customer);
                if (userDto == null)
                    return Unauthorized("Cannot login by provider");

                //get user roles
                var userRoles = await _userService.GetAllUserRolesByIdAsync(userDto.Id);
                if (userRoles is null) return BadRequest("Invaild get user roles");

                //convert userRoles to string of user roles
                var userRolesString = new List<string>();
                foreach (var role in userRoles) userRolesString.Add(role.Name);

                //jwt
                var token = _tokenService.GenerateJwtToken(userDto.Id, userDto.Email, userRolesString);
                var refreshToken = await _tokenService.AddNewRefreshTokenByUserIdAsync(userDto.Id);

                var tokenDto = new TokenDto { Token = token, RefreshToken = refreshToken };
                var externalLoginCallbackDto = new ExternalLoginCallbackDto
                {
                    returnUrl = returnUrl,
                    tokenDto = tokenDto
                };

                return Ok(externalLoginCallbackDto);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("logout",Name ="Logout")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> Logout()
        {
            try
            {
                var userId = Helper.GetIdFromClaimsPrincipal(User);
                if (string.IsNullOrEmpty(userId)) return Unauthorized();

                var IsLogout = await _tokenService.RemoveAllUserRefrechTokensByUserIdAsync(userId);

                if (!IsLogout)
                    return NotFound("Not found any refresh token.");


                return Ok("Logout successfuly.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            } 
        }
    }
}
