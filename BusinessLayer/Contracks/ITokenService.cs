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
        public Task<string> GenerateToken(UserDto userDto);

        public Task<string> GenerateRefreshToken();
    }
}
