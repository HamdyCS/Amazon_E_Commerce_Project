using BusinessLayer.Dtos;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Contracks
{
    public interface ITokenService
    {
        public string GenerateJwtToken(string UserId,string Email,IEnumerable<string> roles);

        public Task<string> AddNewRefreshTokenByUserIdAsync(string UserId);

        public Task<bool> CheckIfRefreshTokenIsActiveByUserIdAsync(string UserId,string RefreshTokenString);
        Task<bool> RemoveAllUserRefrechTokensByUserIdAsync(string userId);
        Task<bool> CheckIfRefreshTokenIsValidAsync(string refreshToken);

        public void AddAuthInfoToCookie(HttpResponse httpResponse, string token, string? refreshToken = null);
       
        //public Task<string> CheckIfRefreshTokenIsActiveByEmailAsync(string Email, string RefreshTokenString);

    }
}
