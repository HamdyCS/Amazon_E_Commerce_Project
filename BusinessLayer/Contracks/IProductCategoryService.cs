using BusinessLayer.Dtos;

namespace BusinessLayer.Contracks
{
    public interface IProductCategoryService
    {
        Task<ProductCategoryDto> FindByNameEnAsync(string nameEn);
        Task<ProductCategoryDto> FindByNameArAsync(string nameAr);

        public Task<ProductCategoryDto> FindByIdAsync(long id);

        public Task<IEnumerable<ProductCategoryDto>> GetAllAsync();

        public Task<long> GetCountAsync();

        public Task<ProductCategoryDto> AddAsync(CreateProductCategoryDto dto, string UserId);

        public Task<IEnumerable<ProductCategoryDto>> AddRangeAsync(IEnumerable<CreateProductCategoryDto> dtos, string UserId);

        public Task<bool> UpdateByIdAsync(long Id, CreateProductCategoryDto dto);

        public Task<bool> DeleteByIdAsync(long Id);

        public Task<bool> DeleteRangeByIdAsync(IEnumerable<long> Ids);

        public Task<IEnumerable<ProductCategoryDto>> GetPagedDataAsync(int pageNumber, int pageSize);
    }
}
