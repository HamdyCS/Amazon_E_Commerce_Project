using BusinessLayer.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Contracks
{
    public interface IBannerService
    {
        Task<BannerDto> GetBannerByIdAsync(long id);

        Task<IEnumerable<BannerDto>> GetActiveBannersAsync();

        Task<IEnumerable<BannerDto>> GetAllBannersPagesAsync(int pageNumber, int pageSize);

        Task<BannerDto> AddBannerAsync(CreateBannerDto createBannerDto);

        Task<IEnumerable<BannerDto>> AddBannersAsync(IEnumerable<CreateBannerDto> createBannerDtos);

        Task<bool> DeleteBannerByIdAsync(long id);

        Task<bool> DeleteAllActiveBannersAsync();

        Task<IEnumerable<BannerDto>> UpdateBannersAsync(IEnumerable<UpdateBannerDto> updateBannersDto);

        Task<BannerDto> UpdateBannerAsync(UpdateBannerDto updateBannerDto);

    }
}
