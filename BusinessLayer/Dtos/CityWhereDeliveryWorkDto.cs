using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Dtos
{
    public class CityWhereDeliveryWorkDto
    {
        public long Id { get; set; }

        public long CityId { get; set; }

        public string CityNameEn {  get; set; }

        public string CityNameAr { get; set; }

    }
}
