using BusinessLayer.Contracks;
using BusinessLayer.Dtos;
using BusinessLayer.Exceptions;
using DataAccessLayer.Enums;
using DataAccessLayer.UnitOfWork.Contracks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Servicese
{
    public class OrderApplicationSummaryService : IOrderApplicationSummaryService
    {

        private readonly IUnitOfWork _unitOfWork;

        public OrderApplicationSummaryService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task<OrderApplicationSummaryDto> GetUserOrderApplicationSummaryByUserIdAndApplicationIdAsync(long ApplicationId, string userId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(ApplicationId, nameof(ApplicationId));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(userId, nameof(userId));

            var OrderApplication = await _unitOfWork.applicationRepository.GetByIdAndUserIdAsync(ApplicationId, userId);

            if (OrderApplication is null || OrderApplication.ApplicationTypeId != (long)EnApplicationType.Order) return null;


            var ActiveApplicationOrder = await _unitOfWork.applicationOrderRepository.GetActiveApplicationOrderByApplicationIdAsync(ApplicationId);
            if (ActiveApplicationOrder == null) return null;


            var payment = await _unitOfWork.paymentRepository.GetByIdAsNoTrackingAsync(ActiveApplicationOrder.PaymentId);
            if (payment is null) return null;

            var userAddress = await _unitOfWork.userAdderssRepository.GetByIdAsNoTrackingAsync(payment.UserAddressId);
            if (userAddress is null) return null;

            OrderApplicationSummaryDto orderApplicationSummaryDto = new()
            {
                ApplicationId = ApplicationId,
                LastApplicationOrderTypeId = ActiveApplicationOrder.ApplicationOrderTypeId,
                LastApplicationOrderCreatedAt = ActiveApplicationOrder.CreatedAt,
                ShoppingCartId = payment.ShoppingCartId,
                UserAddressId = userAddress.Id,
                UserAddressName = userAddress.Address,
                TotalPrice = payment.TotalPrice
            };

            if (OrderApplication.ReturnApplicationId == null)
                return orderApplicationSummaryDto;

            var returnApplication = await _unitOfWork.applicationRepository.GetByIdAsNoTrackingAsync((long)OrderApplication.ReturnApplicationId);
            if (returnApplication is null || returnApplication.ApplicationTypeId != (long)EnApplicationType.Return)
                return null;

            orderApplicationSummaryDto.ReturnApplicatonId = returnApplication.Id;
            orderApplicationSummaryDto.ReturnApplicationCreatedAt = returnApplication.CreatedAt;

            return orderApplicationSummaryDto;
        }

        public async Task<IEnumerable<OrderApplicationSummaryDto>> GetAllUserOrderApplicationSummariesByUserIdAsync(string userId)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(userId, nameof(userId));

            var userOrderApplicationsList = await _unitOfWork.applicationRepository.GetAllUserOrderApplicationsByUserIdAsync(userId);
            if (userOrderApplicationsList is null || !userOrderApplicationsList.Any()) return null;

            var orderApplicationSummariesDtosList = new List<OrderApplicationSummaryDto>();
            foreach (var userOrderApplication in userOrderApplicationsList)
            {
                var orderApplicationSummary = await GetUserOrderApplicationSummaryByUserIdAndApplicationIdAsync(userOrderApplication.Id, userId);

                if (orderApplicationSummary != null) orderApplicationSummariesDtosList.Add(orderApplicationSummary);
            }

            return orderApplicationSummariesDtosList;
        }
    }
}
