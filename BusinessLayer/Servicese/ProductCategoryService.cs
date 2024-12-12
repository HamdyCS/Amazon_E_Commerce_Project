using BusinessLayer.Contracks;
using BusinessLayer.Dtos;
using BusinessLayer.Exceptions;
using BusinessLayer.Mapper.Contracks;
using DataAccessLayer.Entities;
using DataAccessLayer.UnitOfWork.Contracks;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Servicese
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly ILogger<ProductCategoryService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericMapper _genericMapper;
        private readonly IProductCategoryImageService _productCategoryImageService;

        public ProductCategoryService(ILogger<ProductCategoryService> logger, IUnitOfWork unitOfWork,
            IGenericMapper genericMapper, IProductCategoryImageService productCategoryImageService)
        {
            this._logger = logger;
            this._unitOfWork = unitOfWork;
            this._genericMapper = genericMapper;
            this._productCategoryImageService = productCategoryImageService;
        }

        private async Task<bool> _CompleteAsync()
        {
            var NumberOfRowsAfected = await _unitOfWork.CompleteAsync();
            return NumberOfRowsAfected > 0;
        }

        public async Task<ProductCategoryDto> AddAsync(ProductCategoryDto dto, string UserId)
        {
            ParamaterException.CheckIfObjectIfNotNull(dto, nameof(dto));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(UserId, nameof(UserId));

            var productCategory = _genericMapper.MapSingle<ProductCategoryDto, ProductCategory>(dto);
            if (productCategory is null) return null;

            

            productCategory.CreatedBy = UserId;

            try
            {

                await _unitOfWork.BeginTransactionAsync();

                await _unitOfWork.productCategoryRepository.AddAsync(productCategory);

                var IsProductCategoryAdded = await _CompleteAsync();

                if (!IsProductCategoryAdded)
                {
                    return null;
                }

                _genericMapper.MapSingle(productCategory, dto);

                if (string.IsNullOrEmpty(dto.DescriptionEn))
                    productCategory.DescriptionEn = null;


                if (string.IsNullOrEmpty(dto.DescriptionAr))
                    productCategory.DescriptionAr = null;

                if (dto.Images is null || !dto.Images.Any()) return dto;

                var NewProductCategoryImageList = new List<ProductCategoryImageDto>();
                foreach (var image in dto.Images)
                {
                    ProductCategoryImageDto NewProductCategoryImageDto = new()
                    {
                        ProductCategoryId = productCategory.Id,
                        Image = image
                    };

                    NewProductCategoryImageList.Add(NewProductCategoryImageDto);
                }

                await _productCategoryImageService.AddRangeAsync(NewProductCategoryImageList);

                await _unitOfWork.CommitTransactionAsync();

                return dto;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<IEnumerable<ProductCategoryDto>> AddRangeAsync(IEnumerable<ProductCategoryDto> dtos, string UserId)
        {
            ParamaterException.CheckIfIEnumerableIsNotNullOrEmpty(dtos, nameof(dtos));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(UserId, nameof(UserId));

            var NewProductCategoryDtoList = new List<ProductCategoryDto>();
            foreach (var d in dtos)
            {
                var NewProductCategoryDto = await AddAsync(d, UserId);
                if (NewProductCategoryDto != null) NewProductCategoryDtoList.Add(NewProductCategoryDto);
            }

            if (!NewProductCategoryDtoList.Any()) return null;
            return NewProductCategoryDtoList;
        }

        public async Task<bool> DeleteByIdAsync(long Id)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(Id, nameof(Id));

            var productCategory = await _unitOfWork.productCategoryRepository.GetByIdAsNoTrackingAsync(Id);
            if (productCategory == null) return false;

            await _unitOfWork.productCategoryRepository.DeleteAsync(Id);

            var IsDeleted = await _CompleteAsync();

            return IsDeleted;
        }

        public async Task<bool> DeleteRangeByIdAsync(IEnumerable<long> Ids)
        {
            ParamaterException.CheckIfIEnumerableIsNotNullOrEmpty(Ids, nameof(Ids));

            foreach (var id in Ids)
            {
                var IsExist = await _unitOfWork.productCategoryRepository.IsExistAsync(id);

                if (!IsExist) return false;
            }

            await _unitOfWork.productCategoryRepository.DeleteRangeAsync(Ids);

            var IsDeleted = await _CompleteAsync();

            return IsDeleted;
        }

        public async Task<ProductCategoryDto> FindByIdAsync(long id)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(id, nameof(id));
            var productCategory = await _unitOfWork.productCategoryRepository.GetByIdAsTrackingAsync(id);

            if (productCategory is null) return null;

            var productCategoryDto = _genericMapper.MapSingle<ProductCategory, ProductCategoryDto>(productCategory);

            var productCategoryImagesDtos = await _productCategoryImageService.FindAllProductCategoryImagesByProductCategoryIdAsync(productCategory.Id);

            if (productCategoryImagesDtos is not null || productCategoryImagesDtos.Any())
            {
                productCategoryDto.Images = productCategoryImagesDtos.Select(e => e.Image).ToList();
            }
            return productCategoryDto;
        }

        public async Task<ProductCategoryDto> FindByNameArAsync(string nameAr)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(nameAr, nameof(nameAr));
            var productCategory = await _unitOfWork.productCategoryRepository.GetByNameArAsync(nameAr);

            if (productCategory is null) return null;

            var productCategoryDto = _genericMapper.MapSingle<ProductCategory, ProductCategoryDto>(productCategory);

            var productCategoryImagesDtos = await _productCategoryImageService.FindAllProductCategoryImagesByProductCategoryIdAsync(productCategory.Id);

            if (productCategoryImagesDtos is not null || productCategoryImagesDtos.Any())
            {
                productCategoryDto.Images = productCategoryImagesDtos.Select(e => e.Image).ToList();
            }

            return productCategoryDto;
        }

        public async Task<ProductCategoryDto> FindByNameEnAsync(string nameEn)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(nameEn, nameof(nameEn));
            var productCategory = await _unitOfWork.productCategoryRepository.GetByNameEnAsync(nameEn);

            if (productCategory is null) return null;

            var productCategoryDto = _genericMapper.MapSingle<ProductCategory, ProductCategoryDto>(productCategory);

            var productCategoryImagesDtos = await _productCategoryImageService.FindAllProductCategoryImagesByProductCategoryIdAsync(productCategory.Id);

            if (productCategoryImagesDtos is not null || productCategoryImagesDtos.Any())
            {
                productCategoryDto.Images = productCategoryImagesDtos.Select(e => e.Image).ToList();
            }

            return productCategoryDto;
        }

        public async Task<IEnumerable<ProductCategoryDto>> GetAllAsync()
        {
            var productCategories = await _unitOfWork.
               productCategoryRepository.GetAllNoTrackingAsync();

            var productCategoriesDtos = _genericMapper.
                MapCollection<ProductCategory, ProductCategoryDto>(productCategories);

            if (productCategoriesDtos is null)
            {
                return null;
            }

            foreach (var productCategoryDto in productCategoriesDtos)
            {
                var productCategoryImagesDtos = await _productCategoryImageService.FindAllProductCategoryImagesByProductCategoryIdAsync(productCategoryDto.Id);

                if (productCategoryImagesDtos is not null || productCategoryImagesDtos.Any())
                {
                    productCategoryDto.Images = productCategoryImagesDtos.Select(e => e.Image).ToList();
                }
            }

            return productCategoriesDtos;
        }

        public async Task<long> GetCountOfAsync()
        {
            var count = await _unitOfWork.productCategoryRepository.GetCountAsync();
            return count;
        }

        public async Task<IEnumerable<ProductCategoryDto>> GetPagedDataAsync(int pageNumber, int pageSize)
        {
            ParamaterException.CheckIfIntIsBiggerThanZero(pageNumber, nameof(pageNumber));
            ParamaterException.CheckIfIntIsBiggerThanZero(pageSize, nameof(pageSize));

            var productCategories = await _unitOfWork.
                productCategoryRepository.GetPagedDataAsNoTractingAsync(pageNumber, pageSize);

            var productCategoriesDtos = _genericMapper.
                MapCollection<ProductCategory, ProductCategoryDto>(productCategories);

            if (productCategoriesDtos is null)
            {
                return null;
            }

            foreach (var productCategoryDto in productCategoriesDtos)
            {
                var productCategoryImagesDtos = await _productCategoryImageService.FindAllProductCategoryImagesByProductCategoryIdAsync(productCategoryDto.Id);

                if (productCategoryImagesDtos is not null || productCategoryImagesDtos.Any())
                {
                    productCategoryDto.Images = productCategoryImagesDtos.Select(e => e.Image).ToList();
                }
            }

            return productCategoriesDtos;
        }

        public async Task<bool> UpdateByIdAsync(long Id, ProductCategoryDto dto)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(Id, nameof(Id));
            ParamaterException.CheckIfObjectIfNotNull(dto, nameof(dto));

            
            try
            {

                var productCategory = await _unitOfWork.productCategoryRepository.GetByIdAsTrackingAsync(Id);

                if (productCategory == null) return false;

               

                await _unitOfWork.BeginTransactionAsync();

                var IsProductCategoryImagesDeleted = await _productCategoryImageService.DeleteAllProductCategoryImagesByProductCategoryIdAsync(Id);

     

                _genericMapper.MapSingle(dto, productCategory);

                if (string.IsNullOrEmpty(dto.DescriptionEn))
                    productCategory.DescriptionEn = null;


                if (string.IsNullOrEmpty(dto.DescriptionAr))
                    productCategory.DescriptionAr = null;

                await _unitOfWork.productCategoryRepository.UpdateAsync(Id, productCategory);

                var IsProductCategoryUpdated = await _CompleteAsync();

                if (!IsProductCategoryUpdated) return false;


                if (dto.Images is null || !dto.Images.Any())
                {
                    await _unitOfWork.CommitTransactionAsync();
                    return true;
                }

                var NewProductCategoryImageList = new List<ProductCategoryImageDto>();
                foreach (var image in dto.Images)
                {
                    ProductCategoryImageDto NewProductCategoryImageDto = new()
                    {
                        ProductCategoryId = productCategory.Id,
                        Image = image
                    };

                    NewProductCategoryImageList.Add(NewProductCategoryImageDto);
                }

                await _productCategoryImageService.AddRangeAsync(NewProductCategoryImageList);

                await _unitOfWork.CommitTransactionAsync();
                return true;

            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }

        }
    }
}
