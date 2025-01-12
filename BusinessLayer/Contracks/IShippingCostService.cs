using BusinessLayer.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Contracks
{
    public interface IShippingCostService
    {
        public Task<ShippingCostDto> FindByIdAsync(long shippingCostId);

        public Task<ShippingCostDto> FindByCityId(long CityId);

        public Task<decimal> GetPriceOfShippingCostByCityId(long CityId);

        public Task<ShippingCostDto> AddAsync(ShippingCostDto shippingCostDto, string UserId);

        public Task<IEnumerable<ShippingCostDto>> AddRangeAsync(IEnumerable<ShippingCostDto> shippingCostsDtosList, string UserId);

        public Task<IEnumerable<ShippingCostDto>> GetAllAsync();

        public Task<IEnumerable<ShippingCostDto>> GetPagedDataAsync(int pageNumber, int pageSize);

        public Task<bool> UpdateByIdAsync(long shippingCostId, ShippingCostDto shippingCostDto);

        public Task<bool> DeleteByIdAsync(long shippingCostId);
    }
}
