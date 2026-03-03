using BusinessLayer.Contracks;
using BusinessLayer.Exceptions;
using BusinessLayer.Options;
using DataAccessLayer.Entities;
using DataAccessLayer.Identity.Entities;
using DataAccessLayer.UnitOfWork.Contracks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BusinessLayer.Servicese
{
    public class TokenService : ITokenService
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<TokenService> _logger;
        private readonly JwtOptions _jwtOptions;

        public TokenService(IUserService userService, IConfiguration configuration, IUnitOfWork unitOfWork, ILogger<TokenService> logger, JwtOptions jwtOptions)
        {
            _userService = userService;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _jwtOptions = jwtOptions;
        }

        private async Task<bool> _CompleteAsync()
        {
            try
            {
                var RowsAffeced = await _unitOfWork.CompleteAsync();

                return RowsAffeced > 0;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private string _GenerateRefreshToken()
        {
            var RandomNumber = new byte[32];

            using (var random = RandomNumberGenerator.Create())
            {
                random.GetBytes(RandomNumber);
            }

            return Convert.ToBase64String(RandomNumber);
        }

        public async Task<string> AddNewRefreshTokenByUserIdAsync(string UserId)
        {
            if (string.IsNullOrEmpty(UserId)) throw new ArgumentException("Cannot be null or empty", nameof(UserId));

            try
            {
                var userDto = await _userService.FindByIdAsync(UserId);

                if (userDto is null) return null;

                var NewRefreshTokenString = _GenerateRefreshToken();

                //get the refresh token life time in days from configuration
                double days = double.TryParse(_configuration["JwtRefreshToken:LifeTimeDays"], out double result) ? result : 0;


                var refreshToken = new RefreshToken
                {
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddDays(days),
                    UserId = UserId,
                    Token = NewRefreshTokenString
                };

                await _unitOfWork.refreshTokenRepository.AddAsync(refreshToken);

                await _CompleteAsync();

                return NewRefreshTokenString;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<bool> CheckIfRefreshTokenIsActiveByUserIdAsync(string UserId, string RefreshTokenString)
        {
            if (string.IsNullOrEmpty(UserId)) throw new ArgumentException("Cannot be null or empty", nameof(UserId));
            if (string.IsNullOrEmpty(UserId)) throw new ArgumentException("Cannot be null or empty", nameof(UserId));

            try
            {
                var refreshToken = await _unitOfWork.refreshTokenRepository.GetRefreshTokenByTokenAsync(RefreshTokenString);

                return (refreshToken != null && refreshToken.UserId == UserId && refreshToken.IsActive);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string GenerateJwtToken(string UserId, string? Email, IEnumerable<string> roles)
        {
            if (string.IsNullOrEmpty(UserId)) throw new ArgumentException("Cannot be null", nameof(UserId));
            ParamaterException.CheckIfIEnumerableIsNotNullOrEmpty(roles, nameof(roles));

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new();

            var claims = new List<Claim>()
                        {
                            new Claim(ClaimTypes.NameIdentifier, UserId),
                        };

            if (!string.IsNullOrEmpty(Email)) claims.Add(new Claim(ClaimTypes.Email, Email));


            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            SecurityTokenDescriptor securityTokenDescriptor = new()
            {
                Issuer = _jwtOptions.Issuar,
                Audience = _jwtOptions.Audience,

                Expires = DateTime.UtcNow.AddMinutes(_jwtOptions.LifeTimeMin),

                SigningCredentials = new
                    (
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SigningKey)),
                        SecurityAlgorithms.HmacSha256

                    ),

                EncryptingCredentials = new EncryptingCredentials
                    (
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.EncryptionKey.Substring(0, 16))),
                        SecurityAlgorithms.Aes128KW,
                        SecurityAlgorithms.Aes128CbcHmacSha256

                    ),



                Subject = new ClaimsIdentity
                    (
                       claims
                    )
            };

            var Token = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);

            var TokenString = jwtSecurityTokenHandler.WriteToken(Token);

            return TokenString;
        }

        public async Task<bool> RemoveAllUserRefrechTokensByUserIdAsync(string userId)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(userId, nameof(userId));


            //remove all user refreshTokens
            await _unitOfWork.refreshTokenRepository.DeleteAllUserRefreshTokensByUserId(userId);
            var isRefreshTokensDeleted = await _CompleteAsync();

            return isRefreshTokensDeleted;
        }

        public async Task<bool> CheckIfRefreshTokenIsValidAsync(string refreshToken)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(refreshToken, nameof(refreshToken));

            var RefreshToken = await _unitOfWork.refreshTokenRepository.GetRefreshTokenByTokenAsync(refreshToken);

            //check if the refresh token exists and is active
            if (refreshToken != null && RefreshToken.IsActive)
            {
                return true;
            }

            return false;
        }

        public void AddAuthInfoToCookie(HttpResponse httpResponse, string token, string? refreshToken = null)
        {
            //get the refresh token life time in days from configuration
            double days = double.TryParse(_configuration["JwtRefreshToken:LifeTimeDays"], out double result) ? result : 0;

            var cookie = new Cookie();
            var cookieOptions = new CookieOptions()
            {
                Secure = true,
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(days),
                SameSite = SameSiteMode.None,//ليس مسار الفرونت يساوي مسار الباك
                Path = "/"
                
            };

            //append to cookie
            httpResponse.Cookies.Append("access_token", token, cookieOptions);
            if (refreshToken != null)
            {
                httpResponse.Cookies.Append("refresh_token", refreshToken, cookieOptions);
            }
        }
    }
}
