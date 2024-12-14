using DataAccessLayer.Contracks;
using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using DataAccessLayer.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;


namespace DataAccessLayer.Repositories
{
    public class ProductImageRepository : GenericRepository<ProductImage>, IProductImageRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ProductImageRepository> _logger;

        public ProductImageRepository(AppDbContext context, ILogger<ProductImageRepository> logger) : base(context, logger, "ProductImages")
        {
            this._context = context;
            this._logger = logger;
        }

        public async Task DeleteAllProductImagesByProductIdAsync(long productId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(productId, nameof(productId));

            try
            {
                var productImages = await _context.ProductImages.Where
                    (e => e.ProductId == productId).ToListAsync();


                if (productImages is null || !productImages.Any()) return;

                _context.ProductImages.RemoveRange(productImages);
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }

        public async Task<IEnumerable<ProductImage>> GetAllProductProductIdAsync(long productId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(productId, nameof(productId));

            try
            {

                var products = await _context.Products.AsNoTracking()
                    .Include(e => e.ProductImages).FirstOrDefaultAsync(e => e.Id == productId);


                if (products is null) return null;

                return products.ProductImages;

            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }
    }
}
