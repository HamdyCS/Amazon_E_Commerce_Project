using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Contracks
{
    public interface IRefreshTokenRepository : IGenericRepository<RefreshToken>
    {
        Task<RefreshToken> GetRefreshTokenByTokenAsync(string Token);

        //bool CheckIfRefreshTokenIsActiveAsync(RefreshToken refreshToken);
    }
}
