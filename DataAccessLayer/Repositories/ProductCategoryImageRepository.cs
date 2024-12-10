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
    public class ProductCategoryImageRepository : GenericRepository<ProductCategoryImage>, IProductCategoryImageRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ProductCategoryImageRepository> _logger;

        public ProductCategoryImageRepository(AppDbContext context, ILogger<ProductCategoryImageRepository> logger) : base(context, logger, "ProductCategoryImages")
        {
            this._context = context;
            this._logger = logger;
        }

        public async Task<IEnumerable<ProductCategoryImage>> GetAllProductCategoryImagesByProductCategoryIdAsync(long productCategoryId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(productCategoryId,nameof(productCategoryId));

            try
            {
                var productCategoryImages = await _context.ProductCategories.AsNoTracking()
                    .Include(e => e.ProductCategoryImages)
                    .FirstOrDefaultAsync(e => e.Id == productCategoryId);

                return productCategoryImages.ProductCategoryImages;

            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }

        public async Task DeleteAllProductCategoryImagesByProductCategoryIdAsync(long productCategoryId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(productCategoryId, nameof(productCategoryId));

            try
            {
                var productCategoryImages = await _context.ProductCategoryImages.Where
                    (e=>e.ProductCategoryId == productCategoryId).ToListAsync();

                if (productCategoryImages is null || !productCategoryImages.Any()) return;

                _context.ProductCategoryImages.RemoveRange(productCategoryImages);
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }
    }
}
