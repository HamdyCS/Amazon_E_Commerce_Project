using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Contracks
{
    public interface IProductCategoryImageRepository : IGenericRepository<ProductCategoryImage>
    {

        public Task<IEnumerable<ProductCategoryImage>> GetAllProductCategoryImagesByProductCategoryIdAsync(long  productCategoryId);

        public Task DeleteAllProductCategoryImagesByProductCategoryIdAsync(long productCategoryId);


    }
}
