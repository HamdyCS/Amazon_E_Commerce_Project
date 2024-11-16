using BusinessLayer.Contracks;
using BusinessLayer.Dtos;
using BusinessLayer.Help;
using BusinessLayer.Roles;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ApiLayer.Controllers
{
    [Route("api/Authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IOtpService _otpService;
        private readonly IMailService _mailService;
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public AuthenticationController(IOtpService otpService, IMailService mailService,
            IUserService userService, ITokenService tokenService)
        {
            _otpService = otpService;
            _mailService = mailService;
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpGet("IsEmailExist", Name = "IsEmailExist")]
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


        [HttpPost("SendOtp", Name = "SendOtpToEmail")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> SendOtpToEmail(string Email)
        {
            if (string.IsNullOrEmpty(Email)) return BadRequest("Email cannot be null or empty");

            try
            {
                var code = Helper.GenerateRandomSixDigitNumber();
                var otpDto = new OtpDto { Email = Email ,Code = code.ToString()};

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


        [HttpPost("RegisterAdminUser", Name = "RegisterAdminUser")]
        //[Authorize(Roles = Role.Admin)]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TokenDto>> RegisterAdminUser([FromBody] UserDto userDto, string Code)
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


        [HttpPost("RegisterCustomerUser", Name = "RegisterCustomerUser")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TokenDto>> RegisterCustomerUser([FromBody] UserDto userDto, string Code)
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



        [HttpPost("RegisterSellerUser", Name = "RegisterSellerUser")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TokenDto>> RegisterSellerUser([FromBody] UserDto userDto, string Code)
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


        [HttpPost("RegisterDeliveryAgentUser", Name = "RegisterDeliveryAgentUser")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TokenDto>> RegisterDeliveryAgentUser([FromBody] UserDto userDto, string Code)
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


        [HttpPost("Login", Name = "Login")]
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



        [HttpPost("ResetPassword", Name = "ResetPassword")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TokenDto>> ResetPassword([FromBody] string NewPassword, string Email, string Code)
        {
            if (string.IsNullOrEmpty(NewPassword) || string.IsNullOrEmpty(Code))
                return BadRequest("code or Newpassword cannot be null or empty");

            try
            {
                var IsResetedPassword = await _userService.ResetPasswordByEmailAsync(Email, NewPassword, Code);
                if (!IsResetedPassword) return BadRequest("Cannot reseted password");

                return Ok("Changed password successfuly");

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



        [HttpPost("UpdatePassword", Name = "UpdatePassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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


        [HttpGet("All", Name = "GetPagedData")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<IEnumerable<UserDto>>> GetPagedData([FromQuery] int pageNumber, [FromQuery] int pageSize)
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

        [HttpGet("Count", Name = "GetCountOfUsers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult> DeleteAccountById(string Id)
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


        [HttpPut("Email", Name = "UpdateEmail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateEmail([FromBody] string NewEmail, [FromQuery] string Code)
        {
            if (string.IsNullOrEmpty(NewEmail)) return BadRequest("New Email Cannot be null or empty");
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId is null) return BadRequest("User id is null");

                bool IsEmailUpdated = await _userService.UpdateEmailAsync(userId, NewEmail,Code);

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
        public async Task<ActionResult> UpdateUserInfo([FromBody] UserDto userDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId is null) return Unauthorized();

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

        [HttpGet("IsOtpValid",Name = "CheckIfOtpValid")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> CheckIfOtpValid(string Email,string Code)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
               

                bool IsOtpActiveAndNotUsed = await _otpService.CheckIfOtpActiveAndNotUsedAsync(new OtpDto { Email = Email,Code = Code});

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

    }


}
