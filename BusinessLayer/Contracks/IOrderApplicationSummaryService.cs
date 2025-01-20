using BusinessLayer.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Contracks
{
    public interface IOrderApplicationSummaryService
    {
        Task<IEnumerable<OrderApplicationSummaryDto>> GetAllUserOrderApplicationSummariesByUserIdAsync(string userId);
        Task<OrderApplicationSummaryDto> GetUserOrderApplicationSummaryByUserIdAndApplicationIdAsync(long ApplicationId,string userId);
    }
}
