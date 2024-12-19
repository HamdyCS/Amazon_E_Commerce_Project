using DataAccessLayer.Entities;

namespace DataAccessLayer.Contracks
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<Product> GetByNameEnAsync(string NameEn);
        Task<Product> GetByNameArAsync(string NameAr);
    }
}
