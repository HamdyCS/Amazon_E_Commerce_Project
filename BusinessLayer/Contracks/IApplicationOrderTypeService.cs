using BusinessLayer.Dtos;
using DataAccessLayer.Enums;

namespace BusinessLayer.Contracks
{
    public interface IApplicationOrderTypeService
    {
        Task<ApplicationOrderTypeDto> FindByIdAsync(long ApplicationOrderTypeId);
        Task<IEnumerable<ApplicationOrderTypeDto>> GetAllAsync();
        Task<ApplicationOrderTypeDto> FindByEnApplicationOrderTypeAsync(EnApplicationOrderType enApplicationOrderType);
        Task<bool> UpdateByIdAsync(long ApplicationOrderTypeId, ApplicationOrderTypeDto applicationOrderTypeDto);
    }
}
