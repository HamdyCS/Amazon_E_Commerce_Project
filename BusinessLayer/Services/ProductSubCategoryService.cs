using BusinessLayer.Contracks;
using BusinessLayer.Dtos;
using BusinessLayer.Exceptions;
using BusinessLayer.Mapper.Contracks;
using DataAccessLayer.Entities;
using DataAccessLayer.UnitOfWork.Contracks;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Servicese
{
    public class ProductSubCategoryService : IProductSubCategoryService
    {
        private readonly ILogger<ProductSubCategoryService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericMapper _genericMapper;
        private readonly IUserService _userService;
        private readonly IProductCategoryService _productCategoryService;

        public ProductSubCategoryService(ILogger<ProductSubCategoryService> logger,IUnitOfWork unitOfWork
            ,IGenericMapper genericMapper,IUserService userService,IProductCategoryService productCategoryService)
        {
            this._logger = logger;
            this._unitOfWork = unitOfWork;
            this._genericMapper = genericMapper;
            this._userService = userService;
            this._productCategoryService = productCategoryService;
        }

        private async Task<bool> _CompleteAsync()
        {
            var NumberOfRowsAfected = await _unitOfWork.CompleteAsync();
            return NumberOfRowsAfected > 0;
        }

        public async Task<ProductSubCategoryDto> AddAsync(ProductSubCategoryDto productSubCategoryDto, string UserId)
        {
            ParamaterException.CheckIfObjectIfNotNull(productSubCategoryDto, nameof(productSubCategoryDto));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(UserId, nameof(UserId));

            var userDto = await _userService.FindByIdAsync(UserId);
            if (userDto == null) return null;

            var productCategoryDto = await _productCategoryService.FindByIdAsync(productSubCategoryDto.ProductCategoryId);
            if (productCategoryDto == null) return null;

            var NewProductSubCategory = _genericMapper.MapSingle<ProductSubCategoryDto,ProductSubCategory>(productSubCategoryDto);
            if (NewProductSubCategory == null) return null;

            if (string.IsNullOrEmpty(productSubCategoryDto.DescriptionEn)) NewProductSubCategory.DescriptionEn = null;
            if (string.IsNullOrEmpty(productSubCategoryDto.DescriptionAr)) NewProductSubCategory.DescriptionAr = null;

            NewProductSubCategory.CreatedBy = UserId;

            await _unitOfWork.productSubCategoryRepository.AddAsync(NewProductSubCategory);

            var IsProductSubCategoryAdded = await _CompleteAsync();

            if (!IsProductSubCategoryAdded) return null;

            _genericMapper.MapSingle(NewProductSubCategory, productSubCategoryDto);

            return productSubCategoryDto;
        }

        public async Task<IEnumerable<ProductSubCategoryDto>> AddRangeAsync(IEnumerable<ProductSubCategoryDto> productSubCategoriesDtos, string UserId)
        {
            var userDto = await _userService.FindByIdAsync(UserId);
            if (userDto == null) return null;

            ParamaterException.CheckIfIEnumerableIsNotNullOrEmpty(productSubCategoriesDtos, nameof(productSubCategoriesDtos));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(UserId, nameof(UserId));

            var NewProductSubCategoriesDtosList = new List<ProductSubCategoryDto>();

            foreach (var productCategory in productSubCategoriesDtos)
            {
                var productCategoryDto = await _productCategoryService.FindByIdAsync(productCategory.ProductCategoryId);
                if (productCategoryDto == null) return null;
            }

            foreach (var d in productSubCategoriesDtos)
            {
                var NewProductSubCategoryDto = await AddAsync(d, UserId);
                if (NewProductSubCategoryDto != null) NewProductSubCategoriesDtosList.Add(NewProductSubCategoryDto);
            }

            if (!NewProductSubCategoriesDtosList.Any()) return null;
            return NewProductSubCategoriesDtosList;
        }

        public async Task<bool> DeleteByIdAsync(long Id)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(Id, nameof(Id));

            ProductSubCategory? productSubCategory = await _unitOfWork.productSubCategoryRepository.GetByIdAsNoTrackingAsync(Id);
            if (productSubCategory == null) return false;

            await _unitOfWork.productSubCategoryRepository.DeleteAsync(Id);

            var IsDeleted = await _CompleteAsync();

            return IsDeleted;
        }

        public async Task<bool> DeleteRangeByIdAsync(IEnumerable<long> Ids)
        {
            ParamaterException.CheckIfIEnumerableIsNotNullOrEmpty(Ids, nameof(Ids));

            foreach (var id in Ids)
            {
                var IsExist = await _unitOfWork.productSubCategoryRepository.IsExistByIdAsync(id);

                if (!IsExist) return false;
            }

            await _unitOfWork.productSubCategoryRepository.DeleteRangeAsync(Ids);

            var IsDeleted = await _CompleteAsync();

            return IsDeleted;
        }

        public async Task<ProductSubCategoryDto> FindByIdAsync(long id)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(id, nameof(id));
            var productSubCategory = await _unitOfWork.productSubCategoryRepository.GetByIdAsTrackingAsync(id);

            if (productSubCategory is null) return null;

            var productCategoryDto = _genericMapper.MapSingle<ProductSubCategory, ProductSubCategoryDto>(productSubCategory);

            
            return productCategoryDto;
        }

        public async Task<ProductSubCategoryDto> FindByNameArAsync(string nameAr)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(nameAr, nameof(nameAr));
            var productSubCategory = await _unitOfWork.productSubCategoryRepository.GetByNameArAsync(nameAr);

            if (productSubCategory is null) return null;

            var productCategoryDto = _genericMapper.MapSingle<ProductSubCategory, ProductSubCategoryDto>(productSubCategory);

            return productCategoryDto;
        }

        public async Task<ProductSubCategoryDto> FindByNameEnAsync(string nameEn)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(nameEn, nameof(nameEn));
            var productSubCategory = await _unitOfWork.productSubCategoryRepository.GetByNameEnAsync(nameEn);

            if (productSubCategory is null) return null;

            var productCategoryDto = _genericMapper.MapSingle<ProductSubCategory, ProductSubCategoryDto>(productSubCategory);

            return productCategoryDto;
        }

        public async Task<IEnumerable<ProductSubCategoryDto>> GetAllAsync()
        {
            var productSubCategories = await _unitOfWork.
                productSubCategoryRepository.GetAllAsNoTrackingAsync();


            var productSubCategoriesDtos = _genericMapper.
                MapCollection<ProductSubCategory, ProductSubCategoryDto>(productSubCategories);

            return productSubCategoriesDtos;
        }

        public async Task<IEnumerable<ProductSubCategoryDto>> GetAllByProductCategoryIdAsync(long ProductCategoryId)
        {
           ParamaterException.CheckIfLongIsBiggerThanZero(ProductCategoryId, nameof(ProductCategoryId));

            IEnumerable<ProductSubCategory>? productSubCategories = await _unitOfWork.productSubCategoryRepository.GetAllByProductCategoryIdAsync(ProductCategoryId);

            var productSubCategoriesDtos = _genericMapper.MapCollection<ProductSubCategory, ProductSubCategoryDto>(productSubCategories);

            return productSubCategoriesDtos;
        }

        public async Task<long> GetCountAsync()
        {
            var count = await _unitOfWork.productSubCategoryRepository.GetCountAsync();
            return count;
        }

        public async Task<IEnumerable<ProductSubCategoryDto>> GetPagedDataAsync(int pageNumber, int pageSize)
        {
            ParamaterException.CheckIfIntIsBiggerThanZero(pageNumber, nameof(pageNumber));
            ParamaterException.CheckIfIntIsBiggerThanZero(pageSize, nameof(pageSize));

            var productsubCategoriesList = await _unitOfWork.
                productSubCategoryRepository.GetPagedDataAsNoTractingAsync(pageNumber, pageSize);

            var productSubCategoriesDtosList = _genericMapper.
                MapCollection<ProductSubCategory, ProductSubCategoryDto>(productsubCategoriesList);

            return productSubCategoriesDtosList;
        }

        public async Task<bool> UpdateByIdAsync(long Id, ProductSubCategoryDto productSubCategoryDto)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(Id, nameof(Id));
            ParamaterException.CheckIfObjectIfNotNull(productSubCategoryDto, nameof(productSubCategoryDto));

            var productSubCategory = await _unitOfWork.productSubCategoryRepository.GetByIdAsTrackingAsync(Id);

            if (productSubCategory == null) return false;

            _genericMapper.MapSingle(productSubCategoryDto, productSubCategory);

            if (string.IsNullOrEmpty(productSubCategoryDto.DescriptionEn)) productSubCategory.DescriptionEn = null;
            if (string.IsNullOrEmpty(productSubCategoryDto.DescriptionAr)) productSubCategory.DescriptionAr = null;

            var productCategoryDto = await _productCategoryService.FindByIdAsync(productSubCategoryDto.ProductCategoryId);
            if (productCategoryDto == null) return false;

            await _unitOfWork.productSubCategoryRepository.UpdateAsync(Id, productSubCategory);

            var IsProductSubCategoryUpdated = await _CompleteAsync();

            return IsProductSubCategoryUpdated;
        }
    }
}
