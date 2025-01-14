using DataAccessLayer.Entities;
using DataAccessLayer.Enums;

namespace DataAccessLayer.Contracks
{
    public interface IApplicationOrderTypeRepository : IGenericRepository<ApplicationOrderType>
    {
        public Task<ApplicationOrderType> GetByEnApplicationOrderTypeAsync(EnApplicationOrderType enApplicationOrderType);
    }
}
