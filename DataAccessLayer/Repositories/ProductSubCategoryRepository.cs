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
    public class ProductSubCategoryRepository : GenericRepository<ProductSubCategory>, IProductSubCategoryRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ProductSubCategoryRepository> _logger;

        public ProductSubCategoryRepository(AppDbContext context, ILogger<ProductSubCategoryRepository> logger) : base(context, logger, "ProductSubCategories")
        {
            this._context = context;
            this._logger = logger;
        }

        public async Task<IEnumerable<ProductSubCategory>> GetAllByProductCategoryIdAsync(long ProductCategoryId)
        {
           ParamaterException.CheckIfLongIsBiggerThanZero(ProductCategoryId, nameof(ProductCategoryId));

            try
            {
                var productCategory = await _context.ProductCategories.Include(e=>e.ProductSubCategories)
                    .FirstOrDefaultAsync(e=>e.Id == ProductCategoryId);

                if (productCategory == null) return null;

                return productCategory.ProductSubCategories;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }

        public async Task<ProductSubCategory> GetByNameArAsync(string NameAr)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(NameAr, nameof(NameAr));

            try
            {
                var productSubCategory = await _context.ProductSubCategories.
                    FirstOrDefaultAsync(e => e.NameAr == NameAr);
                return productSubCategory;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }

        public async Task<ProductSubCategory> GetByNameEnAsync(string NameEn)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(NameEn, nameof(NameEn));

            try
            {
                var productSubCategory = await _context.ProductSubCategories.
                    FirstOrDefaultAsync(e => e.NameEn == NameEn);
                return productSubCategory;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }
    }
}
