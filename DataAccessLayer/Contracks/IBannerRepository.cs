using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Contracks
{
    public interface IBannerRepository : IGenericRepository<Banner>
    {
        Task<IEnumerable<Banner>> GetActiveBannersOrderByDisplayOrderAsc();

        Task<IEnumerable<Banner>> GetOverLappingBannersOrderByDisplayOrderAsc(DateTime startDate,DateTime endDate);

    }
}
