using DataAccessLayer.Contracks;
using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using DataAccessLayer.Exceptions;
using DataAccessLayer.Pagination;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class SellerProductRepository : GenericRepository<SellerProduct>, ISellerProductRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<SellerProductRepository> _logger;
        public SellerProductRepository(AppDbContext context, ILogger<SellerProductRepository> logger) : base(context, logger, "SellerProducts")
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<SellerProduct>> GetAllByProductIdOrderByPriceAscAsync(long ProductId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(ProductId, nameof(ProductId));
            try
            {
                var sellerProductsList = await _context.SellerProducts.AsNoTracking().
                    Include(s => s.Product).ThenInclude(p => p.ProductImages).Include(s => s.Product).
                    ThenInclude(p => p.ProductReviews)
                    .Where(e => e.ProductId == ProductId).OrderBy(e => e.Price).AsSplitQuery().ToListAsync();
                return sellerProductsList;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }

        public async Task<IEnumerable<SellerProduct>> GetAllSellerProductsBySellerIdAsync(string sellerId)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(sellerId, nameof(sellerId));

            try
            {
                var sellerProductsList = await _context.SellerProducts
                    .Where(e => e.SellerId == sellerId).
                    Include(s => s.Product).ThenInclude(p => p.ProductImages)
                    .Include(s => s.Product).
                    ThenInclude(p => p.ProductReviews).AsSplitQuery().ToListAsync();
                return sellerProductsList;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }

        public async Task<IEnumerable<SellerProduct>> GetPagedDataAsNoTrackingByProductIdAsync(int pageNumber, int pageSize, long productId)
        {
            ParamaterException.CheckIfIntIsBiggerThanZero(pageNumber, nameof(pageNumber));
            ParamaterException.CheckIfIntIsBiggerThanZero(pageSize, nameof(pageSize));
            ParamaterException.CheckIfLongIsBiggerThanZero(productId, nameof(productId));
            try
            {
                var sellerProductsList = await _context.SellerProducts.AsNoTracking()
                    .Include(s => s.Product).ThenInclude(p => p.ProductImages)
                    .Include(s => s.Product).
                    ThenInclude(p => p.ProductReviews)
                    .Where(e => e.ProductId == productId)
                    .Skip(pageSize * (pageNumber - 1)).Take(pageSize).AsSplitQuery().ToListAsync();
                return sellerProductsList;

            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);

            }
        }

        public async Task<SellerProduct> GetSellerProductByIdAndSellerIdAsync(long sellerProductId, string sellerId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(sellerProductId, nameof(sellerProductId));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(sellerId, nameof(sellerId));

            try
            {
                var sellerProduct = await _context.SellerProducts.FirstOrDefaultAsync(e => e.Id == sellerProductId
                && e.SellerId == sellerId);

                return sellerProduct;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }

        }

        public async Task<SellerProduct> GetTheCheaperSellerProductByProductIdAsync(long ProductId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(ProductId, nameof(ProductId));

            try
            {
                var sellerProduct = await _context.SellerProducts.AsNoTracking().
                    Where(e => e.ProductId == ProductId).
                    OrderBy(e => e.Price).FirstOrDefaultAsync();
                return sellerProduct;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }

        public override async Task<PaginationResult<SellerProduct>> GetPaginatedDataAsync(int pageNumber, int pageSize)
        {

            ParamaterException.CheckIfIntIsBiggerThanZero(pageNumber, nameof(pageNumber));
            ParamaterException.CheckIfIntIsBiggerThanZero(pageSize, nameof(pageSize));

            try
            {
                var TotalCount = await _context.SellerProducts.CountAsync();
                var data = await _context.SellerProducts.Include(s => s.Product).
                    ThenInclude(p => p.ProductImages).Include(s => s.Product).
                    ThenInclude(p => p.ProductReviews).OrderBy(s => s.Id).Skip((pageNumber - 1) * pageSize).
                    Take(pageSize).AsSplitQuery().ToListAsync();

                return new PaginationResult<SellerProduct>(data, TotalCount, pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);

            }

        }

        public override async Task<SellerProduct> GetByIdAsTrackingAsync(long id)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(id, nameof(id));
            try
            {
                var sellerProduct = await _context.SellerProducts.Include(s => s.Product).
                    ThenInclude(p => p.ProductImages).Include(s => s.Product).
                    ThenInclude(p => p.ProductReviews).
                    FirstOrDefaultAsync(e => e.Id == id);

                return sellerProduct;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }

        }

        public async Task<PaginationResult<SellerProduct>> GetPagedByProductSubCategoryIdAsync(long productSubCategoryId, int pageNumber, int pageSize)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(productSubCategoryId, nameof(productSubCategoryId));
            ParamaterException.CheckIfIntIsBiggerThanZero(pageNumber, nameof(pageNumber));
            ParamaterException.CheckIfIntIsBiggerThanZero(pageSize, nameof(pageSize));

            try
            {
                var query = _context.SellerProducts.
                    Where(s => s.Product.ProductSubCategoryId == productSubCategoryId);

                var totalCount = await query.CountAsync();

                if (totalCount == 0)
                    return new PaginationResult<SellerProduct>([], totalCount, pageNumber, pageSize); ;

                var data = await query.Include(s => s.Product).
                    ThenInclude(p => p.ProductImages).OrderBy(s => s.Id).Include(s => s.Product).
                    ThenInclude(p => p.ProductReviews)
                    .Skip((pageNumber - 1) * pageSize).
                    Take(pageSize).AsSplitQuery().ToListAsync();


                return new PaginationResult<SellerProduct>(data, totalCount, pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }

        }

        public async Task<PaginationResult<SellerProduct>> GetPagedByProductCategoryIdAsync(long productCategoryId, int pageNumber, int pageSize)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(productCategoryId, nameof(productCategoryId));
            ParamaterException.CheckIfIntIsBiggerThanZero(pageNumber, nameof(pageNumber));
            ParamaterException.CheckIfIntIsBiggerThanZero(pageSize, nameof(pageSize));

            try
            {
                var query = _context.SellerProducts.
                    Where(s => s.Product.ProductSubCategory.ProductCategoryId == productCategoryId);

                var totalCount = await query.CountAsync();

                if (totalCount == 0)
                    return new PaginationResult<SellerProduct>([], totalCount, pageNumber, pageSize); ;

                var data = await query.Include(s => s.Product).ThenInclude(p => p.ProductImages).
                    Include(s => s.Product).
                    ThenInclude(p => p.ProductReviews).
                    OrderBy(s => s.Id)
                    .Skip((pageNumber - 1) * pageSize).
                    Take(pageSize).AsSplitQuery().ToListAsync();


                return new PaginationResult<SellerProduct>(data, totalCount, pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }

        }

        public async Task<PaginationResult<SellerProduct>> GetPagedByBrandIdAsync(long brandId, int pageNumber, int pageSize)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(brandId, nameof(brandId));
            ParamaterException.CheckIfIntIsBiggerThanZero(pageNumber, nameof(pageNumber));
            ParamaterException.CheckIfIntIsBiggerThanZero(pageSize, nameof(pageSize));

            try
            {
                var query = _context.SellerProducts.
                    Where(s => s.Product.BrandId == brandId);

                var totalCount = await query.CountAsync();

                if (totalCount == 0)
                    return new PaginationResult<SellerProduct>([], totalCount, pageNumber, pageSize); ;

                var data = await query.Include(s => s.Product).ThenInclude(p => p.ProductImages).
                    OrderBy(s => s.Id)
                    .Skip((pageNumber - 1) * pageSize).
                    Take(pageSize).AsSplitQuery().ToListAsync();


                return new PaginationResult<SellerProduct>(data, totalCount, pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }

        }

        public async Task<PaginationResult<SellerProduct>> SearchByProductNameAsync(string query, int pageNumber, int pageSize)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(query, nameof(query));

            try
            {
                var sqlQuery = _context.SellerProducts.Where(sp =>
                sp.Product.NameEn.Contains(query) || sp.Product.NameAr.Contains(query));

                var count = await sqlQuery.CountAsync();

                //not found any result
                if (count == 0)
                {
                    sqlQuery = _context.SellerProducts;
                    count = await sqlQuery.CountAsync();

                    var result = await sqlQuery.
                        Include(sp => sp.Product).ThenInclude(p => p.ProductImages).Include(s => s.Product).
                    ThenInclude(p => p.ProductReviews).OrderBy(sp => sp.Id).Skip((pageNumber - 1) * pageSize).
                    Take(pageSize).AsSplitQuery().ToListAsync();

                    return new PaginationResult<SellerProduct>(result, count, pageNumber, pageSize);
                }

                var data = await sqlQuery.
                       Include(sp => sp.Product).ThenInclude(p => p.ProductImages).Include(s => s.Product).
                    ThenInclude(p => p.ProductReviews).OrderByDescending(sp => sp.Id).Skip((pageNumber - 1) * pageSize).
                   Take(pageSize).AsSplitQuery().ToListAsync();


                return new PaginationResult<SellerProduct>(data, count, pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }

        public async Task UpdateStocksAsync(Dictionary<long, int> sellerProductsIdAndQuantities)
        {
            try
            {
                var sellerProductIds = sellerProductsIdAndQuantities.Keys.ToList();

                //get the seller products that match the provided IDs
                var sellerProducts = await _context.SellerProducts.Where(sp =>
                sellerProductIds.Any(id => id == sp.Id)
                ).ToListAsync();

                if (sellerProducts.Count <= 0)
                    throw new InvalidOperationException("No seller products found.");

                // Update the stock for each seller product

                foreach (var sellerProduct in sellerProducts)
                {
                    var quantityToSubtract = sellerProductsIdAndQuantities[sellerProduct.Id];

                    if (quantityToSubtract < 0)
                        throw new InvalidOperationException("Quantity to subtract cannot be negative.");
                    if (sellerProduct.NumberInStock < quantityToSubtract)

                        throw new InvalidOperationException($"Not enough stock for seller product with ID {sellerProduct.Id}.");
                    sellerProduct.NumberInStock -= quantityToSubtract;


                }

                _context.SellerProducts.UpdateRange(sellerProducts);
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }

        }


        //public async Task<IEnumerable<SellerProductReview>> GetSellerProductReviewsBySellerProductIdAsync(long SellerProductId)
        //{
        //    try
        //    {
        //        var sellerProduct = await _context.SellerProducts.AsNoTracking().
        //            Include(e=>e.SellerProductReviews).FirstOrDefaultAsync(e=>e.Id == SellerProductId);

        //        return sellerProduct is null ? null : sellerProduct.SellerProductReviews;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw HandleDatabaseException(ex);
        //    }
        //}


    }
}
