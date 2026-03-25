using DataAccessLayer.Contracks;
using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using DataAccessLayer.Exceptions;
using DataAccessLayer.Identity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class ProductReviewRepository : GenericRepository<ProductReview>, IProductReviewRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ProductReviewRepository> _logger;

        public ProductReviewRepository(AppDbContext context, ILogger<ProductReviewRepository> logger) : base(context, logger, "ProductReviews")
        {
            this._context = context;
            this._logger = logger;
        }

        public async Task<bool> CheckIfThisUserReviewedThisProductByIdAndUserIdAsync(long Id, string UserId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(Id, nameof(Id));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(UserId, nameof(UserId));
            try
            {
                var productReview = await _context.ProductReview.FirstOrDefaultAsync(e=>e.Id == Id
                && e.UserId == UserId);

                return productReview != null;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }

        public async Task<IEnumerable<ProductReview>> GetAllProductReviewsByProductIdAsync(long productId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(productId, nameof(productId));

            try
            {
                var productReviewsList = await _context.ProductReview.AsNoTracking().
                    Where(e=>e.ProductId == productId).ToListAsync();

                return productReviewsList;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }

        public async Task<IEnumerable<ProductReview>> GetAllproductReviewsWithUserInfoByProductIdAsync(long productId)
        { 
            ParamaterException.CheckIfLongIsBiggerThanZero(productId, nameof(productId));

            try
            {
                var productReviewsList = await _context.ProductReview.AsNoTracking()
                    .Include(e=>e.user).
                    Where(e => e.ProductId == productId).ToListAsync();

                return productReviewsList;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }

     
        public async Task<double> GetAveragedOfStarsByProductIdAsync(long productId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(productId, nameof(productId));
            try
            {
                var Avg = await _context.ProductReview.AverageAsync(e => e.NumberOfStars);
                return Avg;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }

        public async Task<IEnumerable<ProductReview>> GetPagedProductReviewsWithUserInfoByProductIdAsync(int PageNumber,int PageSize,long productId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(productId, nameof(productId));
            ParamaterException.CheckIfIntIsBiggerThanZero(PageNumber, nameof(PageNumber));
            ParamaterException.CheckIfIntIsBiggerThanZero(PageSize, nameof(PageSize));

            try
            {
                var productReviewsList = await _context.ProductReview.AsNoTracking()
                    .Include(e => e.user).Where(e => e.ProductId == productId)
                    .Skip((PageNumber-1)*PageSize).Take(PageSize).ToListAsync();

                return productReviewsList;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }

   
        public async Task<ProductReview> GetProductReviewByIdAndUserIdAsync(long Id, string UserId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(Id, nameof(Id));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(UserId, nameof(UserId));

            try
            {
                var productReview = await _context.ProductReview.AsTracking().
                    FirstOrDefaultAsync(e => e.Id == Id && e.UserId == UserId);
                    

                return productReview;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }
    }
}
