using BusinessLayer.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Contracks
{
    public interface IApplicationOrderSummaryService
    {
        Task<IEnumerable<ApplicationOrderSummeryDto>> GetAllUserApplicationOrderSummariesAsync(string userId);
        Task<ApplicationOrderSummeryDto> GetUserApplicationOrderSummaryByUserIdAndApplicationIdAsync(long ApplicationId,string userId);
        Task <ApplicationOrderSummeryDto> GetLatestUserApplicationOrderSummaryAsync(string userId);
    }
}
