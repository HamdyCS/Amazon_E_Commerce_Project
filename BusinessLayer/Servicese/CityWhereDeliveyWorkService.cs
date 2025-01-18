using BusinessLayer.Contracks;
using BusinessLayer.Dtos;
using BusinessLayer.Exceptions;
using BusinessLayer.Mapper.Contracks;
using DataAccessLayer.Entities;
using DataAccessLayer.Identity.Entities;
using DataAccessLayer.UnitOfWork.Contracks;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Servicese
{
    public class CityWhereDeliveyWorkService : ICityWhereDeliveyWorkService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly ICityService _cityService;
        private readonly IGenericMapper _genericMapper;
        private readonly IPersonService _personService;

        public CityWhereDeliveyWorkService(IUnitOfWork unitOfWork, IUserService userService, ICityService cityService,
            IGenericMapper genericMapper,IPersonService personService)
        {
            this._unitOfWork = unitOfWork;
            this._userService = userService;
            this._cityService = cityService;
            this._genericMapper = genericMapper;
            this._personService = personService;
        }

        private async Task<bool> _CompleteAsync()
        {
            var NumberOfRowsAfected = await _unitOfWork.CompleteAsync();
            return NumberOfRowsAfected > 0;
        }

        public async Task<CityWhereDeliveryWorkDto> AddByDeliveryIdAsync(long cityId, string DeliveryId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(cityId, nameof(cityId));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(DeliveryId, nameof(DeliveryId));

            var userDto = await _userService.FindByIdAsync(DeliveryId);
            if (userDto == null) return null;

            var cityDto = await _cityService.FindByIdAsync(cityId);
            if (cityDto == null) return null;

            var IsThisDeliveryWorkInThisCity = await _unitOfWork.CitiyWhereDeliveyWorkRepository.IsThisUserWorkInThisCityAsync(cityId, DeliveryId);
            if (IsThisDeliveryWorkInThisCity) return null;

            var cityWhereDeliveryWork = new CityWhereDeliveryWork
            {
                CityId = cityId,
                DeliveryId = DeliveryId
            };

            await _unitOfWork.CitiyWhereDeliveyWorkRepository.AddAsync(cityWhereDeliveryWork);

            var IsCityWhereDeliveyWorkAdded = await _CompleteAsync();

            if (!IsCityWhereDeliveyWorkAdded) return null;


            var NewCityWhereDeliveryWorkDto = new CityWhereDeliveryWorkDto();
            _genericMapper.MapSingle(cityWhereDeliveryWork, NewCityWhereDeliveryWorkDto);
            _genericMapper.MapSingle(cityDto, NewCityWhereDeliveryWorkDto);

            return NewCityWhereDeliveryWorkDto;
        }

        public async Task<IEnumerable<CityWhereDeliveryWorkDto>> AddRangeByDeliveryIdAsync(IEnumerable<long> CitiesIds, string DeliveryId)
        {
            ParamaterException.CheckIfIEnumerableIsNotNullOrEmpty(CitiesIds, nameof(CitiesIds));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(DeliveryId, nameof(DeliveryId));

            var userDto = await _userService.FindByIdAsync(DeliveryId);
            if (userDto == null) return null;

            var NewCitiesWhereDeliveryWorkDtosList = new List<CityWhereDeliveryWorkDto>();

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                foreach (var cityId in CitiesIds)
                {
                    var NewCityWhereDeliveryWorkDto = await AddByDeliveryIdAsync(cityId, DeliveryId);
                    if(NewCityWhereDeliveryWorkDto is null)
                    {
                        await _unitOfWork.RollbackTransactionAsync();
                        return null;
                    }

                    NewCitiesWhereDeliveryWorkDtosList.Add(NewCityWhereDeliveryWorkDto);
                }

                await _unitOfWork.CommitTransactionAsync();

                return NewCitiesWhereDeliveryWorkDtosList;
            }
            catch 
            {
                await _unitOfWork.RollbackTransactionAsync();
                return null;
            }
        }

        public async Task<bool> DeleteByIdAndDeliveryIdAsync(long Id, string DeliveryId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(Id, nameof(Id));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(DeliveryId, nameof(DeliveryId));

            var userDto = await _userService.FindByIdAsync(DeliveryId);
            if (userDto == null) return false;

            var cityWhereDeliveryWork = await _unitOfWork.CitiyWhereDeliveyWorkRepository.GetByIdAndDeliveryIdAsync(Id, DeliveryId);

            if (cityWhereDeliveryWork is null) return false;

            await _unitOfWork.CitiyWhereDeliveyWorkRepository.DeleteAsync(Id);

            var IsCityWhereDeliveryWorkDeleted = await _CompleteAsync();

            return IsCityWhereDeliveryWorkDeleted;
        }

        public async Task<bool> DeleteRangeByIdAndDeliveryIdAsync(IEnumerable<long> Ids, string DeliveryId)
        {
            ParamaterException.CheckIfIEnumerableIsNotNullOrEmpty(Ids, nameof(Ids));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(DeliveryId, nameof(DeliveryId));

            var userDto = await _userService.FindByIdAsync(DeliveryId);
            if (userDto == null) return false;

            foreach (var id in Ids)
            {
                var cityWhereDeliveryWork = await _unitOfWork.CitiyWhereDeliveyWorkRepository.GetByIdAndDeliveryIdAsync(id, DeliveryId);
                if(cityWhereDeliveryWork is null) return false;
            }

            await _unitOfWork.CitiyWhereDeliveyWorkRepository.DeleteRangeAsync(Ids);

            var IsCitiesWhereDeliveryWorkDeleted = await _CompleteAsync();

            return IsCitiesWhereDeliveryWorkDeleted;
        }

        public async Task<bool> DeleteAllCitiesWhereDeliveryWorkByDeliveryIdAsync(string DeliveryId)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(DeliveryId, nameof(DeliveryId));

            var userDto = await _userService.FindByIdAsync(DeliveryId);
            if (userDto == null) return false;

            var CitiesWhereDeliveryWorkList = await _unitOfWork.CitiyWhereDeliveyWorkRepository.GetAllCitiesWhereDeliveryWorkByIdAsync(DeliveryId);

            await _unitOfWork.CitiyWhereDeliveyWorkRepository.DeleteAllCitiesWhereDeliveryWorkByDeliveryIdAsync(DeliveryId);

            var IsCitiesWhereDeliveryWorkDeleted = await _CompleteAsync();

            return IsCitiesWhereDeliveryWorkDeleted;
        }

        public async Task<IEnumerable<CityWhereDeliveryWorkDto>> GetAllCitiesWhereDeliveryWorkByDeliveryId(string DeliveryId)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(DeliveryId, nameof(DeliveryId));
            var CitiesWhereDeliveryWorkList = await _unitOfWork.CitiyWhereDeliveyWorkRepository.GetAllCitiesWhereDeliveryWorkByIdAsync(DeliveryId);

            if (CitiesWhereDeliveryWorkList is null || !CitiesWhereDeliveryWorkList.Any()) return null;

            var CitiesWhereDeliveryWorkDtosList = _genericMapper.MapCollection<CityWhereDeliveryWork
                ,CityWhereDeliveryWorkDto>(CitiesWhereDeliveryWorkList);


            foreach(var cityWhereDeliveryWork in CitiesWhereDeliveryWorkDtosList)
            {
                var cityDto = await _cityService.FindByIdAsync(cityWhereDeliveryWork.CityId);
                if (cityDto is null) return null;

                _genericMapper.MapSingle(cityDto, cityWhereDeliveryWork);
            }

            
            return CitiesWhereDeliveryWorkDtosList;
        }

        public async Task<bool> UpdateByIdAndDeliveryIdAsync(long Id, string DeliveryId, long CityId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(Id, nameof(Id));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(DeliveryId, nameof(DeliveryId));
            ParamaterException.CheckIfLongIsBiggerThanZero(CityId, nameof(CityId));

            var userDto = await _userService.FindByIdAsync(DeliveryId);
            if (userDto == null) return false;

            var cityDto = await _cityService.FindByIdAsync(CityId);
            if (cityDto == null) return false;

           
            var cityWhereDeliveryWork = await _unitOfWork.CitiyWhereDeliveyWorkRepository.GetByIdAndDeliveryIdAsync(Id, DeliveryId);
            if(cityWhereDeliveryWork == null) return false;

            var IsThisDeliveryWorkInThisCity = await _unitOfWork.CitiyWhereDeliveyWorkRepository.IsThisUserWorkInThisCityAsync(CityId, DeliveryId);
            if (IsThisDeliveryWorkInThisCity) return false;


            cityWhereDeliveryWork.CityId = CityId;

            await _unitOfWork.CitiyWhereDeliveyWorkRepository.UpdateAsync(Id,cityWhereDeliveryWork);

            var IsCityWhereDeliveryWorkUpdated = await _CompleteAsync();

            return IsCityWhereDeliveryWorkUpdated;
        }
       
        public async Task<CityWhereDeliveryWorkDto> FindByIdAndDeliveryIdAsync(long Id, string DeliveryId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(Id, nameof(Id));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(DeliveryId, nameof(DeliveryId));

            var userDto = await _userService.FindByIdAsync(DeliveryId);
            if (userDto == null) return null;

            var cityWhereDeliveryWork = await _unitOfWork.CitiyWhereDeliveyWorkRepository.GetByIdAndDeliveryIdAsync(Id, DeliveryId);
            if (cityWhereDeliveryWork == null) return null;

            var cityDto = await _cityService.FindByIdAsync(cityWhereDeliveryWork.CityId);
            if (cityDto == null) return null;

            var cityWhereDeliveryWorkDto = _genericMapper.MapSingle<CityWhereDeliveryWork,CityWhereDeliveryWorkDto>(cityWhereDeliveryWork);

            _genericMapper.MapSingle(cityDto, cityWhereDeliveryWorkDto);

            return cityWhereDeliveryWorkDto;

        }

        public async Task<CityWhereDeliveryWorkDto> FindByIdAsync(long Id)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(Id, nameof(Id));

          

            var cityWhereDeliveryWork = await _unitOfWork.CitiyWhereDeliveyWorkRepository.GetByIdAsNoTrackingAsync(Id);
            if (cityWhereDeliveryWork == null) return null;

            var cityDto = await _cityService.FindByIdAsync(cityWhereDeliveryWork.CityId);
            if (cityDto == null) return null;

            var cityWhereDeliveryWorkDto = _genericMapper.MapSingle<CityWhereDeliveryWork, CityWhereDeliveryWorkDto>(cityWhereDeliveryWork);

            _genericMapper.MapSingle(cityDto, cityWhereDeliveryWorkDto);

            return cityWhereDeliveryWorkDto;
        }

        public async Task<IEnumerable<UserDto>> GetAllUserWhoWorkInThisCityByCityIdAsync(long cityId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(cityId, nameof(cityId));

            var cityDto = await _cityService.FindByIdAsync(cityId);
            if (cityDto == null) return null;

            var usersWhoWorkInThisCityList = await _unitOfWork.CitiyWhereDeliveyWorkRepository.GetAllUserWhoWorkInThisCityByCityIdAsync(cityId);

            if (usersWhoWorkInThisCityList is null || !usersWhoWorkInThisCityList.Any()) return null;

            var usersWhoWorkInThisCityDtosList = new List<UserDto>();

            foreach (var user in usersWhoWorkInThisCityList)
            {
                var userDto = _genericMapper.MapSingle<User, UserDto>(user);
                if (userDto is null) return null;

                var person = await _personService.FindByIdAsync(user.PersonId);
                if (person is null) return null;

                _genericMapper.MapSingle(person, userDto);

                usersWhoWorkInThisCityDtosList.Add(userDto);
            }

            return usersWhoWorkInThisCityDtosList;
        }
    }

}
