using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Contracks
{
    public interface ISellerProductReviewRepository : IGenericRepository<SellerProductReview>
    {
        Task<IEnumerable<SellerProductReview>> GetAllSellerProductReviewsBySellerProductIdAsync(long sellerProductId);

        Task<double> GetAveragedOfStarsOfSellerProductBySellerProductIdAsync(long sellerProductId);

        Task<SellerProductReview> GetSellerProductReviewByIdAndUserIdAsync(long Id, string UserId);

        Task<IEnumerable<SellerProductReview>> GetAllSellerProductReviewsWithUserInfoBySellerProductIdAsync(long sellerProductId);

        Task<IEnumerable<SellerProductReview>> GetPagedSellerProductReviewsWithUserInfoBySellerProductIdAsync(int PageNumber, int PageSize, long sellerProductId);

        Task<bool> CheckIfThisUserReviewedThisSellerProductByIdAndUserIdAsync(long Id,string UserId);

    }
}
