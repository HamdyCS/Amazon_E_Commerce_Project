using AutoMapper;
using BusinessLayer.Contracks;
using BusinessLayer.Dtos;
using BusinessLayer.Mapper.Contracks;
using DataAccessLayer.Entities;
using DataAccessLayer.UnitOfWork.Contracks;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Servicese
{
    public class CityService : ICityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericMapper _genericMapper;
        private readonly ILogger<CityService> _logger;

        public CityService(IUnitOfWork unitOfWork, IGenericMapper genericMapper, ILogger<CityService> logger)
        {
            _unitOfWork = unitOfWork;
            _genericMapper = genericMapper;
            _logger = logger;
        }

        private async Task<bool> _completeAsync()
        {
            try
            {
                var result = await _unitOfWork.CompleteAsync();
                return result > 0;

            }
            catch (Exception ex)
            {
                throw;
            }


        }
        public async Task<CityDto> AddAsync(CityDto dto)
        {
            if (dto == null) throw new ArgumentNullException("CityDto cannot be null");
            try
            {
                var city = _genericMapper.MapSingle<CityDto, City>(dto);

                if (city == null) return null;

                await _unitOfWork.cityRepository.AddAsync(city);

                var IsComoleted = await _completeAsync();

                if (!IsComoleted)
                {
                    return null;
                }

                var result = _genericMapper.MapSingle<City, CityDto>(city);

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<CityDto>> AddRangeAsync(IEnumerable<CityDto> dtos)
        {
            if (dtos == null || !dtos.Any()) throw new ArgumentNullException("CityDtos cannot be null or empty");
            try
            {
                var cities = _genericMapper.MapCollection<CityDto, City>(dtos);

                if (cities == null) return null;

                await _unitOfWork.cityRepository.AddRangeAsync(cities);

                var IsComoleted = await _completeAsync();

                if (!IsComoleted)
                {
                    return null;
                }
                var result = _genericMapper.MapCollection<City, CityDto>(cities);

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> DeleteByIdAsync(long Id)
        {
            if (Id < 1) throw new ArgumentException("Cannot be smaller than 1", nameof(Id));

            try
            {
                var city = await _unitOfWork.cityRepository.GetByIdAsTrackingAsync(Id);

                if (city == null) return false;

                await _unitOfWork.cityRepository.DeleteAsync(Id);

                var IsComoleted = await _completeAsync();

                return IsComoleted;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> DeleteByNameArAsync(string cityNameAr)
        {
            if (string.IsNullOrEmpty(cityNameAr)) throw new ArgumentException("Cannot be null or empty", nameof(cityNameAr));

            try
            {
                var city = await _unitOfWork.cityRepository.GetByNameArAsync(cityNameAr);

                if (city == null) return false;

                await _unitOfWork.cityRepository.DeleteAsync(city.Id);

                var IsComoleted = await _completeAsync();

                return IsComoleted;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> DeleteByNameEnAsync(string cityNameEn)
        {
            if (string.IsNullOrEmpty(cityNameEn)) throw new ArgumentException("Cannot be null or empty", nameof(cityNameEn));

            try
            {
                var city = await _unitOfWork.cityRepository.GetByNameEnAsync(cityNameEn);

                if (city == null) return false;

                await _unitOfWork.cityRepository.DeleteAsync(city.Id);

                var IsComoleted = await _completeAsync();

                return IsComoleted;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CityDto> FindByIdAsync(long id)
        {
            if (id < 1) throw new ArgumentException("Cannot be smaller than 1", nameof(id));

            try
            {
                var city = await _unitOfWork.cityRepository.GetByIdAsTrackingAsync(id);

                if (city is null) return null;
                var cityDto = _genericMapper.MapSingle<City, CityDto>(city);

                return cityDto;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CityDto> FindByNameArAsync(string cityNameAr)
        {
            if (string.IsNullOrEmpty(cityNameAr)) throw new ArgumentException("Cannot be null or empty", nameof(cityNameAr));

            try
            {
                var city = await _unitOfWork.cityRepository.GetByNameArAsync(cityNameAr);

                if (city is null) return null;

                var cityDto = _genericMapper.MapSingle<City, CityDto>(city);

                return cityDto;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CityDto> FindByNameEnAsync(string cityNameEn)
        {
            if (string.IsNullOrEmpty(cityNameEn)) throw new ArgumentException("Cannot be null or empty", nameof(cityNameEn));

            try
            {
                var city = await _unitOfWork.cityRepository.GetByNameArAsync(cityNameEn);

                if (city is null) return null;

                var cityDto = _genericMapper.MapSingle<City, CityDto>(city);

                return cityDto;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<CityDto>> GetAllAsync()
        {
            try
            {
                var cites = await _unitOfWork.cityRepository.GetAllNoTrackingAsync();

                var citiesDtos = _genericMapper.MapCollection<City, CityDto>(cites);

                return citiesDtos;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<long> GetCountOfAsync()
        {
            try
            {
               var count = await _unitOfWork.cityRepository.GetCountAsync();

                return count;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<CityDto>> GetPagedDataAsync(int pageNumber, int pageSize)
        {
            if (pageNumber < 1) throw new ArgumentException("Must be greater than zero", nameof(pageNumber));
            if (pageSize < 1) throw new ArgumentException("Page size must be greater than zero", nameof(pageSize));

            try
            {
                var cities = await _unitOfWork.cityRepository.GetPagedDataAsNoTractingAsync(pageNumber, pageSize);

                var citiesDtos = _genericMapper.MapCollection<City,CityDto>(cities);

                return citiesDtos;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> UpdateByIdAsync(long Id,CityDto dto)
        {
            if (Id < 1) throw new ArgumentException("Id cannot be smaller than 1");
            if (dto == null) throw new ArgumentNullException("Cannot be null",nameof(dto));

            try
            {
                var city = await _unitOfWork.cityRepository.GetByIdAsTrackingAsync(Id);

                if(city == null) return false;

               _genericMapper.MapSingle(dto, city);

                var IsComoleted = await _completeAsync();

                return IsComoleted;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> DeleteRangeByIdAsync(IEnumerable<long> Ids)
        {
           if(Ids is null ||!Ids.Any()) throw new ArgumentException("cannot be null or empty",nameof(Ids));

            try
            {
               await  _unitOfWork.cityRepository.DeleteRangeAsync(Ids);

                var result = await _completeAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        
    }
}
