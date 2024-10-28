using BusinessLayer.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Contracks
{
    public interface ICityService : IGenriceService<CityDto>
    {
        Task<CityDto> FindByNameEnAsync(string cityNameEn);
        Task<CityDto> FindByNameArAsync(string cityNameAr);
        Task<bool> DeleteByNameEnAsync(string cityNameEn);
        Task<bool> DeleteByNameArAsync(string cityNameAr);
    }
}
