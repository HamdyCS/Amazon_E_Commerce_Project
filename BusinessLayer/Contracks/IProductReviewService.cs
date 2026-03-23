using BusinessLayer.Dtos;

namespace BusinessLayer.Contracks
{
    public interface IProductReviewService
    {
        public Task<ProductReviewDto> FindByIdAsync(long id);

        public Task<IEnumerable<ProductReviewDto>> GetAllProductReviewsByProductIdAsync(long ProductId);

        public Task<IEnumerable<ProductReviewDto>> GetPagedProductReviewsByProductIdAsync(int pageNumber, int pageSize, long ProductId);

        public Task<ProductReviewDto> AddAsync(ProductReviewDto productReivewDto, string UserId);

        public Task<bool> UpdateByIdAndUserIdAsync(long Id, string UserId, ProductReviewDto productReivewDto);

        public Task<bool> DeleteByIdAsync(long Id);

        public Task<bool> DeleteByIdAndUserIdAsync(long Id, string UserId);

        public Task<double> GetAverageOfStarsByProductIdAsync(long productId);
    }
}
