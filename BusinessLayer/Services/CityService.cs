using AutoMapper;
using BusinessLayer.Contracks;
using BusinessLayer.Dtos;
using BusinessLayer.Exceptions;
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
        private readonly IUserService _userService;

        public CityService(IUnitOfWork unitOfWork, IGenericMapper genericMapper, ILogger<CityService> logger,
            IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _genericMapper = genericMapper;
            _logger = logger;
            this._userService = userService;
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
        public async Task<CityDto> AddAsync(CityDto cityDto, string UserId)
        {
            ParamaterException.CheckIfObjectIfNotNull(cityDto, nameof(cityDto));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(UserId, nameof(UserId));
            try
            {
                var userDto = await _userService.FindByIdAsync(UserId);
                if (userDto == null) return null;

                var city = _genericMapper.MapSingle<CityDto, City>(cityDto);

                if (city == null) return null;

                city.CreatedBy = UserId;

                await _unitOfWork.cityRepository.AddAsync(city);

                var IsAdded = await _completeAsync();

                if (!IsAdded)
                {
                    return null;
                }

                 _genericMapper.MapSingle(city,cityDto);

                return cityDto;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> DeleteByIdAsync(long Id)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(Id, nameof(Id));

            try
            {
                var city = await _unitOfWork.cityRepository.GetByIdAsTrackingAsync(Id);

                if (city == null) return false;

                await _unitOfWork.cityRepository.DeleteAsync(Id);

                var IsDeleted = await _completeAsync();

                return IsDeleted;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> DeleteByNameArAsync(string cityNameAr)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(cityNameAr, nameof(cityNameAr));

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
            ParamaterException.CheckIfStringIsNotNullOrEmpty(cityNameEn, nameof(cityNameEn));

            try
            {
                var city = await _unitOfWork.cityRepository.GetByNameEnAsync(cityNameEn);

                if (city == null) return false;

                await _unitOfWork.cityRepository.DeleteAsync(city.Id);

                var IsDeleted = await _completeAsync();

                return IsDeleted;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CityDto> FindByIdAsync(long id)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(id, nameof(id));

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
            ParamaterException.CheckIfStringIsNotNullOrEmpty(cityNameAr, nameof(cityNameAr));

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
            ParamaterException.CheckIfStringIsNotNullOrEmpty(cityNameEn, nameof(cityNameEn));

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
                var cites = await _unitOfWork.cityRepository.GetAllAsNoTrackingAsync();

                var citiesDtos = _genericMapper.MapCollection<City, CityDto>(cites);

                return citiesDtos;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<long> GetCountAsync()
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
            ParamaterException.CheckIfLongIsBiggerThanZero(pageNumber, nameof(pageNumber));
            ParamaterException.CheckIfLongIsBiggerThanZero(pageSize, nameof(pageSize));
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
            ParamaterException.CheckIfLongIsBiggerThanZero(Id, nameof(Id));
            ParamaterException.CheckIfObjectIfNotNull(dto, nameof(dto));


            try
            {
                var city = await _unitOfWork.cityRepository.GetByIdAsTrackingAsync(Id);

                if(city == null) return false;

               _genericMapper.MapSingle(dto,city);

                var IsUpdated = await _completeAsync();

                return IsUpdated;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        
    }
}
