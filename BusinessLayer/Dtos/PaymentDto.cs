using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Dtos
{
    public class PaymentDto
    {
        public long Id { get; set; }

        public long UserAddressId { get; set; }

        public long ShoppingCartId { get; set; }
   
        public long? PaymentStatusId { get; set; }

    }

}
