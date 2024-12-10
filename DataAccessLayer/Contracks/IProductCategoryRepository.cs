using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Entities;

namespace DataAccessLayer.Contracks
{
    public interface IProductCategoryRepository : IGenericRepository<ProductCategory>
    {
        Task<ProductCategory> GetByNameEnAsync(string NameEn);
        Task<ProductCategory> GetByNameArAsync(string NameAr);
    }
}
