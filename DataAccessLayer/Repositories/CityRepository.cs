using DataAccessLayer.Contracks;
using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class CityRepository : GenericRepository<City>, ICityRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CityRepository> _logger;
        private readonly string _TableName = "Cites";
        public CityRepository(AppDbContext context, ILogger<CityRepository> logger) : base(context, logger)
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
                {
                    _logger.LogInformation("Cannot find city by cityNameAr");
                    throw new Exception("Cannot find city by cityNameAr");
                }

                _context.Cities.Remove(city);
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex, _TableName);
            }
        }

        public async Task DeleteByNameEnAsync(string cityNameEn)
        {
            if (string.IsNullOrEmpty(cityNameEn)) throw new ArgumentException("CityNameEn cannot be null or empty");

            try
            {
                var city = await _context.Cities.FirstOrDefaultAsync(c => c.NameEn == cityNameEn);

                if (city == null)
                {
                    _logger.LogInformation("Cannot find city by cityNameEn");
                    throw new Exception("Cannot find city by cityNameEn");
                }

                _context.Cities.Remove(city);
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex, _TableName);
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
                throw HandleDatabaseException(ex, _TableName);
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
                throw HandleDatabaseException(ex, _TableName);
            }
        }
    }
}
