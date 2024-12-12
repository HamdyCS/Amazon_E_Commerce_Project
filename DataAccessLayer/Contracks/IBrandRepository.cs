using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Contracks
{
    public interface IBrandRepository : IGenericRepository<Brand>
    {
        Task<Brand> GetByNameEnAsync(string nameEn);
        Task<Brand> GetByNameArAsync(string nameAr);
    }
}
