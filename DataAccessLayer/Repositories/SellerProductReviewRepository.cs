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
    public class SellerProductReviewRepository : GenericRepository<SellerProductReview>, ISellerProductReviewRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<SellerProductReviewRepository> _logger;

        public SellerProductReviewRepository(AppDbContext context, ILogger<SellerProductReviewRepository> logger) : base(context, logger, "SellerProductReviews")
        {
            this._context = context;
            this._logger = logger;
        }

        public async Task<bool> CheckIfThisUserReviewedThisSellerProductByIdAndUserIdAsync(long Id, string UserId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(Id, nameof(Id));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(UserId, nameof(UserId));
            try
            {
                var sellerProductReview = await _context.SellerProductReviews.FirstOrDefaultAsync(e=>e.Id == Id
                && e.UserId == UserId);

                return sellerProductReview != null;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }

        public async Task<IEnumerable<SellerProductReview>> GetAllSellerProductReviewsBySellerProductIdAsync(long sellerProductId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(sellerProductId, nameof(sellerProductId));

            try
            {
                var sellerProductReviewsList = await _context.SellerProductReviews.AsNoTracking().
                    Where(e=>e.SellerProductId == sellerProductId).ToListAsync();

                return sellerProductReviewsList;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }

        public async Task<IEnumerable<SellerProductReview>> GetAllSellerProductReviewsWithUserInfoBySellerProductIdAsync(long sellerProductId)
        { 
            ParamaterException.CheckIfLongIsBiggerThanZero(sellerProductId, nameof(sellerProductId));

            try
            {
                var sellerProductReviewsList = await _context.SellerProductReviews.AsNoTracking()
                    .Include(e=>e.user).
                    Where(e => e.SellerProductId == sellerProductId).ToListAsync();

                return sellerProductReviewsList;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }

        public async Task<double> GetAveragedOfStarsOfSellerProductBySellerProductIdAsync(long sellerProductId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(sellerProductId, nameof(sellerProductId));
            try
            {
                var Avg = await _context.SellerProductReviews.AverageAsync(e => e.NumberOfStars);
                return Avg;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }

        public async Task<IEnumerable<SellerProductReview>> GetPagedSellerProductReviewsWithUserInfoBySellerProductIdAsync(int PageNumber,int PageSize,long sellerProductId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(sellerProductId, nameof(sellerProductId));
            ParamaterException.CheckIfIntIsBiggerThanZero(PageNumber, nameof(PageNumber));
            ParamaterException.CheckIfIntIsBiggerThanZero(PageSize, nameof(PageSize));

            try
            {
                var sellerProductReviewsList = await _context.SellerProductReviews.AsNoTracking()
                    .Include(e => e.user).Where(e => e.SellerProductId == sellerProductId)
                    .Skip((PageNumber-1)*PageSize).Take(PageSize).ToListAsync();

                return sellerProductReviewsList;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }

        public async Task<SellerProductReview> GetSellerProductReviewByIdAndUserIdAsync(long Id, string UserId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(Id, nameof(Id));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(UserId, nameof(UserId));

            try
            {
                var sellerProductReview = await _context.SellerProductReviews.AsTracking().
                    FirstOrDefaultAsync(e => e.Id == Id && e.UserId == UserId);
                    

                return sellerProductReview;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }
    }
}
