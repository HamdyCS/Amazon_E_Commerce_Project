using BusinessLayer.Contracks;
using BusinessLayer.Dtos;
using BusinessLayer.Exceptions;
using BusinessLayer.Mapper.Contracks;
using DataAccessLayer.Entities;
using DataAccessLayer.Enums;
using DataAccessLayer.Identity.Entities;
using DataAccessLayer.UnitOfWork.Contracks;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Servicese
{
    public class ApplicationService : IApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericMapper _genericMapper;
        private readonly IUserService _userService;

        public ApplicationService(IUnitOfWork unitOfWork, IGenericMapper genericMapper, IUserService userService)
        {
            this._unitOfWork = unitOfWork;
            this._genericMapper = genericMapper;
            this._userService = userService;
        }

        private async Task<bool> _CompleteAsync()
        {
            var NumberOfRowsAfected = await _unitOfWork.CompleteAsync();
            return NumberOfRowsAfected > 0;
        }

        public async Task<ApplicationDto> AddNewAsync(string userId, EnApplicationType enApplicationType)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(userId, nameof(userId));
            ParamaterException.CheckIfObjectIfNotNull(enApplicationType, nameof(enApplicationType));

            var userDto = await _userService.FindByIdAsync(userId);
            if (userDto == null) return null;

            var application = new Application()
            {
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                ApplicationTypeId = (long)enApplicationType
            };

            await _unitOfWork.applicationRepository.AddAsync(application);

            var IsApplicationAdded = await _CompleteAsync();
            if (!IsApplicationAdded) return null;

            var applicationDto = _genericMapper.MapSingle<Application, ApplicationDto>(application);
            return applicationDto;

        }

        public async Task<ApplicationDto> FindByIdAsync(long ApplicationId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(ApplicationId, nameof(ApplicationId));

            var application = await _unitOfWork.applicationRepository.GetByIdAsNoTrackingAsync(ApplicationId);
            if (application is null) return null;

            var applicationDto = _genericMapper.MapSingle<Application, ApplicationDto>(application);
            return applicationDto;
        }

        public async Task<IEnumerable<ApplicationDto>> GetAllUserApplicationsByUserIdAsync(string UserId)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(UserId, nameof(UserId));

            var userDto = await _userService.FindByIdAsync(UserId);
            if (userDto == null) return null;

            var applicationsList = await _unitOfWork.applicationRepository.GetAllUserApplicationsByUserIdAsync(UserId);
            if (applicationsList is null || !applicationsList.Any()) return null;

            var applicationsDtosList = _genericMapper.MapCollection<Application, ApplicationDto>(applicationsList);
            return applicationsDtosList;
        }


    }
}
