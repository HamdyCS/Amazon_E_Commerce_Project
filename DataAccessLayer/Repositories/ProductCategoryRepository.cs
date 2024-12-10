using DataAccessLayer.Contracks;
using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using DataAccessLayer.Exceptions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class ProductCategoryRepository : GenericRepository<ProductCategory>, IProductCategoryRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ProductCategoryRepository> _logger;

        public ProductCategoryRepository(AppDbContext context, ILogger<ProductCategoryRepository> logger) : base(context, logger, "ProductCategories")
        {
            this._context = context;
            this._logger = logger;
        }

        public async Task<ProductCategory> GetByNameArAsync(string NameAr)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(NameAr, nameof(NameAr));

            try
            {
                var productCategory =  await _context.ProductCategories.
                    FirstOrDefaultAsync(e=>e.NameEn==NameAr);
                return productCategory;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }

        public async Task<ProductCategory> GetByNameEnAsync(string NameEn)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(NameEn, nameof(NameEn));

            try
            {
                var productCategory = await _context.ProductCategories.
                    FirstOrDefaultAsync(e => e.NameEn == NameEn);
                return productCategory;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }


    }
}
