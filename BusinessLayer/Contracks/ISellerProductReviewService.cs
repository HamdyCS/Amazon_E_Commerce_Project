using BusinessLayer.Dtos;

namespace BusinessLayer.Contracks
{
    public interface ISellerProductReviewService
    {
        public Task<SellerProductReviewDto> FindByIdAsync(long id);

        public Task<IEnumerable<SellerProductReviewDto>> GetAllSellerProductReviewsBySellerProductIdAsync(long SellerProductId);

        public Task<IEnumerable<SellerProductReviewDto>> GetPagedSellerProductReviewsBySellerProductIdAsync(int pageNumber, int pageSize, long SellerProductId);

        public Task<SellerProductReviewDto> AddAsync(SellerProductReviewDto sellerProductReivewDto, string UserId);

        public Task<bool> UpdateByIdAndUserIdAsync(long Id, string UserId, SellerProductReviewDto sellerProductReivewDto);

        public Task<bool> DeleteByIdAsync(long Id);

        public Task<bool> DeleteByIdAndUserIdAsync(long Id, string UserId);

        public Task<double> GetAverageOfStarsBySellerProductIdAsync(long sellerProductId);
    }
}
