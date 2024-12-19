using DataAccessLayer.Contracks;
using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using DataAccessLayer.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ProductRepository> _logger;

        public ProductRepository(AppDbContext context, ILogger<ProductRepository> logger) : base(context, logger, "Products")
        {
            this._context = context;
            this._logger = logger;
        }

        public async Task<Product> GetByNameArAsync(string NameAr)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(NameAr, nameof(NameAr));

            try
            {
                var product = await _context.Products.
                    FirstOrDefaultAsync(e => e.NameAr == NameAr);
                return product;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }

        public async Task<Product> GetByNameEnAsync(string NameEn)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(NameEn, nameof(NameEn));

            try
            {
                var product = await _context.Products.
                    FirstOrDefaultAsync(e => e.NameEn == NameEn);
                return product;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }

    }
}
