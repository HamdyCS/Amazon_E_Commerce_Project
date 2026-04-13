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

        public UserAddressService(ICityService cityService, ILogger<UserAddressDto> logger, IUnitOfWork unitOfWork, IGenericMapper genericMapper)
        {
            this._cityService = cityService;
            this._logger = logger;
            this._unitOfWork = unitOfWork;
            this._genericMapper = genericMapper;
        }

        private async Task _HandleDefaultAddress(UserAddress userAddress, string userId)
        {
            //get all user addresses
            var userAddresses = await _unitOfWork.userAdderssRepository.GetAllUserAddressesAsNoTrackinByUserIdAsync(userId);

            //if there are no addresses for user set new address to default
            if (userAddresses == null || !userAddresses.Any())
            {
                userAddress.IsDefault = true;
            }

            else
            {
                //remove user address from user addresses list if exist to avoid conflict when update all user addresses
                userAddresses = userAddresses.Where(x => x.Id != userAddress.Id).ToList();

                //if new address is default and there are other addresses for user
                if (userAddress.IsDefault)
                {
                    //set all user addresses to not default
                    foreach (var item in userAddresses)
                        item.IsDefault = false;

                    //update all user addresses
                    _unitOfWork.userAdderssRepository.UpdateRange(userAddresses);

                    //complete update all user addresses
                    await _IsCompletedAsync();
                }
                else
                {
                    //if not there are other addresses for user set new address to default
                    if (!userAddresses.Any(x => x.IsDefault))
                        userAddress.IsDefault = true;
                }
            }
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

            var userDto = await _unitOfWork.userRepository.GetByIdAsync(UserId);
            if (userDto == null) throw new KeyNotFoundException($"User not found");

            var cityDto = await _cityService.FindByIdAsync(UserAddressdto.CityId);
            if (cityDto is null) throw new KeyNotFoundException($"City not found.Id= ${UserAddressdto.CityId}"); ;

            var userAddress = _genericMapper.MapSingle<UserAddressDto, UserAddress>(UserAddressdto);
            userAddress.UserId = UserId;



            try
            {
                //start transaction
                await _unitOfWork.BeginTransactionAsync();

                await _HandleDefaultAddress(userAddress, UserId);


                //add new user address
                await _unitOfWork.userAdderssRepository.AddAsync(userAddress);
                var IsAdded = await _IsCompletedAsync();

                if (!IsAdded) throw new Exception("Error during addition new userAddress ");

                _genericMapper.MapSingle(userAddress, UserAddressdto);

                //commit transaction
                await _unitOfWork.CommitTransactionAsync();

                return UserAddressdto;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }

        }

        public async Task<bool> DeleteByIdAndUserIdAsync(long Id, string userId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(Id, nameof(Id));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(userId, nameof(userId));

            var userAddress = await _unitOfWork.userAdderssRepository.GetByIdAndUserIdAsync(Id, userId);
            if (userAddress is null) return false;

            try
            {
                //start transaction
                await _unitOfWork.BeginTransactionAsync();

                await _unitOfWork.userAdderssRepository.DeleteAsync(Id);
                var IsDeleted = await _IsCompletedAsync();

                if (!IsDeleted) throw new Exception("Error during deleting userAddress");

                //check if deleted user address is default
                if (userAddress.IsDefault)
                {
                    var oldestUserAddress = await _unitOfWork.userAdderssRepository.GetOldestUserAddressByUserIdAsync(userId);

                    if (oldestUserAddress != null)
                    {
                        oldestUserAddress.IsDefault = true;

                        _unitOfWork.userAdderssRepository.Update(oldestUserAddress);

                        //update oldest user address to default
                        var IsUpdated = await _IsCompletedAsync();

                        if (!IsUpdated) throw new Exception("Error during updating userAddress to default");
                    }
                }

                //commit transaction
                await _unitOfWork.CommitTransactionAsync();

            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }

            return true;
        }

        public async Task<UserAddressDto> FindByIdAndUserIdAsync(long id, string userid)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(id, nameof(id));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(userid, nameof(userid));


            var userAddress = await _unitOfWork.userAdderssRepository.GetByIdAndUserIdAsync(id, userid);
            if (userAddress is null) return null;

            var userAddressDto = _genericMapper.MapSingle<UserAddress, UserAddressDto>(userAddress);
            return userAddressDto;

        }

        public async Task<IEnumerable<UserAddressDto>> GetAllUserAddressesByUserIdAsync(string userId)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(userId, nameof(userId));

            var userAddresses = await _unitOfWork.userAdderssRepository.GetAllUserAddressesAsNoTrackinByUserIdAsync(userId);
            if (userAddresses is null || !userAddresses.Any())
                return null;

            var userAddressesDto = _genericMapper.MapCollection<UserAddress, UserAddressDto>(userAddresses);
            return userAddressesDto;
        }

        public async Task<int> GetCountOfUserAddressesByUserId(string userId)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(userId, nameof(userId));

            var count = await _unitOfWork.userAdderssRepository.GetCountOfUserAddressesByUserIdAsync(userId);
            return count;


        }

        public async Task<bool> UpdateByIdAndUserIdAsync(long Id, string userId, UserAddressDto UserAddressdto)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(userId, nameof(userId));
            ParamaterException.CheckIfLongIsBiggerThanZero(Id, nameof(Id));
            ParamaterException.CheckIfObjectIfNotNull(UserAddressdto, nameof(UserAddressdto));

            var userAddress = await _unitOfWork.userAdderssRepository.GetByIdAndUserIdAsync(Id, userId);
            if (userAddress is null) return false;

            var IsBeforeUpdateDefault = userAddress.IsDefault;

            var cityDto = await _cityService.FindByIdAsync(UserAddressdto.CityId);
            if (cityDto is null) return false;

            _genericMapper.MapSingle(UserAddressdto, userAddress);

            try
            {
                //start transaction
                await _unitOfWork.BeginTransactionAsync();

                await _HandleDefaultAddress(userAddress, userId);

                //حالة خاصة عند تحديث العنوان الافتراضي إلى غير الافتراضي يجب تعيين أقدم عنوان آخر كافتراضي
                if (IsBeforeUpdateDefault && !userAddress.IsDefault)
                {
                    var oldestUserAddress = await _unitOfWork.userAdderssRepository.GetOldestUserAddressByUserIdAsync(userId);
                    oldestUserAddress.IsDefault = true;

                    if (oldestUserAddress != null)
                    {

                        _unitOfWork.userAdderssRepository.Update(oldestUserAddress);

                        //update oldest user address to default
                        var IsOldestUpdated = await _IsCompletedAsync();

                        if (!IsOldestUpdated) throw new Exception("Error during updating userAddress to default");
                    }
                    else
                    {
                        //if there are no other addresses for user set updated address to default
                        userAddress.IsDefault = true;
                    }
                }

                _unitOfWork.userAdderssRepository.Update(userAddress);

                var IsUpdated = await _IsCompletedAsync();
                if (!IsUpdated) throw new Exception("Error during updating userAddress");

                //commit transaction
                await _unitOfWork.CommitTransactionAsync();

                return true;

            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }

        }

    }
}
