using BusinessLayer.Dtos;
using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Contracks
{
    public interface ICityService
    {
        public Task<CityDto> AddAsync(CityDto dto,string UserId);

        public Task<bool> DeleteByIdAsync(long Id);

        public Task<bool> DeleteByNameArAsync(string cityNameAr);


        public Task<bool> DeleteByNameEnAsync(string cityNameEn);


        public Task<CityDto> FindByIdAsync(long id);


        public Task<CityDto> FindByNameArAsync(string cityNameAr);


        public Task<CityDto> FindByNameEnAsync(string cityNameEn);


        public Task<IEnumerable<CityDto>> GetAllAsync();


        public Task<long> GetCountAsync();


        public Task<IEnumerable<CityDto>> GetPagedDataAsync(int pageNumber, int pageSize);


        public Task<bool> UpdateByIdAsync(long Id, CityDto dto);


    }
}
