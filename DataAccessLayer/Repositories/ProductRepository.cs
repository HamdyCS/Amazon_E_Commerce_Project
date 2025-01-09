using DataAccessLayer.Contracks;
using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using DataAccessLayer.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using static Azure.Core.HttpHeader;

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

        public async Task<IEnumerable<Product>> SearchByNameArAsync(string NameAr, int pageSize)
        {

            try
            {
                //var products = await _context.Products.FromSqlInterpolated($"select * from Products where Name_Ar like N'%{NameAr}%' order by Name_Ar Offset 0 rows fetch next {pageSize} rows only")
                //   .ToListAsync();

                var products = await _context.Products.Where(e => e.NameAr.Contains(NameAr)).Take(pageSize)
                    .ToListAsync();
                return products;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }

        public async Task<IEnumerable<Product>> SearchByNameEnAsync(string NameEn, int pageSize)
        {

            try
            {
                //var products = await _context.Products.FromSqlInterpolated($"select * from Products where Name_En like N'%{NameEn}%'")
                //    .ToListAsync();

                var products = await _context.Products.Where(e=>e.NameEn.Contains(NameEn)).Take(pageSize)
                    .ToListAsync();
                return products;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }
    }
}
