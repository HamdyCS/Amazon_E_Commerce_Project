using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Dtos
{
    public class ApplicationOrderDto
    {
        public long Id { get; set; }

        public long ApplicationId { get; set; }

        public long ApplicationOrderTypeId { get; set; }

        public long ShoppingCartId { get; set; }

        public long PaymentId { get; set; }

        public string? DeliveryId { get; set; }
    }
}
