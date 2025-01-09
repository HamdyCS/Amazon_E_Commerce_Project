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
    public class CityWhereDeliveyWorkRepository : GenericRepository<CityWhereDeliveryWork>, ICityWhereDeliveyWorkRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CityWhereDeliveyWorkRepository> _logger;

        public CityWhereDeliveyWorkRepository(AppDbContext context, ILogger<CityWhereDeliveyWorkRepository> logger) : base(context, logger, "CitiesWhereDeliveiesWorks")
        {
            this._context = context;
            this._logger = logger;
        }

        public async Task DeleteAllCitiesWhereDeliveryWorkByDeliveryIdAsync(string DeliveryId)
        {
            try
            {

                var citiesWhereDeliveryWorkList = await GetAllCitiesWhereDeliveryWorkByIdAsync(DeliveryId);
                if (citiesWhereDeliveryWorkList is null || !citiesWhereDeliveryWorkList.Any()) return;

                _context.CitiesWhereDeliveiesWorks.RemoveRange(citiesWhereDeliveryWorkList);

                return;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }

        public async Task<IEnumerable<CityWhereDeliveryWork>> GetAllCitiesWhereDeliveryWorkByIdAsync(string DeliveryId)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(DeliveryId, nameof(DeliveryId));
            try
            {
                var citiesWhereDeliveryWorkList = await _context.CitiesWhereDeliveiesWorks.
                    Where(e => e.DeliveryId == DeliveryId).ToListAsync();

                return citiesWhereDeliveryWorkList;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }

        public async Task<CityWhereDeliveryWork> GetByIdAndDeliveryIdAsync(long Id, string DeliveryId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(Id, nameof(Id));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(DeliveryId, nameof(DeliveryId));

            try
            {
                var cityWhereDeliveryWork = await _context.CitiesWhereDeliveiesWorks.
                    FirstOrDefaultAsync(e=>e.Id == Id && e.DeliveryId == DeliveryId);

                return cityWhereDeliveryWork;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }

        public async Task<bool> IsThisUserWorkInThisCityAsync(long cityId, string DeliveryId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(cityId, nameof(cityId));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(DeliveryId, nameof(DeliveryId));

            try
            {
                var cityWhereDeliveryWork = await _context.CitiesWhereDeliveiesWorks.
                    FirstOrDefaultAsync(e => e.CityId == cityId && e.DeliveryId == DeliveryId);

                return cityWhereDeliveryWork is not null;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }
    }
}
