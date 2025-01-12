using BusinessLayer.Contracks;
using BusinessLayer.Dtos;
using BusinessLayer.Exceptions;
using BusinessLayer.Mapper.Contracks;
using DataAccessLayer.Entities;
using DataAccessLayer.UnitOfWork.Contracks;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Servicese
{
    public class ShippingCostService : IShippingCostService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly ICityService _cityService;
        private readonly IGenericMapper _genericMapper;

        public ShippingCostService(IUnitOfWork unitOfWork, IUserService userService, ICityService cityService,
            IGenericMapper genericMapper)
        {
            this._unitOfWork = unitOfWork;
            this._userService = userService;
            this._cityService = cityService;
            this._genericMapper = genericMapper;
        }

        private async Task<bool> _completeAsync()
        {
            var numbersOfRowsAffeted = await _unitOfWork.CompleteAsync();
            return numbersOfRowsAffeted > 0;
        }
        public async Task<ShippingCostDto> AddAsync(ShippingCostDto shippingCostDto, string UserId)
        {
            ParamaterException.CheckIfObjectIfNotNull(shippingCostDto, nameof(shippingCostDto));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(UserId, nameof(UserId));

            var cityDto = await _cityService.FindByIdAsync(shippingCostDto.CityId);
            if (cityDto == null) return null;

            var userDto = await _userService.FindByIdAsync(UserId);
            if (userDto is null) return null;


            var shippingCostByCityId = await _unitOfWork.shippingCostRepository.GetByCityIdAsync(shippingCostDto.CityId);
            if (shippingCostByCityId != null) return null;

            var NewShippingCost = _genericMapper.MapSingle<ShippingCostDto, ShippingCost>(shippingCostDto);
            if (NewShippingCost == null) return null;

            NewShippingCost.CreatedAt = DateTime.UtcNow;
            NewShippingCost.CreatedBy = UserId;

            await _unitOfWork.shippingCostRepository.AddAsync(NewShippingCost);

            var IsNewShippingCostAdded = await _completeAsync();

            if (!IsNewShippingCostAdded) return null;

            _genericMapper.MapSingle(NewShippingCost, shippingCostDto);

            return shippingCostDto;
        }

        public async Task<IEnumerable<ShippingCostDto>> AddRangeAsync(IEnumerable<ShippingCostDto> shippingCostsDtosList, string UserId)
        {
            ParamaterException.CheckIfIEnumerableIsNotNullOrEmpty(shippingCostsDtosList, nameof(shippingCostsDtosList));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(UserId, nameof(UserId));

            var userDto = await _userService.FindByIdAsync(UserId);
            if (userDto is null) return null;

            var NewShippingCostsDtosList = new List<ShippingCostDto>();
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                foreach (var shippingCostDto in shippingCostsDtosList)
                {
                    var NewShippingCostDto = await AddAsync(shippingCostDto, UserId);
                    if (NewShippingCostDto is null) return null;

                    NewShippingCostsDtosList.Add(NewShippingCostDto);

                }

                await _unitOfWork.CommitTransactionAsync();

                return NewShippingCostsDtosList;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<bool> DeleteByIdAsync(long shippingCostId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(shippingCostId, nameof(shippingCostId));

            var IsShippingCostExist = await _unitOfWork.shippingCostRepository.IsExistByIdAsync(shippingCostId);
            if (!IsShippingCostExist) return false;

            await _unitOfWork.shippingCostRepository.DeleteAsync(shippingCostId);

            var IsShippingCostDeleted = await _completeAsync();

            return IsShippingCostDeleted;
        }

        public async Task<ShippingCostDto> FindByCityId(long CityId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(CityId, nameof(CityId));

            var shippingCost = await _unitOfWork.shippingCostRepository.GetByCityIdAsync(CityId);
            if (shippingCost is null) return null;

            var shippingCostDto = _genericMapper.MapSingle<ShippingCost, ShippingCostDto>(shippingCost);
            return shippingCostDto;
        }

        public async Task<ShippingCostDto> FindByIdAsync(long shippingCostId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(shippingCostId, nameof(shippingCostId));

            var shippingCost = await _unitOfWork.shippingCostRepository.GetByIdAsNoTrackingAsync(shippingCostId);
            if (shippingCost is null) return null;

            var shippingCostDto = _genericMapper.MapSingle<ShippingCost, ShippingCostDto>(shippingCost);
            return shippingCostDto;
        }

        public async Task<IEnumerable<ShippingCostDto>> GetAllAsync()
        {

            var shippingCostsList = await _unitOfWork.shippingCostRepository.GetAllAsNoTrackingAsync();

            if (shippingCostsList is null || !shippingCostsList.Any())
                return null;

            var shippingCostsDtosList = _genericMapper.MapCollection<ShippingCost,
                ShippingCostDto>(shippingCostsList);

            return shippingCostsDtosList;
        }

        public async Task<IEnumerable<ShippingCostDto>> GetPagedDataAsync(int pageNumber, int pageSize)
        {
            ParamaterException.CheckIfIntIsBiggerThanZero(pageNumber, nameof(pageNumber));
            ParamaterException.CheckIfIntIsBiggerThanZero(pageSize, nameof(pageSize));

            var shippingCostsList = await _unitOfWork.shippingCostRepository.GetPagedDataAsNoTractingAsync(pageNumber, pageSize);

            if (shippingCostsList is null || !shippingCostsList.Any())
                return null;

            var shippingCostsDtosList = _genericMapper.MapCollection<ShippingCost,
                ShippingCostDto>(shippingCostsList);

            return shippingCostsDtosList;
        }

        public async Task<decimal> GetPriceOfShippingCostByCityId(long CityId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(CityId, nameof(CityId));

            var price = await _unitOfWork.shippingCostRepository.GetPriceOfShippingCostByCityIdAsync(CityId);
            return price;
        }

        public async Task<bool> UpdateByIdAsync(long shippingCostId, ShippingCostDto shippingCostDto)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(shippingCostId, nameof(shippingCostId));
            ParamaterException.CheckIfObjectIfNotNull(shippingCostDto, nameof(shippingCostDto));

            var shippingCostByCityId = await _unitOfWork.shippingCostRepository.GetByCityIdAsync(shippingCostDto.CityId);
            if (shippingCostByCityId != null && shippingCostByCityId.Id != shippingCostId) return false;

            var shippingCart = await _unitOfWork.shippingCostRepository.GetByIdAsNoTrackingAsync(shippingCostId);
            if (shippingCart == null) return false;

            var cityDto = await _cityService.FindByIdAsync(shippingCostDto.CityId);
            if (cityDto == null) return false;

            _genericMapper.MapSingle(shippingCostDto, shippingCart);

            await _unitOfWork.shippingCostRepository.UpdateAsync(shippingCostId, shippingCart);

            var IsShippingCartUpdated = await _completeAsync();

            return IsShippingCartUpdated;

        }
    }
}
