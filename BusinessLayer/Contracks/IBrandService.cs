using BusinessLayer.Dtos;

namespace BusinessLayer.Contracks
{
    public interface IBrandService
    {
        public Task<BrandDto> FindByIdAsync(long id);
        public Task<BrandDto> FindByNameEnAsync(string NameEn);
        public Task<BrandDto> FindByNameArAsync(string NameAr);
        public Task<IEnumerable<BrandDto>> GetAllAsync();
        public Task<long> GetCountAsync();
        public Task<BrandDto> AddAsync(CreateBrandDto createBrandDto, string UserId);

        public Task<IEnumerable<BrandDto>> AddRangeAsync(IEnumerable<CreateBrandDto> createBrandDtos, string UserId);

        public Task<bool> UpdateByIdAsync(long Id, CreateBrandDto createBrandDto);

        public Task<bool> DeleteByIdAsync(long Id);

        public Task<IEnumerable<BrandDto>> GetPagedDataAsync(int pageNumber, int pageSize);
    }
}
