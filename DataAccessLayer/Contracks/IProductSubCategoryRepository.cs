using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Contracks
{
    public interface IProductSubCategoryRepository : IGenericRepository<ProductSubCategory>
    {
        Task<ProductSubCategory> GetByNameEnAsync(string NameEn);
        Task<ProductSubCategory> GetByNameArAsync(string NameAr);
        Task<IEnumerable<ProductSubCategory>> GetAllByProductCategoryIdAsync(long ProductCategoryId);
    }
}
