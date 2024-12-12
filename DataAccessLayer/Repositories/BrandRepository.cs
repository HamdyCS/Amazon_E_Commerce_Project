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
using static Azure.Core.HttpHeader;

namespace DataAccessLayer.Repositories
{
    public class BrandRepository : GenericRepository<Brand>, IBrandRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<BrandRepository> _logger;

        public BrandRepository(AppDbContext context, ILogger<BrandRepository> logger) : base(context, logger, "Brands")
        {
            this._context = context;
            this._logger = logger;
        }

        public async Task<Brand> GetByNameArAsync(string nameAr)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(nameAr, nameof(nameAr));

            try
            {
                var brand = await _context.Brands.FirstOrDefaultAsync(e=>e.NameAr== nameAr);
                return brand;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }

        public async Task<Brand> GetByNameEnAsync(string nameEn)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(nameEn, nameof(nameEn));

            try
            {
                var brand = await _context.Brands.FirstOrDefaultAsync(e => e.NameEn == nameEn);
                return brand;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }
    }
}
