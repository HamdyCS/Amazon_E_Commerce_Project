using BusinessLayer.Dtos;
using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Contracks
{
    public interface IProductCategoryService
    {
        Task<ProductCategoryDto> FindByNameEnAsync(string nameEn);
        Task<ProductCategoryDto> FindByNameArAsync(string nameAr);

        public Task<ProductCategoryDto> FindByIdAsync(long id);

        public Task<IEnumerable<ProductCategoryDto>> GetAllAsync();

        public Task<long> GetCountAsync();

        public Task<ProductCategoryDto> AddAsync(ProductCategoryDto dto,string UserId);

        public Task<IEnumerable<ProductCategoryDto>> AddRangeAsync(IEnumerable<ProductCategoryDto> dtos, string UserId);

        public Task<bool> UpdateByIdAsync(long Id, ProductCategoryDto dto);

        public Task<bool> DeleteByIdAsync(long Id);

        public Task<bool> DeleteRangeByIdAsync(IEnumerable<long> Ids);

        public Task<IEnumerable<ProductCategoryDto>> GetPagedDataAsync(int pageNumber, int pageSize);
    }
}
