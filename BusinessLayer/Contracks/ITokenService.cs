using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Dtos;

namespace BusinessLayer.Contracks
{
    public interface ITokenService
    {
        public string GenerateJwtToken(string UserId,string Email,IEnumerable<string> roles);

        public Task<string> AddNewRefreshTokenByUserIdAsync(string UserId);

        public Task<bool> CheckIfRefreshTokenIsActiveByUserIdAsync(string UserId,string RefreshTokenString);

        //public Task<string> CheckIfRefreshTokenIsActiveByEmailAsync(string Email, string RefreshTokenString);

    }
}
