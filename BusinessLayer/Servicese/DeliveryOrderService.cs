using BusinessLayer.Contracks;
using BusinessLayer.Dtos;
using BusinessLayer.Exceptions;
using BusinessLayer.Mapper.Contracks;
using DataAccessLayer.Entities;
using DataAccessLayer.UnitOfWork.Contracks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Servicese
{
    public class DeliveryOrderService : IDeliveryOrderService
    {
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericMapper _genericMapper;

        public DeliveryOrderService(IUserService userService,IUnitOfWork unitOfWork,IGenericMapper genericMapper)
        {
            this._userService = userService;
            this._unitOfWork = unitOfWork;
            this._genericMapper = genericMapper;
        }
        public async Task<IEnumerable<DeliveryOrderDto>> GetDeliveryOrdersNeedsDeliveryByDeliveryIdAsync(string deliveryId)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(deliveryId,nameof(deliveryId));

            var userDto = await _userService.FindByIdAsync(deliveryId);
            if (userDto == null) return null;

            var deliveryOrdersList = await _unitOfWork.deliveryOrderRepository.GetDeliveryOrdersNeedsDeliveryByDeliveryIdAsync(deliveryId);
            if (deliveryOrdersList is null || !deliveryOrdersList.Any()) return null;

            var deliveryOrdersDtosList = _genericMapper.MapCollection<DeliveryOrder,DeliveryOrderDto>(deliveryOrdersList);
            return deliveryOrdersDtosList;
        }

        public async Task<IEnumerable<DeliveryOrderDto>> GetDeliveryOrdersThatDeliveriedByDeliveryIdAsync(string deliveryId)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(deliveryId, nameof(deliveryId));

            var userDto = await _userService.FindByIdAsync(deliveryId);
            if (userDto == null) return null;

            var deliveryOrdersList = await _unitOfWork.deliveryOrderRepository.GetDeliveryOrdersThatDeliveriedByDeliveryIdAsync(deliveryId);
            if (deliveryOrdersList is null || !deliveryOrdersList.Any()) return null;

            var deliveryOrdersDtosList = _genericMapper.MapCollection<DeliveryOrder, DeliveryOrderDto>(deliveryOrdersList);
            return deliveryOrdersDtosList;
        }
    }
}
