using DataAccessLayer.Contracks;
using DataAccessLayer.Data;
using DataAccessLayer.Entities;
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

        public RefreshTokenRepository(AppDbContext context, ILogger<RefreshTokenRepository> logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }
    }
}
