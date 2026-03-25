using DataAccessLayer.Entities;
using DataAccessLayer.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Contracks
{
    public interface ISellerProductRepository : IGenericRepository<SellerProduct>
    {
        Task<IEnumerable<SellerProduct>> GetAllByProductIdOrderByPriceAscAsync(long  ProductId);

        //Task<IEnumerable<SellerProductReview>> GetSellerProductReviewsBySellerProductIdAsync(long SellerProductId);

        Task<SellerProduct> GetTheCheaperSellerProductByProductIdAsync(long ProductId);

        public Task<IEnumerable<SellerProduct>> GetAllSellerProductsBySellerIdAsync(string sellerId);

        public Task<IEnumerable<SellerProduct>> GetPagedDataAsNoTrackingByProductIdAsync(int pageNumber, int pageSize, long productId);

        public Task<SellerProduct> GetSellerProductByIdAndSellerIdAsync(long sellerProductId,string  sellerId);

        Task<PaginationResult<SellerProduct>> GetPagedByProductSubCategoryIdAsync(long productSubCategoryId, int pageNumber, int pageSize);

        Task<PaginationResult<SellerProduct>> GetPagedByProductCategoryIdAsync(long productCategoryId, int pageNumber, int pageSize);

        Task<PaginationResult<SellerProduct>> GetPagedByBrandIdAsync(long brandId, int pageNumber, int pageSize);

    }
}
