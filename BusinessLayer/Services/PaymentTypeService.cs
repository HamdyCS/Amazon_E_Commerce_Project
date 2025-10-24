using BusinessLayer.Contracks;
using BusinessLayer.Dtos;
using BusinessLayer.Exceptions;
using BusinessLayer.Mapper.Contracks;
using DataAccessLayer.Entities;
using DataAccessLayer.Enums;
using DataAccessLayer.UnitOfWork.Contracks;

namespace BusinessLayer.Servicese
{
    public class PaymentTypeService : IPaymentTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericMapper _genericMapper;

        public PaymentTypeService(IUnitOfWork unitOfWork, IGenericMapper genericMapper)
        {
            this._unitOfWork = unitOfWork;
            this._genericMapper = genericMapper;
        }

        private async Task<bool> _CompleteAsync()
        {
            var NumberOfRowsAfected = await _unitOfWork.CompleteAsync();
            return NumberOfRowsAfected > 0;
        }

        public async Task<PaymentTypeDto> FindByEnPaymentTypeAsync(EnPaymentType enPaymentType)
        {
            ParamaterException.CheckIfObjectIfNotNull(enPaymentType, nameof(enPaymentType));

            var paymentType = await _unitOfWork.paymentTypeRepository.GetByEnPaymentTypeAsync(enPaymentType);
            if (paymentType == null) return null;

            var paymentTypeDto = _genericMapper.MapSingle<PaymentsType, PaymentTypeDto>(paymentType);
            return paymentTypeDto;
        }

        public async Task<PaymentTypeDto> FindByIdAsync(long paymentTypeOd)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(paymentTypeOd, nameof(paymentTypeOd));

            var paymentsType = await _unitOfWork.paymentTypeRepository.GetByIdAsNoTrackingAsync(paymentTypeOd);
            if (paymentsType == null) return null;

            var paymentTypeDto = _genericMapper.MapSingle<PaymentsType, PaymentTypeDto>(paymentsType);
            return paymentTypeDto;
        }

        public async Task<IEnumerable<PaymentTypeDto>> GetAllAsync()
        {

            var paymentsTypesList = await _unitOfWork.paymentTypeRepository.GetAllAsNoTrackingAsync();
            if (paymentsTypesList == null || !paymentsTypesList.Any()) return null;

            var paymentsTypesDtosList = _genericMapper.MapCollection<PaymentsType, PaymentTypeDto>(paymentsTypesList);
            return paymentsTypesDtosList;
        }

        public async Task<bool> UpdateByIdAsync(long paymentTypeId, PaymentTypeDto paymentTypeDto)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(paymentTypeId, nameof(paymentTypeId));
            ParamaterException.CheckIfObjectIfNotNull(paymentTypeDto, nameof(paymentTypeDto));

            var paymentsType = await _unitOfWork.paymentTypeRepository.GetByIdAsNoTrackingAsync(paymentTypeId);
            if (paymentsType == null) return false;

            _genericMapper.MapSingle(paymentTypeDto, paymentsType);

            await _unitOfWork.paymentTypeRepository.UpdateAsync(paymentTypeId, paymentsType);

            var IsPaymentTypeUpdated = await _CompleteAsync();
            return IsPaymentTypeUpdated;

        }
    }
}
