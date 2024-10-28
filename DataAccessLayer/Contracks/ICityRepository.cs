using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Contracks
{
    public interface ICityRepository : IGenericRepository<City>
    {
        Task<City> GetByNameEnAsync(string cityNameEn);
        Task<City> GetByNameArAsync(string cityNameAr);
        Task DeleteByNameEnAsync(string cityNameEn);
        Task DeleteByNameArAsync(string cityNameAr);
    }
}
