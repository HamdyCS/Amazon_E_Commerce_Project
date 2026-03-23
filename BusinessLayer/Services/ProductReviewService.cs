using BusinessLayer.Contracks;
using BusinessLayer.Dtos;
using BusinessLayer.Exceptions;
using BusinessLayer.Mapper.Contracks;
using DataAccessLayer.Entities;
using DataAccessLayer.Identity.Entities;
using DataAccessLayer.UnitOfWork.Contracks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Servicese
{
    public class ProductReviewService : IProductReviewService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly IProductService _ProductService;
        private readonly IGenericMapper _genericMapper;

        public ProductReviewService(IUnitOfWork unitOfWork, IUserService userService, IProductService productService,
            IGenericMapper genericMapper)
        {
            this._unitOfWork = unitOfWork;
            this._userService = userService;
            this._ProductService = productService;
            this._genericMapper = genericMapper;
        }
        private async Task<bool> _completeAsync()
        {
            var numbersOfRowsAffeted = await _unitOfWork.CompleteAsync();
            return numbersOfRowsAffeted > 0;
        }
        public async Task<ProductReviewDto> AddAsync(ProductReviewDto productReivewDto, string UserId)
        {

            ParamaterException.CheckIfObjectIfNotNull(productReivewDto, nameof(productReivewDto));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(UserId, nameof(UserId));

            var sellerProductDto = await _ProductService.FindByIdAsync(productReivewDto.ProductId);
            if (sellerProductDto == null) return null;

            var userDto = await _userService.FindByIdAsync(UserId);
            if (userDto is null) return null;

            var productReview = _genericMapper.MapSingle<ProductReviewDto, ProductReview>(productReivewDto);
            if (productReview is null) return null;

            productReview.UserId = UserId;
            productReview.CreatedAt = DateTime.UtcNow;

            await _unitOfWork.productReviewRepository.AddAsync(productReview);

            var IsProductReviewAdded = await _completeAsync();
            if (!IsProductReviewAdded) return null;

            _genericMapper.MapSingle(productReview, productReivewDto);

            return productReivewDto;
        }

        public async Task<bool> DeleteByIdAndUserIdAsync(long Id, string UserId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(Id, nameof(Id));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(UserId, nameof(UserId));

            var productReview = await _unitOfWork.productReviewRepository.GetProductReviewByIdAndUserIdAsync(Id, UserId);
            if (productReview is null) return false;

            await _unitOfWork.productReviewRepository.DeleteAsync(Id);

            var IsProductReviewDeleted = await _completeAsync();

            return IsProductReviewDeleted;
        }

        public async Task<bool> DeleteByIdAsync(long Id)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(Id, nameof(Id));

            var ProductReview = await _unitOfWork.productReviewRepository.GetByIdAsNoTrackingAsync(Id);
            if (ProductReview is null) return false;

            await _unitOfWork.productReviewRepository.DeleteAsync(Id);

            var IsProductReviewDeleted = await _completeAsync();

            return IsProductReviewDeleted;
        }

        public async Task<ProductReviewDto> FindByIdAsync(long id)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(id, nameof(id));

            var ProductReview = await _unitOfWork.productReviewRepository.GetByIdAsNoTrackingAsync(id);
            if (ProductReview is null) return null;

            var ProductReviewDto = _genericMapper.MapSingle<ProductReview, ProductReviewDto>(ProductReview);
            return ProductReviewDto;
        }

        public async Task<IEnumerable<ProductReviewDto>> GetAllProductReviewsByProductIdAsync(long ProductId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(ProductId, nameof(ProductId));

            var ProductDto = await _ProductService.FindByIdAsync(ProductId);
            if (ProductDto == null) return null;

            var ProductReviewsList = await _unitOfWork.productReviewRepository.GetAllProductReviewsByProductIdAsync(ProductId);

            if (!ProductReviewsList.Any())
                return null;

            var ProductReviewsDtosList = _genericMapper.MapCollection<ProductReview,
                ProductReviewDto>(ProductReviewsList);

            return ProductReviewsDtosList;
        }

        public async Task<double> GetAverageOfStarsByProductIdAsync(long ProductId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(ProductId, nameof(ProductId));

            var ProductDto = await _ProductService.FindByIdAsync(ProductId);
            if (ProductDto == null) return -1;

            var avg = await _unitOfWork.productReviewRepository.GetAveragedOfStarsByProductIdAsync(ProductId);

            return avg;
        }
   
        public async Task<IEnumerable<ProductReviewDto>> GetPagedProductReviewsByProductIdAsync(int pageNumber, int pageSize, long ProductId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(ProductId, nameof(ProductId));

            var sellerProductDto = await _ProductService.FindByIdAsync(ProductId);
            if (sellerProductDto == null) return null;

            var ProductReviewsList = await _unitOfWork.productReviewRepository.GetPagedProductReviewsWithUserInfoByProductIdAsync(pageNumber,pageSize,ProductId);

            if (!ProductReviewsList.Any())
                return null;

            var ProductReviewsDtosList = _genericMapper.MapCollection<ProductReview,
                ProductReviewDto>(ProductReviewsList);

            return ProductReviewsDtosList;
        }

        public async Task<bool> UpdateByIdAndUserIdAsync(long Id, string UserId, ProductReviewDto ProductReivewDto)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(Id, nameof(Id));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(UserId, nameof(UserId));
            ParamaterException.CheckIfObjectIfNotNull(ProductReivewDto, nameof(ProductReivewDto));

            var ProductReview = await _unitOfWork.productReviewRepository.GetProductReviewByIdAndUserIdAsync(Id, UserId);
            if (ProductReview == null) return false;

            var ProductDto = await _ProductService.FindByIdAsync(ProductReivewDto.ProductId);
            if (ProductDto == null) return false;

            _genericMapper.MapSingle(ProductReivewDto, ProductReview);

            await _unitOfWork.productReviewRepository.UpdateAsync(Id, ProductReview);

            var ProductReviewUpdate = await _completeAsync();

            return ProductReviewUpdate;
        }

       
    }
}
