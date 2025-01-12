using BusinessLayer.Contracks;
using BusinessLayer.Dtos;
using BusinessLayer.Exceptions;
using BusinessLayer.Mapper.Contracks;
using DataAccessLayer.Entities;
using DataAccessLayer.Enums;
using DataAccessLayer.UnitOfWork.Contracks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Servicese
{
    public class ApplicationTypeService : IApplicationTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericMapper _genericMapper;

        public ApplicationTypeService(IUnitOfWork unitOfWork,IGenericMapper genericMapper)
        {
            this._unitOfWork = unitOfWork;
            this._genericMapper = genericMapper;
        }

        private async Task<bool> _CompleteAsync()
        {
            var NumberOfRowsAfected = await _unitOfWork.CompleteAsync();
            return NumberOfRowsAfected > 0;
        }

        public async Task<ApplicationTypeDto> FindByEnApplicationTypeAsync(EnApplicationType enApplicationType)
        {
            ParamaterException.CheckIfObjectIfNotNull(enApplicationType, nameof(enApplicationType));

            var applicationType = await _unitOfWork.applicationTypeRepository.GetByEnApplicationTypeAsync(enApplicationType);
            if (applicationType == null) return null;

            var applicationTypeDto = _genericMapper.MapSingle<ApplicationType,ApplicationTypeDto>(applicationType);
            return applicationTypeDto;
        }

        public async Task<ApplicationTypeDto> FindByIdAsync(long ApplicationTypeId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(ApplicationTypeId, nameof(ApplicationTypeId));

            var applicationType = await _unitOfWork.applicationTypeRepository.GetByIdAsNoTrackingAsync(ApplicationTypeId);
            if (applicationType == null) return null;

            var applicationTypeDto = _genericMapper.MapSingle<ApplicationType, ApplicationTypeDto>(applicationType);
            return applicationTypeDto;
        }

        public async Task<IEnumerable<ApplicationTypeDto>> GetAllAsync()
        {

            var applicationTypesList = await _unitOfWork.applicationTypeRepository.GetAllAsNoTrackingAsync();
            if (applicationTypesList == null || !applicationTypesList.Any()) return null;

            var applicationTypesDtosList = _genericMapper.MapCollection<ApplicationType, ApplicationTypeDto>(applicationTypesList);
            return applicationTypesDtosList;
        }

        public async Task<bool> UpdateByIdAsync(long ApplicationTypeId, ApplicationTypeDto applicationTypeDto)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(ApplicationTypeId, nameof(ApplicationTypeId));
            ParamaterException.CheckIfObjectIfNotNull(applicationTypeDto, nameof(applicationTypeDto));

            var applicationType = await _unitOfWork.applicationTypeRepository.GetByIdAsNoTrackingAsync(ApplicationTypeId);
            if (applicationType == null) return false;

            _genericMapper.MapSingle(applicationTypeDto,applicationType);

            await _unitOfWork.applicationTypeRepository.UpdateAsync(ApplicationTypeId, applicationType);

            var IsApplicationTypeUpdated = await _CompleteAsync();
            return IsApplicationTypeUpdated;

        }
    }
}
