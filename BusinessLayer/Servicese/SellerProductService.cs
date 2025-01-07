using BusinessLayer.Contracks;
using BusinessLayer.Dtos;
using BusinessLayer.Exceptions;
using BusinessLayer.Mapper.Contracks;
using DataAccessLayer.Entities;
using DataAccessLayer.UnitOfWork.Contracks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Servicese
{
    public class SellerProductService : ISellerProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly IProductService _productService;
        private readonly IGenericMapper _genericMapper;

        public SellerProductService(IUnitOfWork unitOfWork, IUserService userService, IProductService productService,
            IGenericMapper genericMapper)
        {
            this._unitOfWork = unitOfWork;
            this._userService = userService;
            this._productService = productService;
            this._genericMapper = genericMapper;
        }

        private async Task<bool> _completeAsync()
        {
            var numbersOfRowsAffeted = await _unitOfWork.CompleteAsync();
            return numbersOfRowsAffeted > 0;
        }

        public async Task<SellerProductDto> AddAsync(SellerProductDto sellerProductDto, string UserId)
        {
            ParamaterException.CheckIfObjectIfNotNull(sellerProductDto, nameof(sellerProductDto));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(UserId, nameof(UserId));

            var productDto = await _productService.FindByIdAsync(sellerProductDto.ProductId);
            if (productDto == null) return null;

            var userDto = await _userService.FindByIdAsync(UserId);
            if (userDto is null) return null;

            var sellerProduct = _genericMapper.MapSingle<SellerProductDto, SellerProduct>(sellerProductDto);
            if (sellerProduct is null) return null;

            sellerProduct.SellerId = UserId;

            await _unitOfWork.sellerProductRepository.AddAsync(sellerProduct);

            var IsSellerProductAdded = await _completeAsync();

            _genericMapper.MapSingle(sellerProduct, sellerProductDto);

            return sellerProductDto;
        }

        public async Task<IEnumerable<SellerProductDto>> AddRangeAsync(IEnumerable<SellerProductDto> sellerProductDtosList, string UserId)
        {
            ParamaterException.CheckIfIEnumerableIsNotNullOrEmpty(sellerProductDtosList, nameof(sellerProductDtosList));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(UserId, nameof(UserId));

            var userDto = await _userService.FindByIdAsync(UserId);
            if (userDto is null) return null;

            var NewsellerProductDtosList = new List<SellerProductDto>();
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                foreach (var sellerProductDto in sellerProductDtosList)
                {
                    var productDto = await _productService.FindByIdAsync(sellerProductDto.ProductId);
                    if (productDto == null) throw new Exception($"Product not found Id = {sellerProductDto.ProductId}");

                    var sellerProduct = _genericMapper.MapSingle<SellerProductDto, SellerProduct>(sellerProductDto);
                    if (sellerProduct is null) throw new Exception("sellerProduct is null. cannot map from SellerProductDto to SellerProduct");

                    sellerProduct.SellerId = UserId;

                    await _unitOfWork.sellerProductRepository.AddAsync(sellerProduct);

                    var IsSellerProductAdded = await _completeAsync();

                    _genericMapper.MapSingle(sellerProduct, sellerProductDto);

                    NewsellerProductDtosList.Add(sellerProductDto);

                }

                await _unitOfWork.CommitTransactionAsync();

                return NewsellerProductDtosList;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }

        }

        public async Task<bool> DeleteByIdAsync(long Id)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(Id, nameof(Id));

            var IsSellerProductExist = await _unitOfWork.sellerProductRepository.IsExistByIdAsync(Id);
            if (!IsSellerProductExist) return false;

            await _unitOfWork.sellerProductRepository.DeleteAsync(Id);

            var IsSellerProductDeleted = await _completeAsync();

            return IsSellerProductDeleted;
        }

        public async Task<bool> DeleteByIdAndUserIdAsync(long Id, string UserId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(Id, nameof(Id));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(UserId, nameof(UserId));

            var sellerProduct = await _unitOfWork.sellerProductRepository.GetSellerProductByIdAndSellerIdAsync(Id,UserId);
            if (sellerProduct is null) return false;

            await _unitOfWork.sellerProductRepository.DeleteAsync(Id);

            var IsSellerProductDeleted = await _completeAsync();

            return IsSellerProductDeleted;
        }
        
        public async Task<bool> DeleteRangeByIdAsync(IEnumerable<long> Ids)
        {
            ParamaterException.CheckIfIEnumerableIsNotNullOrEmpty(Ids, nameof(Ids));

            foreach (var id in Ids)
            {
                var IsSellerProductExist = await _unitOfWork.sellerProductRepository.IsExistByIdAsync(id);
                if (!IsSellerProductExist) return false;
            }


            await _unitOfWork.sellerProductRepository.DeleteRangeAsync(Ids);

            var IsSellerProductsDeleted = await _completeAsync();

            return IsSellerProductsDeleted;
        }

        public async Task<SellerProductDto> FindByIdAsync(long id)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(id, nameof(id));
           
            var sellerProduct = await _unitOfWork.sellerProductRepository.GetByIdAsTrackingAsync(id);
            if (sellerProduct is null) return null;

            var sellerProductDto = _genericMapper.MapSingle<SellerProduct,SellerProductDto>(sellerProduct);
            return sellerProductDto;

        }

        public async Task<IEnumerable<SellerProductDto>> GetAllByProductIdOrderByPriceAscAsync(long productId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(productId, nameof(productId));
            var productDto = await _productService.FindByIdAsync(productId);
            if (productDto == null) return null;

            var sellerProductsList = await _unitOfWork.sellerProductRepository.GetAllByProductIdOrderByPriceAscAsync(productId);

            if (!sellerProductsList.Any())
                return null;

            var sellerProductsDtosList = _genericMapper.MapCollection<SellerProduct,
                SellerProductDto>(sellerProductsList);

            return sellerProductsDtosList;
        }

        public async Task<IEnumerable<SellerProductDto>> GetAllSellerProductsBySellerIdAsync(string sellerId)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(sellerId, nameof(sellerId));
            var userDto = await _userService.FindByIdAsync(sellerId);
            if (userDto == null) return null;

            var sellerProductsList = await _unitOfWork.sellerProductRepository.GetAllSellerProductsBySellerIdAsync(sellerId);

            if (!sellerProductsList.Any())
                return null;

            var sellerProductsDtosList = _genericMapper.MapCollection<SellerProduct,
                SellerProductDto>(sellerProductsList);

            return sellerProductsDtosList;
        }

        public async Task<IEnumerable<SellerProductDto>> GetPagedByProductIdAsync(int pageNumber, int pageSize, long productId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(productId, nameof(productId));
            var productDto = await _productService.FindByIdAsync(productId);
            if (productDto == null) return null;

            var sellerProductsList = await _unitOfWork.sellerProductRepository.
                GetPagedDataAsNoTrackingByProductIdAsync(pageNumber,pageSize,productId);

            if (!sellerProductsList.Any())
                return null;

            var sellerProductsDtosList = _genericMapper.MapCollection<SellerProduct,
                SellerProductDto>(sellerProductsList);

            return sellerProductsDtosList;
        }

        public async Task<bool> UpdateByIdAndUserIdAsync(long Id,string sellerId, SellerProductDto sellerProductDto)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(Id, nameof(Id));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(sellerId, nameof(sellerId));
            ParamaterException.CheckIfObjectIfNotNull(sellerProductDto, nameof(sellerProductDto));

            var sellerProduct = await _unitOfWork.sellerProductRepository.GetSellerProductByIdAndSellerIdAsync(Id,sellerId);
            if(sellerProduct == null) return false;

            var productDto = await _productService.FindByIdAsync(sellerProductDto.ProductId);
            if (productDto == null) return false;

            _genericMapper.MapSingle(sellerProductDto,sellerProduct);

            await _unitOfWork.sellerProductRepository.UpdateAsync(Id, sellerProduct);

            var IsSellerProductUpdate = await _completeAsync();

            return IsSellerProductUpdate;
        }

        
    }
}
