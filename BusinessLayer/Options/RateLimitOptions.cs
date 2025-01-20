using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Options
{
    public class RateLimitOptions
    {
        public int PermitLimit { get; set; }// عدد الطلبات المسموحة
        public int Window { get; set; }// عدد الثواني
        public int QueueLimit { get; set; }// العدد في الطبور

    }
}
