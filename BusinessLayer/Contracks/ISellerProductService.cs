using BusinessLayer.Dtos;
using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Contracks
{
    public interface ISellerProductService
    {
       
        public Task<SellerProductDto> FindByIdAsync(long id);

        public Task<IEnumerable<SellerProductDto>> GetAllByProductIdOrderByPriceAscAsync(long productId);

        public Task<IEnumerable<SellerProductDto>> GetAllSellerProductsBySellerIdAsync(string sellerId);

        public Task<SellerProductDto> AddAsync(SellerProductDto sellerProductDto, string UserId);

        public Task<IEnumerable<SellerProductDto>> AddRangeAsync(IEnumerable<SellerProductDto> sellerProductDtosList, string UserId);

        public Task<bool> UpdateByIdAndUserIdAsync(long Id,string UserId, SellerProductDto sellerProductDto);

        public Task<bool> DeleteByIdAsync(long Id);

        public Task<bool> DeleteByIdAndUserIdAsync(long Id, string UserId);

        public Task<bool> DeleteRangeByIdAsync(IEnumerable<long> Ids);

        public Task<IEnumerable<SellerProductDto>> GetPagedByProductIdAsync(int pageNumber, int pageSize,long productId);
    }
}
