using BusinessLayer.Contracks;
using BusinessLayer.Dtos;
using BusinessLayer.Exceptions;
using BusinessLayer.Mapper.Contracks;
using DataAccessLayer.Entities;
using DataAccessLayer.Enums;
using DataAccessLayer.UnitOfWork.Contracks;

namespace BusinessLayer.Servicese
{
    public class ApplicationOrderTypeService : IApplicationOrderTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericMapper _genericMapper;

        public ApplicationOrderTypeService(IUnitOfWork unitOfWork, IGenericMapper genericMapper)
        {
            this._unitOfWork = unitOfWork;
            this._genericMapper = genericMapper;
        }

        private async Task<bool> _CompleteAsync()
        {
            var NumberOfRowsAfected = await _unitOfWork.CompleteAsync();
            return NumberOfRowsAfected > 0;
        }

        public async Task<ApplicationOrderTypeDto> FindByEnApplicationOrderTypeAsync(EnApplicationOrderType enApplicationOrderType)
        {
            ParamaterException.CheckIfObjectIfNotNull(enApplicationOrderType, nameof(enApplicationOrderType));

            var applicationOrderType = await _unitOfWork.applicationOrderTypeRepository.GetByEnApplicationOrderTypeAsync(enApplicationOrderType);
            if (applicationOrderType == null) return null;

            var applicationOrderTypeDto = _genericMapper.MapSingle<ApplicationOrderType, ApplicationOrderTypeDto>(applicationOrderType);
            return applicationOrderTypeDto;
        }

        public async Task<ApplicationOrderTypeDto> FindByIdAsync(long ApplicationOrderTypeId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(ApplicationOrderTypeId, nameof(ApplicationOrderTypeId));

            var applicationOrderType = await _unitOfWork.applicationOrderTypeRepository.GetByIdAsNoTrackingAsync(ApplicationOrderTypeId);
            if (applicationOrderType == null) return null;

            var applicationOrderTypeDto = _genericMapper.MapSingle<ApplicationOrderType, ApplicationOrderTypeDto>(applicationOrderType);
            return applicationOrderTypeDto;
        }

        public async Task<IEnumerable<ApplicationOrderTypeDto>> GetAllAsync()
        {

            var applicationOrderTypesList = await _unitOfWork.applicationOrderTypeRepository.GetAllAsNoTrackingAsync();
            if (applicationOrderTypesList == null || !applicationOrderTypesList.Any()) return null;

            var applicationOrderTypesDtosList = _genericMapper.MapCollection<ApplicationOrderType, ApplicationOrderTypeDto>(applicationOrderTypesList);
            return applicationOrderTypesDtosList;
        }

        public async Task<bool> UpdateByIdAsync(long ApplicationOrderTypeId, ApplicationOrderTypeDto applicationOrderTypeDto)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(ApplicationOrderTypeId, nameof(ApplicationOrderTypeId));
            ParamaterException.CheckIfObjectIfNotNull(applicationOrderTypeDto, nameof(applicationOrderTypeDto));

            var applicationOrderType = await _unitOfWork.applicationOrderTypeRepository.GetByIdAsNoTrackingAsync(ApplicationOrderTypeId);
            if (applicationOrderType == null) return false;

            _genericMapper.MapSingle(applicationOrderTypeDto, applicationOrderType);

            await _unitOfWork.applicationOrderTypeRepository.UpdateAsync(ApplicationOrderTypeId, applicationOrderType);

            var IsApplicationOrderTypeUpdated = await _CompleteAsync();
            return IsApplicationOrderTypeUpdated;

        }
    }
}
