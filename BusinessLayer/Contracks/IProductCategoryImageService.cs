using BusinessLayer.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Contracks
{
    public interface IProductCategoryImageService : IGenriceService<ProductCategoryImageDto>
    {
       Task<IEnumerable<ProductCategoryImageDto>> FindAllProductCategoryImagesByProductCategoryIdAsync(long productCategoryId);
       Task<bool> DeleteAllProductCategoryImagesByProductCategoryIdAsync(long productCategoryId);
    }
}
