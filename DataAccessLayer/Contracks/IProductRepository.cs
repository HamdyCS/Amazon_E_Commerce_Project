using DataAccessLayer.Entities;

namespace DataAccessLayer.Contracks
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<Product> GetByNameEnAsync(string NameEn);
        Task<Product> GetByNameArAsync(string NameAr);

        Task<IEnumerable<Product>> SearchByNameEnAsync(string NameEn, int pageSize);
        Task<IEnumerable<Product>> SearchByNameArAsync(string NameAr, int pageSize);
    }
}
