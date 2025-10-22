using BusinessLayer.Contracks;
using BusinessLayer.Dtos;
using BusinessLayer.Exceptions;
using BusinessLayer.Mapper.Contracks;
using DataAccessLayer.Entities;
using DataAccessLayer.UnitOfWork.Contracks;
using Microsoft.Extensions.Logging;

namespace BusinessLayer.Servicese
{
    public class ProductService : IProductService
    {
        private readonly ILogger<ProductService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericMapper _genericMapper;
        private readonly IProductImageService _productImageService;
        private readonly IUserService _userService;
        private readonly IProductSubCategoryService _productSubCategoryService;
        private readonly IBrandService _brandService;
        private readonly IImageService _imageService;

        public ProductService(ILogger<ProductService> logger, IUnitOfWork unitOfWork,
            IGenericMapper genericMapper, IProductImageService productImageService,
            IUserService userService, IProductSubCategoryService productSubCategoryService,
            IBrandService brandService, IImageService imageService)
        {
            this._logger = logger;
            this._unitOfWork = unitOfWork;
            this._genericMapper = genericMapper;
            this._productImageService = productImageService;
            this._userService = userService;
            this._productSubCategoryService = productSubCategoryService;
            this._brandService = brandService;
            _imageService = imageService;
        }

        private async Task<bool> _CompleteAsync()
        {
            var NumberOfRowsAfected = await _unitOfWork.CompleteAsync();
            return NumberOfRowsAfected > 0;
        }

        public async Task<ProductDto> AddAsync(CreateProductDto createProductDto, string UserId)
        {
            ParamaterException.CheckIfObjectIfNotNull(createProductDto, nameof(createProductDto));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(UserId, nameof(UserId));


            try
            {
                var userDto = await _userService.FindByIdAsync(UserId);
                if (userDto == null) return null;

                var productSubCategoryDto = await _productSubCategoryService.FindByIdAsync(createProductDto.ProductSubCategoryId);
                if (productSubCategoryDto == null) return null;

                var brandDto = await _brandService.FindByIdAsync(createProductDto.BrandId);
                if (brandDto is null) return null;


                //check if not images to add
                if (createProductDto.Images is null || !createProductDto.Images.Any()) return null;

                var NewProduct = _genericMapper.MapSingle<CreateProductDto, Product>(createProductDto);
                if (NewProduct is null) return null;

                NewProduct.CreatedBy = UserId;


                await _unitOfWork.BeginTransactionAsync();

                await _unitOfWork.productRepository.AddAsync(NewProduct);

                var IsProductAdded = await _CompleteAsync();

                if (!IsProductAdded)
                {
                    _logger.LogError("Failed to add product.");
                    throw new Exception("Failed to add product.");
                }

                //map from createProductDto to NewProduct
                _genericMapper.MapSingle(NewProduct, createProductDto);

                //upload images
                var uploadedImagesDtos = await _imageService.UploadImagesAsync(createProductDto.Images);
                if (!uploadedImagesDtos.Any())
                {
                    _logger.LogError("No images were uploaded during product category creation.");
                    throw new Exception("No images were uploaded during product category creation.");
                }



                var NewProductImageList = new List<ProductImageDto>();
                foreach (var image in uploadedImagesDtos)
                {
                    ProductImageDto NewProductImageDto = new()
                    {
                        ProductId = NewProduct.Id,
                        PublicId = image.PublicId,
                        ImageUrl = image.Url,
                    };

                    NewProductImageList.Add(NewProductImageDto);
                }

                //save images info to db
                await _productImageService.AddRangeAsync(NewProductImageList);


                //map to dto
                var productDto = _genericMapper.MapSingle<Product, ProductDto>(NewProduct);
                productDto.Images = uploadedImagesDtos;


                await _unitOfWork.CommitTransactionAsync();

                return productDto;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<IEnumerable<ProductDto>> AddRangeAsync(IEnumerable<CreateProductDto> dtos, string UserId)
        {
            ParamaterException.CheckIfIEnumerableIsNotNullOrEmpty(dtos, nameof(dtos));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(UserId, nameof(UserId));

            var NewProductDtosList = new List<ProductDto>();
            foreach (var d in dtos)
            {
                var NewProductDto = await AddAsync(d, UserId);
                if (NewProductDto != null) NewProductDtosList.Add(NewProductDto);
            }

            if (!NewProductDtosList.Any()) return null;
            return NewProductDtosList;
        }

        public async Task<bool> DeleteByIdAsync(long Id)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(Id, nameof(Id));

            var IsProductExist = await _unitOfWork.productRepository.IsExistByIdAsync(Id);
            if (!IsProductExist) return false;

            await _unitOfWork.productRepository.DeleteAsync(Id);

            var IsDeleted = await _CompleteAsync();

            return IsDeleted;
        }

        public async Task<bool> DeleteRangeByIdAsync(IEnumerable<long> Ids)
        {
            ParamaterException.CheckIfIEnumerableIsNotNullOrEmpty(Ids, nameof(Ids));

            foreach (var id in Ids)
            {
                var IsExist = await _unitOfWork.productCategoryRepository.IsExistByIdAsync(id);

                if (!IsExist) return false;
            }

            await _unitOfWork.productRepository.DeleteRangeAsync(Ids);

            var IsDeleted = await _CompleteAsync();

            return IsDeleted;
        }

        public async Task<ProductDto> FindByIdAsync(long id)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(id, nameof(id));
            var product = await _unitOfWork.productRepository.GetByIdAsTrackingAsync(id);

            if (product is null) return null;

            var productDto = _genericMapper.MapSingle<Product, ProductDto>(product);

            if (productDto is null) return null;

            var productImagesDtoList = await _productImageService.FindAllProductImagesByProductIdAsync(productDto.Id);

            if (productImagesDtoList is not null || productImagesDtoList.Any())
            {
                productDto.Images = productImagesDtoList.Select(e => new ImageDto() { PublicId = e.PublicId, Url = e.ImageUrl }).ToList();
            }
            return productDto;
        }

        public async Task<ProductDto> FindByNameArAsync(string nameAr)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(nameAr, nameof(nameAr));
            var product = await _unitOfWork.productRepository.GetByNameArAsync(nameAr);

            if (product is null) return null;

            var productDto = _genericMapper.MapSingle<Product, ProductDto>(product);

            if (productDto is null) return null;

            var productImagesDtoList = await _productImageService.FindAllProductImagesByProductIdAsync(productDto.Id);

            if (productImagesDtoList is not null || productImagesDtoList.Any())
            {
                productDto.Images = productImagesDtoList.Select(e => new ImageDto() { PublicId = e.PublicId, Url = e.ImageUrl }).ToList();
            }
            return productDto;
        }

        public async Task<ProductDto> FindByNameEnAsync(string nameEn)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(nameEn, nameof(nameEn));
            var product = await _unitOfWork.productRepository.GetByNameEnAsync(nameEn);

            if (product is null) return null;

            var productDto = _genericMapper.MapSingle<Product, ProductDto>(product);

            if (productDto is null) return null;

            var productImagesDtoList = await _productImageService.FindAllProductImagesByProductIdAsync(productDto.Id);

            if (productImagesDtoList is not null || productImagesDtoList.Any())
            {
                productDto.Images = productImagesDtoList.Select(e => new ImageDto() { PublicId = e.PublicId, Url = e.ImageUrl }).ToList();
            }
            return productDto;
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var products = await _unitOfWork.
               productRepository.GetAllAsNoTrackingAsync();


            var productsDtosList = _genericMapper.
                MapCollection<Product, ProductDto>(products);

            if (productsDtosList is null)
            {
                return null;
            }

            foreach (var productDto in productsDtosList)
            {
                var productImagesDtosList = await _productImageService.FindAllProductImagesByProductIdAsync(productDto.Id);

                if (productImagesDtosList is not null || productImagesDtosList.Any())
                {
                    productDto.Images = productImagesDtosList.Select(e => new ImageDto() { PublicId = e.PublicId, Url = e.ImageUrl }).ToList();
                }
            }

            return productsDtosList;
        }

        public async Task<long> GetCountAsync()
        {
            var count = await _unitOfWork.productRepository.GetCountAsync();
            return count;
        }

        public async Task<IEnumerable<ProductDto>> GetPagedDataAsync(int pageNumber, int pageSize)
        {
            ParamaterException.CheckIfIntIsBiggerThanZero(pageNumber, nameof(pageNumber));
            ParamaterException.CheckIfIntIsBiggerThanZero(pageSize, nameof(pageSize));

            var products = await _unitOfWork.
                productRepository.GetPagedDataAsNoTractingAsync(pageNumber, pageSize);


            var productsDtosList = _genericMapper.
                MapCollection<Product, ProductDto>(products);

            if (productsDtosList is null)
            {
                return null;
            }

            foreach (var productDto in productsDtosList)
            {
                var productImagesDtosList = await _productImageService.FindAllProductImagesByProductIdAsync(productDto.Id);

                if (productImagesDtosList is not null || productImagesDtosList.Any())
                {
                    productDto.Images = productImagesDtosList.Select(e => new ImageDto() { PublicId = e.PublicId, Url = e.ImageUrl }).ToList();
                }
            }

            return productsDtosList;
        }

        public async Task<bool> UpdateByIdAsync(long Id, CreateProductDto dto)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(Id, nameof(Id));
            ParamaterException.CheckIfObjectIfNotNull(dto, nameof(dto));


            try
            {

                var product = await _unitOfWork.productRepository.GetByIdAsNoTrackingAsync(Id);

                if (product == null) return false;


                //get all product category images
                var productImagesDtos = await _productImageService.FindAllProductImagesByProductIdAsync(Id);
                if (!productImagesDtos.Any())
                    return false;

                var ImageDtos = _genericMapper.MapCollection<ProductImageDto, ImageDto>(productImagesDtos);

                //remove old images from image server
                var isImagesDeletedFromServer = await _imageService.DeleteImagesAsync(ImageDtos);
                if (!isImagesDeletedFromServer)
                {
                    throw new Exception("Failed to delete old images from image server during product category update.");
                }


                await _unitOfWork.BeginTransactionAsync();

                //delete old images from db
                var IsProductImagesDeleted = await _productImageService.DeleteAllProductImagesByProductIdAsync(Id);

                //update product category info
                _genericMapper.MapSingle(dto, product);


                await _unitOfWork.productRepository.UpdateAsync(Id, product);

                var IsProductUpdated = await _CompleteAsync();

                if (!IsProductUpdated)
                {
                    throw new Exception("Failed to update product info.");
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
                    _logger.LogError("No images were uploaded during product category update.");
                    throw new Exception("No images were uploaded during product category update.");
                }

                //Add new product Images images to db
                var NewProductImageList = new List<ProductImageDto>();
                foreach (var image in uploadedImagesDtos)
                {
                    ProductImageDto NewProductImageDto = new()
                    {
                        ProductId = product.Id,
                        ImageUrl = image.Url,
                        PublicId = image.PublicId,
                    };

                    NewProductImageList.Add(NewProductImageDto);
                }


                var productImagesDtosList = await _productImageService.AddRangeAsync(NewProductImageList);

                if (productImagesDtosList is null)
                {
                    _logger.LogError("Failed to add new product images during update.");
                    throw new Exception("Failed to add new product images during update.");
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

        public async Task<IEnumerable<ProductSearchResultDto>> SearchByNameEnAsync(string NameEn, int pageSize)
        {
            var productsList = await
                _unitOfWork.productRepository.SearchByNameEnAsync(NameEn, pageSize);

            var productSearchResultsDtosList = productsList.Select(e =>
            {
                return new ProductSearchResultDto
                {
                    Id = e.Id,
                    Name = e.NameEn
                };
            });

            return productSearchResultsDtosList;


        }

        public async Task<IEnumerable<ProductSearchResultDto>> SearchByNameArAsync(string NameAr, int pageSize)
        {
            var productsList = await
                _unitOfWork.productRepository.SearchByNameArAsync(NameAr, pageSize);

            var productSearchResultsDtosList = productsList.Select(e =>
            {
                return new ProductSearchResultDto
                {
                    Id = e.Id,
                    Name = e.NameAr
                };
            });

            return productSearchResultsDtosList;
        }

        public async Task<IEnumerable<ProductDto>> GetAllOrderByBestSellerDescAsync()
        {
            var products = await _unitOfWork.
               productRepository.GetAllOrderByBestSellerDescAsync();


            var productsDtosList = _genericMapper.
                MapCollection<Product, ProductDto>(products);

            if (productsDtosList is null)
            {
                return null;
            }

            foreach (var productDto in productsDtosList)
            {
                var productImagesDtosList = await _productImageService.FindAllProductImagesByProductIdAsync(productDto.Id);

                if (productImagesDtosList is not null || productImagesDtosList.Any())
                {
                    productDto.Images = productImagesDtosList.Select(e => new ImageDto() { PublicId = e.PublicId, Url = e.ImageUrl }).ToList();
                }
            }

            return productsDtosList;
        }

        public async Task<IEnumerable<ProductDto>> GetPagedOrderByBestSellerDescAsync(int pageNumber, int pageSize)
        {
            ParamaterException.CheckIfIntIsBiggerThanZero(pageNumber, nameof(pageNumber));
            ParamaterException.CheckIfIntIsBiggerThanZero(pageSize, nameof(pageSize));

            var products = await _unitOfWork.
               productRepository.GetPagedOrderByBestSellerDescAsync(pageNumber, pageSize);


            var productsDtosList = _genericMapper.
                MapCollection<Product, ProductDto>(products);

            if (productsDtosList is null)
            {
                return null;
            }

            foreach (var productDto in productsDtosList)
            {
                var productImagesDtosList = await _productImageService.FindAllProductImagesByProductIdAsync(productDto.Id);

                if (productImagesDtosList is not null || productImagesDtosList.Any())
                {
                    productDto.Images = productImagesDtosList.Select(e => new ImageDto() { PublicId = e.PublicId, Url = e.ImageUrl }).ToList();
                }
            }

            return productsDtosList;
        }
    }
}
