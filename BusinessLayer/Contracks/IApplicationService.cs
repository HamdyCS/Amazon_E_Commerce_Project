using BusinessLayer.Dtos;
using DataAccessLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Contracks
{
    public interface IApplicationService
    {
        Task<ApplicationDto> AddNewAsync(string userId,EnApplicationType enApplicationType);

        Task<IEnumerable<ApplicationDto>> GetAllUserApplicationsByUserIdAsync(string UserId);
        Task<ApplicationDto> FindByIdAsync(long ApplicationId);
    }
}
