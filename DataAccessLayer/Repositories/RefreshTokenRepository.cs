using DataAccessLayer.Contracks;
using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class RefreshTokenRepository : GenericRepository<RefreshToken>, IRefreshTokenRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<RefreshTokenRepository> _logger;

        public RefreshTokenRepository(AppDbContext context, ILogger<RefreshTokenRepository> logger) : base(context, logger, "RefreshTokens")
        {
            _context = context;
            _logger = logger;
        }

        //public bool CheckIfRefreshTokenIsActiveAsync(RefreshToken refreshToken)
        //{
        //    if (refreshToken == null) throw new ArgumentNullException("Cannot be null", nameof(refreshToken));

        //    try
        //    {
        //        return refreshToken.IsActive;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw HandleDatabaseException(ex, TableName);
        //    }
        //}

        public async Task<RefreshToken> GetRefreshTokenByTokenAsync(string Token)
        {
            if (string.IsNullOrEmpty(Token)) throw new ArgumentException("Cannot be null", nameof(Token));

            try
            {
                var refreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == Token);

                return refreshToken;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }
    }
}
