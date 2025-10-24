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
    public class SellerProductReviewService : ISellerProductReviewService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly ISellerProductService _sellerProductService;
        private readonly IGenericMapper _genericMapper;

        public SellerProductReviewService(IUnitOfWork unitOfWork, IUserService userService, ISellerProductService sellerProductService,
            IGenericMapper genericMapper)
        {
            this._unitOfWork = unitOfWork;
            this._userService = userService;
            this._sellerProductService = sellerProductService;
            this._genericMapper = genericMapper;
        }
        private async Task<bool> _completeAsync()
        {
            var numbersOfRowsAffeted = await _unitOfWork.CompleteAsync();
            return numbersOfRowsAffeted > 0;
        }
        public async Task<SellerProductReviewDto> AddAsync(SellerProductReviewDto sellerProductReivewDto, string UserId)
        {

            ParamaterException.CheckIfObjectIfNotNull(sellerProductReivewDto, nameof(sellerProductReivewDto));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(UserId, nameof(UserId));

            var sellerProductDto = await _sellerProductService.FindByIdAsync(sellerProductReivewDto.SellerProductId);
            if (sellerProductDto == null) return null;

            var userDto = await _userService.FindByIdAsync(UserId);
            if (userDto is null) return null;

            var sellerProductReview = _genericMapper.MapSingle<SellerProductReviewDto, SellerProductReview>(sellerProductReivewDto);
            if (sellerProductReview is null) return null;

            sellerProductReview.UserId = UserId;
            sellerProductReview.CreatedAt = DateTime.UtcNow;

            await _unitOfWork.sellerProductReviewRepository.AddAsync(sellerProductReview);

            var IsSellerProductReviewAdded = await _completeAsync();
            if (!IsSellerProductReviewAdded) return null;

            _genericMapper.MapSingle(sellerProductReview, sellerProductReivewDto);

            return sellerProductReivewDto;
        }

        public async Task<bool> DeleteByIdAndUserIdAsync(long Id, string UserId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(Id, nameof(Id));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(UserId, nameof(UserId));

            var sellerProductReview = await _unitOfWork.sellerProductReviewRepository.GetSellerProductReviewByIdAndUserIdAsync(Id, UserId);
            if (sellerProductReview is null) return false;

            await _unitOfWork.sellerProductReviewRepository.DeleteAsync(Id);

            var IsSellerProductReviewDeleted = await _completeAsync();

            return IsSellerProductReviewDeleted;
        }

        public async Task<bool> DeleteByIdAsync(long Id)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(Id, nameof(Id));

            var sellerProductReview = await _unitOfWork.sellerProductReviewRepository.GetByIdAsNoTrackingAsync(Id);
            if (sellerProductReview is null) return false;

            await _unitOfWork.sellerProductReviewRepository.DeleteAsync(Id);

            var IsSellerProductReviewDeleted = await _completeAsync();

            return IsSellerProductReviewDeleted;
        }

        public async Task<SellerProductReviewDto> FindByIdAsync(long id)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(id, nameof(id));

            var sellerProductReview = await _unitOfWork.sellerProductReviewRepository.GetByIdAsNoTrackingAsync(id);
            if (sellerProductReview is null) return null;

            var sellerProductReviewDto = _genericMapper.MapSingle<SellerProductReview, SellerProductReviewDto>(sellerProductReview);
            return sellerProductReviewDto;
        }

        public async Task<IEnumerable<SellerProductReviewDto>> GetAllSellerProductReviewsBySellerProductIdAsync(long SellerProductId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(SellerProductId, nameof(SellerProductId));

            var sellerProductDto = await _sellerProductService.FindByIdAsync(SellerProductId);
            if (sellerProductDto == null) return null;

            var sellerProductReviewsList = await _unitOfWork.sellerProductReviewRepository.GetAllSellerProductReviewsBySellerProductIdAsync(SellerProductId);

            if (!sellerProductReviewsList.Any())
                return null;

            var sellerProductReviewsDtosList = _genericMapper.MapCollection<SellerProductReview,
                SellerProductReviewDto>(sellerProductReviewsList);

            return sellerProductReviewsDtosList;
        }

        public async Task<double> GetAverageOfStarsBySellerProductIdAsync(long sellerProductId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(sellerProductId, nameof(sellerProductId));

            var sellerProductDto = await _sellerProductService.FindByIdAsync(sellerProductId);
            if (sellerProductDto == null) return -1;

            var avg = await _unitOfWork.sellerProductReviewRepository.GetAveragedOfStarsOfSellerProductBySellerProductIdAsync(sellerProductId);

            return avg;
        }
   
        public async Task<IEnumerable<SellerProductReviewDto>> GetPagedSellerProductReviewsBySellerProductIdAsync(int pageNumber, int pageSize, long SellerProductId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(SellerProductId, nameof(SellerProductId));

            var sellerProductDto = await _sellerProductService.FindByIdAsync(SellerProductId);
            if (sellerProductDto == null) return null;

            var sellerProductReviewsList = await _unitOfWork.sellerProductReviewRepository.GetPagedSellerProductReviewsWithUserInfoBySellerProductIdAsync(pageNumber,pageSize,SellerProductId);

            if (!sellerProductReviewsList.Any())
                return null;

            var sellerProductReviewsDtosList = _genericMapper.MapCollection<SellerProductReview,
                SellerProductReviewDto>(sellerProductReviewsList);

            return sellerProductReviewsDtosList;
        }

        public async Task<bool> UpdateByIdAndUserIdAsync(long Id, string UserId, SellerProductReviewDto sellerProductReivewDto)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(Id, nameof(Id));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(UserId, nameof(UserId));
            ParamaterException.CheckIfObjectIfNotNull(sellerProductReivewDto, nameof(sellerProductReivewDto));

            var sellerProductReview = await _unitOfWork.sellerProductReviewRepository.GetSellerProductReviewByIdAndUserIdAsync(Id, UserId);
            if (sellerProductReview == null) return false;

            var sellerProductDto = await _sellerProductService.FindByIdAsync(sellerProductReivewDto.SellerProductId);
            if (sellerProductDto == null) return false;

            _genericMapper.MapSingle(sellerProductReivewDto, sellerProductReview);

            await _unitOfWork.sellerProductReviewRepository.UpdateAsync(Id, sellerProductReview);

            var IsSellerProductReviewUpdate = await _completeAsync();

            return IsSellerProductReviewUpdate;
        }

       
    }
}
