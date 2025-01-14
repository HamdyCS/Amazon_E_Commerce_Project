using DataAccessLayer.Contracks;
using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using DataAccessLayer.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class ApplicationRepository : GenericRepository<Application>, IApplicationRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ApplicationRepository> _logger;

        public ApplicationRepository(AppDbContext context, ILogger<ApplicationRepository> logger) : base(context, logger, "Applications")
        {
            this._context = context;
            this._logger = logger;
        }

        public async Task<IEnumerable<Application>> GetAllUserApplicationsByUserIdAsync(string UserId)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(UserId, nameof(UserId));

            try
            {
                var applicationsList = await _context.Applications.Where(e => e.UserId == UserId).ToListAsync();
                return applicationsList;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }


    }
}
