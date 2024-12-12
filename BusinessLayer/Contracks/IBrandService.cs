using BusinessLayer.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Contracks
{
    public interface IBrandService
    {
        public Task<BrandDto> FindByIdAsync(long id);
        public Task<BrandDto> FindByNameEnAsync(string NameEn);
        public Task<BrandDto> FindByNameArAsync(string NameAr);
        public Task<IEnumerable<BrandDto>> GetAllAsync();
        public Task<long> GetCountAsync();
        public Task<BrandDto> AddAsync(BrandDto dto, string UserId);

        public Task<IEnumerable<BrandDto>> AddRangeAsync(IEnumerable<BrandDto> dtos, string UserId);

        public Task<bool> UpdateByIdAsync(long Id, BrandDto dto);

        public Task<bool> DeleteByIdAsync(long Id);

        public Task<IEnumerable<BrandDto>> GetPagedDataAsync(int pageNumber, int pageSize);
    }
}
