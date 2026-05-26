using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Dtos
{
    public class ApplicationOrderSummeryDto
    {
        public long ApplicationId { get; set; }
        public long LastApplicationOrderTypeId { get; set; }
        public DateTime LastApplicationOrderCreatedAt { get; set; }
        public long ShoppingCartId { get; set; }
        public long UserAddressId { get; set; }
        public string UserAddressName { get; set; }
        public decimal TotalPrice { get; set; }
        public long? ReturnApplicatonId { get; set; }
        public DateTime? ReturnApplicationCreatedAt { get; set; }
        public ShoppingCartDto shoppingCartDto { get; set; }
        public string PaymentType { get; set; }
        public string LastApplicationOrderType { get; set; }
        public long ApplicationOrderTypeId { get; set; }

        public DateOnly? EstimatedDeliveryFrom { get; set; }

        public DateOnly? EstimatedDeliveryTo { get; set; }
    }
}
