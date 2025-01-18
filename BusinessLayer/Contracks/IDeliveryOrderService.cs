using BusinessLayer.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Contracks
{
    public interface IDeliveryOrderService
    {
        Task<IEnumerable<DeliveryOrderDto>> GetDeliveryOrdersNeedsDeliveryByDeliveryIdAsync(string deliveryId);

        Task<IEnumerable<DeliveryOrderDto>> GetDeliveryOrdersThatDeliveriedByDeliveryIdAsync(string deliveryId);
    }
}
