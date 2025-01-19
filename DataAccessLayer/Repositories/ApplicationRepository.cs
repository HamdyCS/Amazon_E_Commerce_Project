using DataAccessLayer.Contracks;
using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using DataAccessLayer.Enums;
using DataAccessLayer.Exceptions;
using DataAccessLayer.Identity.Entities;
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

        public async Task<IEnumerable<Application>> GetAllReturnApplicationsAsync()
        {
            try
            {
                var applicationsList = await _context.Applications.Where(e => e.ApplicationTypeId == (long)EnApplicationType.Return).ToListAsync();
                return applicationsList;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
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

        public async Task<IEnumerable<Application>> GetAllUserReturnApplicationsByUserIdAsync(string UserId)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(UserId,nameof(UserId));
            try
            {
                var applicationsList = await _context.Applications.
                    Where(e => e.ApplicationTypeId == (long)EnApplicationType.Return&& e.UserId==UserId).ToListAsync();
                return applicationsList;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }

        public async Task<Application> GetByIdAndUserIdAsync(long ApplicationId, string userId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(ApplicationId, nameof(ApplicationId));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(userId, nameof(userId));

            try
            {
                var application = await _context.Applications.FirstOrDefaultAsync(e => e.Id==ApplicationId && e.UserId == userId);
                return application;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }
    }
}
