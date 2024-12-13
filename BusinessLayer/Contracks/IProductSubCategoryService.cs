using BusinessLayer.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Contracks
{
    public interface IProductSubCategoryService
    {
        Task<ProductSubCategoryDto> FindByNameEnAsync(string nameEn);
        Task<ProductSubCategoryDto> FindByNameArAsync(string nameAr);

        public Task<ProductSubCategoryDto> FindByIdAsync(long id);

        public Task<IEnumerable<ProductSubCategoryDto>> GetAllAsync();

        public Task<IEnumerable<ProductSubCategoryDto>> GetAllByProductCategoryIdAsync(long ProductCategoryId);

        public Task<long> GetCountAsync();

        public Task<ProductSubCategoryDto> AddAsync(ProductSubCategoryDto productSubCategoryDto, string UserId);

        public Task<IEnumerable<ProductSubCategoryDto>> AddRangeAsync(IEnumerable<ProductSubCategoryDto> productSubCategoriesDtos, string UserId);

        public Task<bool> UpdateByIdAsync(long Id, ProductSubCategoryDto productSubCategoryDto);

        public Task<bool> DeleteByIdAsync(long Id);

        public Task<bool> DeleteRangeByIdAsync(IEnumerable<long> Ids);

        public Task<IEnumerable<ProductSubCategoryDto>> GetPagedDataAsync(int pageNumber, int pageSize);
    }
}
