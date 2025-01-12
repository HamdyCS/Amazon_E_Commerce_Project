using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Dtos
{
    public class ShippingCostDto
    {
        public long Id { get; set; }

        [Range(0,double.MaxValue,ErrorMessage = "Price must be equal zero or bigger than zero")]
        public decimal Price { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        [Range(1,double.MaxValue,ErrorMessage ="Id must be bigger than zero")]
        public long CityId { get; set; }


    }
}
