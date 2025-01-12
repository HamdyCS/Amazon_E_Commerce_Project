using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Dtos;
using DataAccessLayer.Enums;

namespace BusinessLayer.Contracks
{
    public interface IApplicationTypeService
    {
        Task<ApplicationTypeDto> FindByIdAsync(long ApplicationTypeId);
        Task<IEnumerable<ApplicationTypeDto>> GetAllAsync();
        Task<ApplicationTypeDto> FindByEnApplicationTypeAsync(EnApplicationType enApplicationType);
        Task<bool> UpdateByIdAsync(long ApplicationTypeId, ApplicationTypeDto applicationTypeDto);
    }
}
