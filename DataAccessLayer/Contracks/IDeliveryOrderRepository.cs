using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Contracks
{
    public interface IDeliveryOrderRepository
    {
        Task<IEnumerable<DeliveryOrder>> GetDeliveryOrdersNeedsDeliveryByDeliveryIdAsync(string  deliveryId);

        Task<IEnumerable<DeliveryOrder>> GetDeliveryOrdersThatDeliveriedByDeliveryIdAsync(string deliveryId);

    }
}
