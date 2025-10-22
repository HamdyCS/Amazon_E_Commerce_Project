using BusinessLayer.Contracks;
using BusinessLayer.Dtos;
using BusinessLayer.Exceptions;
using BusinessLayer.Mapper.Contracks;
using DataAccessLayer.Entities;
using DataAccessLayer.UnitOfWork.Contracks;
using Microsoft.Extensions.Logging;

namespace BusinessLayer.Servicese
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly ILogger<ProductCategoryService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericMapper _genericMapper;
        private readonly IProductCategoryImageService _productCategoryImageService;
        private readonly IUserService _userService;
        private readonly IImageService _imageService;

        public ProductCategoryService(ILogger<ProductCategoryService> logger, IUnitOfWork unitOfWork,
            IGenericMapper genericMapper, IProductCategoryImageService productCategoryImageService,
            IUserService userService, IImageService imageService)
        {
            this._logger = logger;
            this._unitOfWork = unitOfWork;
            this._genericMapper = genericMapper;
            this._productCategoryImageService = productCategoryImageService;
            this._userService = userService;
            _imageService = imageService;
        }

        private async Task<bool> _CompleteAsync()
        {
            var NumberOfRowsAfected = await _unitOfWork.CompleteAsync();
            return NumberOfRowsAfected > 0;
        }

        public async Task<ProductCategoryDto> AddAsync(CreateProductCategoryDto createProductCategoryDto, string UserId)
        {
            ParamaterException.CheckIfObjectIfNotNull(createProductCategoryDto, nameof(createProductCategoryDto));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(UserId, nameof(UserId));

            try
            {
                var userDto = await _userService.FindByIdAsync(UserId);
                if (userDto == null) return null;

                var productCategory = _genericMapper.MapSingle<CreateProductCategoryDto, ProductCategory>(createProductCategoryDto);
                if (productCategory is null) return null;

                productCategory.CreatedBy = UserId;

                if (string.IsNullOrEmpty(createProductCategoryDto.DescriptionEn))
                    productCategory.DescriptionEn = null;


                if (string.IsNullOrEmpty(createProductCategoryDto.DescriptionAr))
                    productCategory.DescriptionAr = null;

                //check if not images to add
                if (createProductCategoryDto.Images is null || !createProductCategoryDto.Images.Any()) return null;

                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.productCategoryRepository.AddAsync(productCategory);

                var IsProductCategoryAdded = await _CompleteAsync();

                if (!IsProductCategoryAdded)
                {
                    throw new Exception("Failed to add product category.");
                }

                _genericMapper.MapSingle(productCategory, createProductCategoryDto);


                //upload images
                var uploadedImagesDtos = await _imageService.UploadImagesAsync(createProductCategoryDto.Images);
                if (!uploadedImagesDtos.Any())
                {
                    throw new Exception("No images were uploaded during product category creation.");
                }

                var NewProductCategoryImageList = new List<ProductCategoryImageDto>();
                foreach (var imageDto in uploadedImagesDtos)
                {
                    ProductCategoryImageDto NewProductCategoryImageDto = new()
                    {
                        ProductCategoryId = productCategory.Id,
                        ImageUrl = imageDto.Url,
                        PublicId = imageDto.PublicId

                    };

                    NewProductCategoryImageList.Add(NewProductCategoryImageDto);
                }

                //save images info to db
                await _productCategoryImageService.AddRangeAsync(NewProductCategoryImageList);

                //map to dto
                var productCategoryDto = _genericMapper.MapSingle<ProductCategory, ProductCategoryDto>(productCategory);
                productCategoryDto.Images = uploadedImagesDtos;

                await _unitOfWork.CommitTransactionAsync();

                return productCategoryDto;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<IEnumerable<ProductCategoryDto>> AddRangeAsync(IEnumerable<CreateProductCategoryDto> dtos, string UserId)
        {
            ParamaterException.CheckIfIEnumerableIsNotNullOrEmpty(dtos, nameof(dtos));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(UserId, nameof(UserId));

            var NewProductCategoriesDtosList = new List<ProductCategoryDto>();
            foreach (var d in dtos)
            {
                var NewProductCategoryDto = await AddAsync(d, UserId);
                if (NewProductCategoryDto != null) NewProductCategoriesDtosList.Add(NewProductCategoryDto);
            }

            if (!NewProductCategoriesDtosList.Any()) return null;
            return NewProductCategoriesDtosList;
        }

        public async Task<bool> DeleteByIdAsync(long Id)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(Id, nameof(Id));

            try
            {

                var productCategory = await _unitOfWork.productCategoryRepository.GetByIdAsNoTrackingAsync(Id);
                if (productCategory == null) return false;

                await _unitOfWork.productCategoryRepository.DeleteAsync(Id);

                var IsDeleted = await _CompleteAsync();

                return IsDeleted;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<bool> DeleteRangeByIdAsync(IEnumerable<long> Ids)
        {
            ParamaterException.CheckIfIEnumerableIsNotNullOrEmpty(Ids, nameof(Ids));

            foreach (var id in Ids)
            {
                var IsExist = await _unitOfWork.productCategoryRepository.IsExistByIdAsync(id);

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
                productCategoryDto.Images = productCategoryImagesDtos.Select(e => new ImageDto() { PublicId = e.PublicId, Url = e.ImageUrl }).ToList();
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
                productCategoryDto.Images = productCategoryImagesDtos.Select(e => new ImageDto() { PublicId = e.PublicId, Url = e.ImageUrl }).ToList();
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
                productCategoryDto.Images = productCategoryImagesDtos.Select(e => new ImageDto() { PublicId = e.PublicId, Url = e.ImageUrl }).ToList();
            }

            return productCategoryDto;
        }

        public async Task<IEnumerable<ProductCategoryDto>> GetAllAsync()
        {
            var productCategories = await _unitOfWork.
               productCategoryRepository.GetAllAsNoTrackingAsync();


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
                    productCategoryDto.Images = productCategoryImagesDtos.Select(e => new ImageDto() { PublicId = e.PublicId, Url = e.ImageUrl }).ToList();
                }
            }

            return productCategoriesDtos;
        }

        public async Task<long> GetCountAsync()
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
                    productCategoryDto.Images = productCategoryImagesDtos.Select(e => new ImageDto() { PublicId = e.PublicId, Url = e.ImageUrl }).ToList();
                }
            }

            return productCategoriesDtos;
        }

        public async Task<bool> UpdateByIdAsync(long Id, CreateProductCategoryDto dto)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(Id, nameof(Id));
            ParamaterException.CheckIfObjectIfNotNull(dto, nameof(dto));


            try
            {

                var productCategory = await _unitOfWork.productCategoryRepository.GetByIdAsTrackingAsync(Id);

                if (productCategory == null) return false;


                //get all product category images
                var productCategoryImagesDtos = await _productCategoryImageService.FindAllProductCategoryImagesByProductCategoryIdAsync(Id);
                if (!productCategoryImagesDtos.Any())
                    return false;

                var ImageDtos = _genericMapper.MapCollection<ProductCategoryImageDto, ImageDto>(productCategoryImagesDtos);

                //remove old images from image server
                var isImagesDeletedFromServer = await _imageService.DeleteImagesAsync(ImageDtos);
                if (!isImagesDeletedFromServer)
                {
                    throw new Exception("Failed to delete old images from image server during product category update.");
                }


                await _unitOfWork.BeginTransactionAsync();

                //delete old images from db
                var IsProductCategoryImagesDeleted = await _productCategoryImageService.DeleteAllProductCategoryImagesByProductCategoryIdAsync(Id);

                //update product category info
                _genericMapper.MapSingle(dto, productCategory);

                if (string.IsNullOrEmpty(dto.DescriptionEn))
                    productCategory.DescriptionEn = null;

                if (string.IsNullOrEmpty(dto.DescriptionAr))
                    productCategory.DescriptionAr = null;


                await _unitOfWork.productCategoryRepository.UpdateAsync(Id, productCategory);

                var IsProductCategoryUpdated = await _CompleteAsync();
                if (!IsProductCategoryUpdated)
                {
                    throw new Exception("Failed to update product category info.");
                }


                if (dto.Images is null || !dto.Images.Any())
                {
                    await _unitOfWork.CommitTransactionAsync();
                    return true;
                }

                //upload new images
                var uploadedImagesDtos = await _imageService.UploadImagesAsync(dto.Images);
                if (!uploadedImagesDtos.Any())
                {
                    throw new Exception(message: "No images were uploaded during product category update.");
                }


                //Add new product Category Images images to db

                var NewProductCategoryImageList = new List<ProductCategoryImageDto>();
                foreach (var imageDto in uploadedImagesDtos)
                {
                    ProductCategoryImageDto NewProductCategoryImageDto = new()
                    {
                        ProductCategoryId = productCategory.Id,
                        ImageUrl = imageDto.Url,
                        PublicId = imageDto.PublicId
                    };

                    NewProductCategoryImageList.Add(NewProductCategoryImageDto);
                }

                var productCategoryImagesDtosList = await _productCategoryImageService.AddRangeAsync(NewProductCategoryImageList);

                if (productCategoryImagesDtosList is null)
                {
                    _logger.LogError("Failed to add new product category images during update.");
                    throw new Exception("Failed to add new product category images during update.");
                }

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
