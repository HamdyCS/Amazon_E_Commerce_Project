using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Contracks
{
    public interface IApplicationRepository : IGenericRepository<Application>
    {
        Task<IEnumerable<Application>> GetAllUserApplicationsByUserIdAsync(string UserId);
        Task<Application> GetByIdAndUserIdAsync(long ApplicationId, string userId);
        Task<long> GetShoppingCartIdByApplicationIdAndUserIdAsync(long ApplicationId, string userId);
        Task<IEnumerable<Application>> GetAllReturnApplicationsAsync();
        Task<IEnumerable<Application>> GetAllUserReturnApplicationsByUserIdAsync(string UserId);
        Task<IEnumerable<Application>> GetAllUserOrderApplicationsByUserIdAsync(string UserId);
    }


}
