using DataAccessLayer.Entities;
using DataAccessLayer.Identity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Contracks
{
    public interface ICityWhereDeliveyWorkRepository : IGenericRepository<CityWhereDeliveryWork>
    {
        Task<IEnumerable<CityWhereDeliveryWork>> GetAllCitiesWhereDeliveryWorkByIdAsync(string DeliveryId);

        Task DeleteAllCitiesWhereDeliveryWorkByDeliveryIdAsync(string DeliveryId);

        Task<CityWhereDeliveryWork> GetByIdAndDeliveryIdAsync(long Id, string DeliveryId);

        Task<bool> IsThisUserWorkInThisCityAsync(long cityId, string DeliveryId);

        Task<IEnumerable<User>> GetAllUserWhoWorkInThisCityByCityIdAsync(long cityId);
    }
}
