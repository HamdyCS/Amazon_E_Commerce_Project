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
                var sellerProductsList = await _context.SellerProducts.AsNoTracking()
                    .Where(e => e.ProductId == ProductId).OrderBy(e => e.Price).ToListAsync();
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
                var sellerProductsList = await _context.SellerProducts.Where(e => e.SellerId == sellerId).ToListAsync();
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
                var sellerProductsList = await _context.SellerProducts.AsNoTracking().Where(e=>e.ProductId == productId)
                    .Skip(pageSize*(pageNumber-1)).Take(pageSize).ToListAsync();
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
                var sellerProduct = await _context.SellerProducts.FirstOrDefaultAsync(e=>e.Id == sellerProductId
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
