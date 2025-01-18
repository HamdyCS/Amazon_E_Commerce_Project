using BusinessLayer.Dtos;
using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Contracks
{
    public interface IApplicationOrderService
    {
        Task<ApplicationOrderDto> FindByIdAsync(long ApplicationOrderId);//admin
        Task<ApplicationOrderDto> FindActiveApplicationOrderByApplicationIdAndUserIdAsync(long ApplicationId,string userId);//customer
        Task<IEnumerable<ApplicationOrderDto>> TrackApplicationOrderByApplicatonIdAsync(long ApplicationId);//admin
        Task<IEnumerable<ApplicationOrderDto>> TrackApplicationOrderByApplicatonIdAndUserIdAsync(long ApplicationId,string userId);//customer
        Task<IEnumerable<ApplicationOrderDto>> GetActiveUnderProcessingApplicationOrdersAsync();//admin
        Task<IEnumerable<ApplicationOrderDto>> GetActiveShippedApplicationOrdersAsync();//admin
        Task<IEnumerable<ApplicationOrderDto>> GetActiveDeliveredApplicationOrdersAsync();//admin
        Task<ApplicationOrderDto> AddNewUnderProcessingApplicationOrderAsync(long ShoppingCartId,long PaymentId, string UserId);//customer
        Task<ApplicationOrderDto> AddNewDeliveredApplicationOrderAsync(long ApplicationId);//admin
        Task<ApplicationOrderDto> AddNewShippedApplicationOrderAsync(long ApplicationId,string DeliveredId);//admin
    }
}
