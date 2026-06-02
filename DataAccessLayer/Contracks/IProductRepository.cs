using DataAccessLayer.Entities;
using DataAccessLayer.Enums;
using DataAccessLayer.Pagination;

namespace DataAccessLayer.Contracks
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<Product> GetByNameEnAsync(string NameEn);
        Task<Product> GetByNameArAsync(string NameAr);

        Task<IEnumerable<Product>> SearchByNameEnAsync(string NameEn, int pageSize);
        Task<IEnumerable<Product>> SearchByNameArAsync(string NameAr, int pageSize);
        Task<IEnumerable<string>> SearchByNameAsync(string query, int pageSize, EnLang lang);
        Task<IEnumerable<Product>> GetAllOrderByBestSellerDescAsync();
        Task<IEnumerable<Product>> GetPagedOrderByBestSellerDescAsync(int pageNumber,int pageSize);

    }
}
