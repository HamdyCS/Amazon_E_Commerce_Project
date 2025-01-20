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
        Task<ApplicationDto> AddNewOrderApplicationAsync(string userId);

        Task<ApplicationDto> AddNewReturnApplicationAsync(string userId,long OrderApplicationId);

        Task<IEnumerable<ApplicationDto>> GetAllUserApplicationsByUserIdAsync(string UserId);

        Task<IEnumerable<ApplicationDto>> GetAllReturnApplicationsAsync();

        Task<IEnumerable<ApplicationDto>> GetAllUserReturnApplicationsByUserIdAsync(string UserId);

        Task<ApplicationDto> FindByIdAsync(long ApplicationId);

        public Task<ShoppingCartDto> FindShoppingCartByApplicationIdAndUserIdAsync(long ApplicationId, string userId);
       
    }
}
