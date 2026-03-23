using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Contracks
{
    public interface IProductReviewRepository : IGenericRepository<ProductReview>
    {
        Task<IEnumerable<ProductReview>> GetAllProductReviewsByProductIdAsync(long productId);

        Task<double> GetAveragedOfStarsByProductIdAsync(long ProductId);

        Task<ProductReview> GetProductReviewByIdAndUserIdAsync(long Id, string UserId);

        Task<IEnumerable<ProductReview>> GetAllSellerProductReviewsWithUserInfoByProductIdAsync(long productId);

        Task<IEnumerable<ProductReview>> GetPagedProductReviewsWithUserInfoByProductIdAsync(int PageNumber, int PageSize, long ProductId);

        Task<bool> CheckIfThisUserReviewedThisProductByIdAndUserIdAsync(long Id,string UserId);

    }
}
