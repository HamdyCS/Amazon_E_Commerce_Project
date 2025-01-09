using BusinessLayer.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Contracks
{
    public interface ICityWhereDeliveyWorkService 
    {
        public Task<CityWhereDeliveryWorkDto> AddByDeliveryIdAsync(long cityId, string DeliveryId);

        public Task<IEnumerable<CityWhereDeliveryWorkDto>> AddRangeByDeliveryIdAsync(IEnumerable<long> CitiesIds, string DeliveryId);

        public Task<bool> DeleteByIdAndDeliveryIdAsync(long Id, string DeliveryId);

        public Task<bool> DeleteAllCitiesWhereDeliveryWorkByDeliveryIdAsync(string DeliveryId);

        public Task<bool> DeleteRangeByIdAndDeliveryIdAsync(IEnumerable<long> Ids, string DeliveryId);

        public Task<IEnumerable<CityWhereDeliveryWorkDto>> GetAllCitiesWhereDeliveryWorkByDeliveryId(string DeliveryId);

        public Task<bool> UpdateByIdAndDeliveryIdAsync(long Id, string DeliveryId,long CityId);

        public Task<CityWhereDeliveryWorkDto> FindByIdAndDeliveryIdAsync(long Id, string DeliveryId);
        public Task<CityWhereDeliveryWorkDto> FindByIdAsync(long Id);
    }
}
