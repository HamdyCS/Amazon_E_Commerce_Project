using DataAccessLayer.Entities;
using DataAccessLayer.Enums;

namespace DataAccessLayer.Contracks
{
    public interface IPaymentTypeRepository : IGenericRepository<PaymentsType>
    {
        public Task<PaymentsType> GetByEnPaymentTypeAsync(EnPaymentType enPaymentType);
    }
}
