using DataAccessLayer.Entities;

namespace DataAccessLayer.Contracks
{
    public interface IProductImageRepository : IGenericRepository<ProductImage>
    {

        public Task<IEnumerable<ProductImage>> GetAllProductProductIdAsync(long productId);

        public Task DeleteAllProductImagesByProductIdAsync(long productId);


    }
}
