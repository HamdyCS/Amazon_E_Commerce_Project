using DataAccessLayer.Contracks;
using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using DataAccessLayer.Enums;
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
    public class ApplicationTypeRepository : GenericRepository<ApplicationType>, IApplicationTypeRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ApplicationTypeRepository> _logger;

        public ApplicationTypeRepository(AppDbContext context, ILogger<ApplicationTypeRepository> logger) : base(context, logger, "ApplicationTypes")
        {
            this._context = context;
            this._logger = logger;
        }

        public async Task<ApplicationType> GetByEnApplicationTypeAsync(EnApplicationType enApplicationType)
        {
            ParamaterException.CheckIfObjectIfNotNull(enApplicationType, nameof(enApplicationType));
            try
            {
                var ApplicationTypeId = (long)enApplicationType;

                var applicationType = await _context.ApplicationTypes.FirstOrDefaultAsync(e=>e.Id == ApplicationTypeId);
                return applicationType;

            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }
    }
}
