﻿using BusinessLayer.Authentication;
using BusinessLayer.Contracks;
using DataAccessLayer.Entities;
using DataAccessLayer.UnitOfWork.Contracks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Servicese
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<TokenService> _logger;
        private readonly JwtOptions _jwtOptions;

        public TokenService(IConfiguration configuration, IUnitOfWork unitOfWork, ILogger<TokenService> logger, JwtOptions jwtOptions)
        {
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
                var NewRefreshTokenString = _GenerateRefreshToken();

                var refreshToken = new RefreshToken
                {
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddDays(double.Parse(_configuration["JwtRefreshToken:LifeTimeDays"])),
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

        public string GenerateJwtToken(string UserId, string Email)
        {
            if (string.IsNullOrEmpty(UserId)) throw new ArgumentException("Cannot be null", nameof(UserId));
            if (string.IsNullOrEmpty(Email)) throw new ArgumentException("Cannot be null", nameof(Email));

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new();

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
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SigningKey)),
                        SecurityAlgorithms.Aes128KW,
                        SecurityAlgorithms.Aes128CbcHmacSha256

                    ),

                Subject = new ClaimsIdentity
                    (
                        new List<Claim>()
                        {
                            new Claim(ClaimTypes.NameIdentifier, UserId),
                            new Claim(ClaimTypes.Email, Email)
                        }
                    ),
            };

            var Token = jwtSecurityTokenHandler.CreateJwtSecurityToken(securityTokenDescriptor);

            var TokenString = jwtSecurityTokenHandler.WriteToken(Token);

            return TokenString;
        }
    }
}