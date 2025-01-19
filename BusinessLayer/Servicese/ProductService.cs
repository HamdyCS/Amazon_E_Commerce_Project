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

        public ProductService(ILogger<ProductService> logger, IUnitOfWork unitOfWork,
            IGenericMapper genericMapper, IProductImageService productImageService,
            IUserService userService,IProductSubCategoryService productSubCategoryService,
            IBrandService brandService)
        {
            this._logger = logger;
            this._unitOfWork = unitOfWork;
            this._genericMapper = genericMapper;
            this._productImageService = productImageService;
            this._userService = userService;
            this._productSubCategoryService = productSubCategoryService;
            this._brandService = brandService;
        }

        private async Task<bool> _CompleteAsync()
        {
            var NumberOfRowsAfected = await _unitOfWork.CompleteAsync();
            return NumberOfRowsAfected > 0;
        }

        public async Task<ProductDto> AddAsync(ProductDto productDto, string UserId)
        {
            ParamaterException.CheckIfObjectIfNotNull(productDto, nameof(productDto));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(UserId, nameof(UserId));

            
            try
            {
                var userDto = await _userService.FindByIdAsync(UserId);
                if (userDto == null) return null;

                var productSubCategoryDto = await _productSubCategoryService.FindByIdAsync(productDto.ProductSubCategoryId);
                if (productSubCategoryDto == null) return null;

                var brandDto = await _brandService.FindByIdAsync(productDto.BrandId);
                if (brandDto is null) return null;


                  var NewProduct = _genericMapper.MapSingle<ProductDto, Product>(productDto);
                if (NewProduct is null) return null;

                NewProduct.CreatedBy = UserId;


                await _unitOfWork.BeginTransactionAsync();

                await _unitOfWork.productRepository.AddAsync(NewProduct);

                var IsProductAdded = await _CompleteAsync();

                if (!IsProductAdded)
                {
                    return null;
                }

                _genericMapper.MapSingle(NewProduct, productDto);

               

                if (productDto.Images is null || !productDto.Images.Any()) return productDto;

                var NewProductImageList = new List<ProductImageDto>();
                foreach (var image in productDto.Images)
                {
                    ProductImageDto NewProductImageDto = new()
                    {
                        ProductId = NewProduct.Id,
                        Image = image
                    };

                    NewProductImageList.Add(NewProductImageDto);
                }

                await _productImageService.AddRangeAsync(NewProductImageList);

                await _unitOfWork.CommitTransactionAsync();

                return productDto;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<IEnumerable<ProductDto>> AddRangeAsync(IEnumerable<ProductDto> dtos, string UserId)
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

            if (productDto is null) return null ;

            var productImagesDtoList = await _productImageService.FindAllProductImagesByProductIdAsync(productDto.Id);

            if (productImagesDtoList is not null || productImagesDtoList.Any())
            {
                productDto.Images = productImagesDtoList.Select(e => e.Image).ToList();
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
                productDto.Images = productImagesDtoList.Select(e => e.Image).ToList();
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
                productDto.Images = productImagesDtoList.Select(e => e.Image).ToList();
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
                    productDto.Images = productImagesDtosList.Select(e => e.Image).ToList();
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
                productRepository.GetPagedDataAsNoTractingAsync(pageNumber,pageSize);


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
                    productDto.Images = productImagesDtosList.Select(e => e.Image).ToList();
                }
            }

            return productsDtosList;
        }

        public async Task<bool> UpdateByIdAsync(long Id, ProductDto dto)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(Id, nameof(Id));
            ParamaterException.CheckIfObjectIfNotNull(dto, nameof(dto));


            try
            {

                var product = await _unitOfWork.productRepository.GetByIdAsNoTrackingAsync(Id);

                if (product == null) return false;



                await _unitOfWork.BeginTransactionAsync();

                var IsProductImagesDeleted = await _productImageService.DeleteAllProductImagesByProductIdAsync(Id);



                _genericMapper.MapSingle(dto, product);

                

                await _unitOfWork.productRepository.UpdateAsync(Id, product);

                var IsProductUpdated = await _CompleteAsync();

                if (!IsProductUpdated) return false;


                if (dto.Images is null || !dto.Images.Any())
                {
                    await _unitOfWork.CommitTransactionAsync();
                    return true;
                }

                var NewProductImageList = new List<ProductImageDto>();
                foreach (var image in dto.Images)
                {
                    ProductImageDto NewProductImageDto = new()
                    {
                        ProductId = product.Id,
                        Image = image
                    };

                    NewProductImageList.Add(NewProductImageDto);
                }

               var productImagesDtosList = await _productImageService.AddRangeAsync(NewProductImageList);

                if(productImagesDtosList is null) return false;

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
                    productDto.Images = productImagesDtosList.Select(e => e.Image).ToList();
                }
            }

            return productsDtosList;
        }

        public async Task<IEnumerable<ProductDto>> GetPagedOrderByBestSellerDescAsync(int pageNumber, int pageSize)
        {
            ParamaterException.CheckIfIntIsBiggerThanZero(pageNumber, nameof(pageNumber));
            ParamaterException.CheckIfIntIsBiggerThanZero(pageSize, nameof(pageSize));

            var products = await _unitOfWork.
               productRepository.GetPagedOrderByBestSellerDescAsync(pageNumber,pageSize);


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
                    productDto.Images = productImagesDtosList.Select(e => e.Image).ToList();
                }
            }

            return productsDtosList;
        }
    }
}
