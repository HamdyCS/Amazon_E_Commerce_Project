using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Contracks
{
    public interface IShippingCostRepository : IGenericRepository<ShippingCost>
    {
        Task<ShippingCost> GetByCityIdAsync(long cityId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns>return -1 if null</returns>
        Task<decimal> GetPriceOfShippingCostByCityIdAsync(long cityId);
    }
}
