using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Contracks
{
    public interface IApplicationOrderRepository : IGenericRepository<ApplicationOrder>
    {
        Task<IEnumerable<ApplicationOrder>> GetActiveUnderProcessingApplicationOrdersAsync();
        Task<IEnumerable<ApplicationOrder>> GetActiveShippedApplicationOrdersAsync();
        Task<IEnumerable<ApplicationOrder>> GetActiveDeliveredApplicationOrdersAsync();
        Task<ApplicationOrder> GetActiveApplicationOrderByApplicationIdAsync(long ApplicationId);

        Task<IEnumerable<ApplicationOrder>> GetAllApplicationOrdersByApplicatonIdAsync(long ApplicationId);
    }
}
