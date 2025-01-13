using BusinessLayer.Dtos;
using DataAccessLayer.Enums;

namespace BusinessLayer.Contracks
{
    public interface IPaymentTypeService
    {
        Task<PaymentTypeDto> FindByIdAsync(long PaymentTypeId);
        Task<IEnumerable<PaymentTypeDto>> GetAllAsync();
        Task<PaymentTypeDto> FindByEnPaymentTypeAsync(EnPaymentType enPaymentType);
        Task<bool> UpdateByIdAsync(long PaymentTypeId, PaymentTypeDto paymentTypeDto);
    }
}
