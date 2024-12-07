using BusinessLayer.Contracks;
using BusinessLayer.Dtos;
using BusinessLayer.Exceptions;
using BusinessLayer.Mapper.Contracks;
using DataAccessLayer.Entities;
using DataAccessLayer.Identity.Entities;
using DataAccessLayer.UnitOfWork.Contracks;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Servicese
{
    public class UserAddressService : IUserAddressService
    {
        private readonly ICityService _cityService;
        private readonly ILogger<UserAddressDto> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericMapper _genericMapper;

        public UserAddressService(ICityService cityService,ILogger<UserAddressDto> logger, IUnitOfWork unitOfWork, IGenericMapper genericMapper)
        {
            this._cityService = cityService;
            this._logger = logger;
            this._unitOfWork = unitOfWork;
            this._genericMapper = genericMapper;
        }

        private async Task<bool> _IsCompletedAsync()
        {
            try
            {
                var NumberOfRowsAffected = await _unitOfWork.CompleteAsync();
                return NumberOfRowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<UserAddressDto> AddAsync(string UserId, UserAddressDto UserAddressdto)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(UserId, nameof(UserId));
            ParamaterException.CheckIfObjectIfNotNull(UserAddressdto, nameof(UserAddressdto));

            try
            {
                var userDto = await _unitOfWork.userRepository.GetByIdAsync(UserId);
                if (userDto == null) return null;

                var cityDto = await _cityService.FindByIdAsync(UserAddressdto.CityId);
                if (cityDto is null) return null;

                var userAddress = _genericMapper.MapSingle<UserAddressDto, UserAddress>(UserAddressdto);
                if (userAddress is null) return null;

                userAddress.UserId = UserId;

                await _unitOfWork.userAdderssRepository.AddAsync(userAddress);

                var IsAdded = await _IsCompletedAsync();

                if (!IsAdded) return null;

                _genericMapper.MapSingle(userAddress, UserAddressdto);

                return UserAddressdto;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> DeleteByIdAndUserIdAsync(long Id, string userId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(Id, nameof(Id));
            try
            {
                var userAddress = await _unitOfWork.userAdderssRepository.GetByIdAndUserIdAsync(Id,userId);
                if (userAddress is null) return false;

                await _unitOfWork.userAdderssRepository.DeleteAsync(Id);
                var IsDeleted = await _IsCompletedAsync();

                return IsDeleted;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<UserAddressDto> FindByIdAndUserIdAsync(long id, string userid)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(id, nameof(id));
            try
            {
                var userAddress = await _unitOfWork.userAdderssRepository.GetByIdAndUserIdAsync(id,userid);
                if (userAddress is null) return null;

                var userAddressDto = _genericMapper.MapSingle<UserAddress, UserAddressDto>(userAddress);
                return userAddressDto;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<UserAddressDto>> GetAllUserAddressesByUserIdAsync(string userId)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(userId, nameof(userId));
            try
            {
                var userAddresses = await _unitOfWork.userAdderssRepository.GetAllUserAddressesAsNoTrackinByUserIdAsync(userId);
                if (userAddresses is null || !userAddresses.Any())
                    return null;

                var userAddressesDto = _genericMapper.MapCollection<UserAddress, UserAddressDto>(userAddresses);
                return userAddressesDto;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<int> GetCountOfUserAddressesByUserId(string userId)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(userId, nameof(userId));
            try
            {
                var count = await _unitOfWork.userAdderssRepository.GetCountOfUserAddressesByUserIdAsync(userId);
                return count;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> UpdateByIdAndUserIdAsync(long Id,string userId, UserAddressDto UserAddressdto)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(Id, nameof(Id));
            ParamaterException.CheckIfObjectIfNotNull(UserAddressdto, nameof(UserAddressdto));
            try
            {
                var userAddress = await _unitOfWork.userAdderssRepository.GetByIdAndUserIdAsync(Id, userId);
                if (userAddress is null) return false;

                var cityDto = await _cityService.FindByIdAsync(UserAddressdto.CityId);
                if (cityDto is null) return false;

                _genericMapper.MapSingle(UserAddressdto, userAddress);

                await _unitOfWork.userAdderssRepository.UpdateAsync(Id, userAddress);

                var IsUpdated = await _IsCompletedAsync();
                return IsUpdated;

            }
            catch (Exception ex)
            {
                throw;
            }
        }



    }
}
