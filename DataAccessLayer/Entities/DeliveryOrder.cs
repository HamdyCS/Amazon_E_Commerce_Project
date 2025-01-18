using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public class DeliveryOrder
    {
        public long CityId { get; set; }

        public string CityNameAr { get; set; }

        public string Address {  get; set; }

        public decimal TotalPrice { get; set; }

        public long ApplicationId {  get; set; }

    }
}
