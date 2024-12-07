using DataAccessLayer.Contracks;
using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using DataAccessLayer.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DataAccessLayer.Repositories
{
    public class CityRepository : GenericRepository<City>, ICityRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CityRepository> _logger;
        public CityRepository(AppDbContext context, ILogger<CityRepository> logger) : base(context, logger,"Cites")
        {
            _context = context;
            _logger = logger;
        }

        public async Task DeleteByNameArAsync(string cityNameAr)
        {
            if (string.IsNullOrEmpty(cityNameAr)) throw new ArgumentException("CityNameAr cannot be null or empty");

            try
            {
                var city = await _context.Cities.FirstOrDefaultAsync(c => c.NameAr == cityNameAr);

                if (city == null)
                    return;

                _context.Cities.Remove(city);
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }

        public async Task DeleteByNameEnAsync(string cityNameEn)
        {
            if (string.IsNullOrEmpty(cityNameEn)) throw new ArgumentException("CityNameEn cannot be null or empty");

            try
            {
                var city = await _context.Cities.FirstOrDefaultAsync(c => c.NameEn == cityNameEn);

                if (city == null)
                    return;

                _context.Cities.Remove(city);
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }

        public async Task<City> GetByNameArAsync(string cityNameAr)
        {
            if (string.IsNullOrWhiteSpace(cityNameAr)) throw new ArgumentException("CityNameAr cannot be null or empty");

            try
            {
                var city = await _context.Cities.FirstOrDefaultAsync(c => c.NameAr == cityNameAr);

                return city;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }

        public async Task<City> GetByNameEnAsync(string cityNameEn)
        {
            if (string.IsNullOrWhiteSpace(cityNameEn)) throw new ArgumentException("CityNameEn cannot be null or empty");

            try
            {
                var city = await _context.Cities.FirstOrDefaultAsync(c => c.NameEn == cityNameEn);

                return city;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }

        public async Task DeleteAsync(long Id)
        {

            ParamaterException.CheckIfLongIsBiggerThanZero(Id, nameof(Id));

            try
            {
                var entity = await GetByIdAsTrackingAsync(Id);

                if (entity is null)
                    return;

                entity.IsDeleted = true;
                entity.DateOfDelete = DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }

        public async Task DeleteRangeAsync(IEnumerable<long> Ids)
        {
            ParamaterException.CheckIfIEnumerableIsNotNullOrEmpty(Ids, nameof(Ids));

            try
            {
                foreach (long Id in Ids)
                {
                    await DeleteAsync(Id);
                }
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }
    }
}
