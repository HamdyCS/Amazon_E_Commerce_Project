using BusinessLayer.Dtos;

namespace BusinessLayer.Contracks
{
    public interface IProductImageService : IGenriceService<ProductImageDto>
    {
        Task<IEnumerable<ProductImageDto>> FindAllProductImagesByProductIdAsync(long productId);
        Task<bool> DeleteAllProductImagesByProductIdAsync(long productId);
    }
}
