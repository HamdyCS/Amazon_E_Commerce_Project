using BusinessLayer.Dtos;
using DataAccessLayer.Entities;

namespace BusinessLayer.Contracks
{
    public interface IProductService
    {
        Task<ProductDto> FindByNameEnAsync(string nameEn);
        Task<ProductDto> FindByNameArAsync(string nameAr);

        public Task<ProductDto> FindByIdAsync(long id);

        public Task<IEnumerable<ProductDto>> GetAllAsync();

        public Task<long> GetCountAsync();

        public Task<ProductDto> AddAsync(ProductDto productDto, string UserId);

        public Task<IEnumerable<ProductDto>> AddRangeAsync(IEnumerable<ProductDto> productDtos, string UserId);

        public Task<bool> UpdateByIdAsync(long Id, ProductDto productDto);

        public Task<bool> DeleteByIdAsync(long Id);

        public Task<bool> DeleteRangeByIdAsync(IEnumerable<long> Ids);

        public Task<IEnumerable<ProductDto>> GetPagedDataAsync(int pageNumber, int pageSize);

        Task<IEnumerable<ProductSearchResultDto>> SearchByNameEnAsync(string NameEn, int pageSize);

        Task<IEnumerable<ProductSearchResultDto>> SearchByNameArAsync(string NameAr, int pageSize);
    }
}
