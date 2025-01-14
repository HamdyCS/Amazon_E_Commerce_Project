using DataAccessLayer.Contracks;
using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using DataAccessLayer.Enums;
using DataAccessLayer.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataAccessLayer.Repositories
{
    public class ApplicationOrderTypeRepository : GenericRepository<ApplicationOrderType>, IApplicationOrderTypeRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ApplicationOrderTypeRepository> _logger;

        public ApplicationOrderTypeRepository(AppDbContext context, ILogger<ApplicationOrderTypeRepository> logger) : base(context, logger, "ApplicationOrdersTypes")
        {
            this._context = context;
            this._logger = logger;
        }

        public async Task<ApplicationOrderType> GetByEnApplicationOrderTypeAsync(EnApplicationOrderType enApplicationOrderType)
        {
            ParamaterException.CheckIfObjectIfNotNull(enApplicationOrderType, nameof(enApplicationOrderType));
            try
            {
                var ApplicationOrderTypeId = (long)enApplicationOrderType;

                var applicationOrderType = await _context.ApplicationOrdersTypes.FirstOrDefaultAsync(e => e.Id == ApplicationOrderTypeId);
                return applicationOrderType;

            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }
    }
}
