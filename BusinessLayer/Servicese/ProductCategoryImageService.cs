using BusinessLayer.Contracks;

using BusinessLayer.Dtos;
using BusinessLayer.Exceptions;
using BusinessLayer.Mapper.Contracks;
using DataAccessLayer.Entities;
using DataAccessLayer.UnitOfWork.Contracks;
using Microsoft.Extensions.Logging;
namespace BusinessLayer.Servicese
{
    public class ProductCategoryImageService : IProductCategoryImageService
    {
        private readonly ILogger<UserAddressDto> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericMapper _genericMapper;

        public ProductCategoryImageService(ILogger<UserAddressDto> logger, IUnitOfWork unitOfWork, IGenericMapper genericMapper)
        {
            this._logger = logger;
            this._unitOfWork = unitOfWork;
            this._genericMapper = genericMapper;
        }
        private async Task<bool> _IsCompletedAsync()
        {
            try
            {
                var NumberOfRowsAffected = await _unitOfWork.CompleteAsync();
                return NumberOfRowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<ProductCategoryImageDto> AddAsync(ProductCategoryImageDto dto)
        {
            ParamaterException.CheckIfObjectIfNotNull(dto, nameof(dto));

            var productCategoryImage = _genericMapper.MapSingle<ProductCategoryImageDto, ProductCategoryImage>(dto);
            if (productCategoryImage is null) return null;

            await _unitOfWork.productCategoryImageRepository.AddAsync(productCategoryImage);

            var IsAdded = await _IsCompletedAsync();
            if (!IsAdded) return null;

            _genericMapper.MapSingle(productCategoryImage, dto);
            return dto;
        }

        public async Task<IEnumerable<ProductCategoryImageDto>> AddRangeAsync(IEnumerable<ProductCategoryImageDto> dtos)
        {
            ParamaterException.CheckIfIEnumerableIsNotNullOrEmpty(dtos, nameof(dtos));

            List<ProductCategoryImageDto> newProductCategoryImageDtos = new();
            foreach (var dto in dtos)
            {
                ProductCategoryImageDto? newProductCategoryImageDto = await AddAsync(dto);
                if (newProductCategoryImageDto != null)
                    newProductCategoryImageDtos.Add(newProductCategoryImageDto);
            }

            if (!newProductCategoryImageDtos.Any()) return null;
            return newProductCategoryImageDtos;
        }

        public async Task<bool> DeleteByIdAsync(long Id)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(Id, nameof(Id));

            var productCategoryImage = await _unitOfWork.productCategoryImageRepository.GetByIdAsTrackingAsync(Id);
            if (productCategoryImage is null) return false;

            await _unitOfWork.productCategoryImageRepository.DeleteAsync(Id);
            var IsDeleted = await _IsCompletedAsync();

            return IsDeleted;

        }

        public async Task<bool> DeleteRangeByIdAsync(IEnumerable<long> Ids)
        {
            ParamaterException.CheckIfIEnumerableIsNotNullOrEmpty(Ids, nameof(Ids));

            foreach (var Id in Ids)
            {
                var productCategoryImage = await _unitOfWork.productCategoryImageRepository.GetByIdAsTrackingAsync(Id);
                if (productCategoryImage is null) return false;

            }

            await _unitOfWork.productCategoryImageRepository.DeleteRangeAsync(Ids);
            var IsDeleted = await _IsCompletedAsync();

            return IsDeleted;
        }

        public async Task<IEnumerable<ProductCategoryImageDto>> FindAllProductCategoryImagesByProductCategoryIdAsync(long productCategoryId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(productCategoryId, nameof(productCategoryId));

            var productCategoryImages = await _unitOfWork.
                productCategoryImageRepository.
                GetAllProductCategoryImagesByProductCategoryIdAsync(productCategoryId);

            if (productCategoryImages is null || !productCategoryImages.Any())
                return null;

            var productCategoryImagesDtos = _genericMapper.
                MapCollection<ProductCategoryImage, ProductCategoryImageDto>(productCategoryImages);
            return productCategoryImagesDtos;
        }

        public async Task<ProductCategoryImageDto> FindByIdAsync(long id)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(id, nameof(id));

            var productCategoryImage = await _unitOfWork.productCategoryImageRepository.GetByIdAsTrackingAsync(id);
            if (productCategoryImage is null) return null;

            var productCategoryImageDto = _genericMapper.MapSingle<ProductCategoryImage, ProductCategoryImageDto>(productCategoryImage);
            return productCategoryImageDto;

        }

        public async Task<IEnumerable<ProductCategoryImageDto>> GetAllAsync()
        {
            var productCategoryImages = await _unitOfWork.
                productCategoryImageRepository.GetAllAsNoTrackingAsync();

            var productCategoryImagesDtos = _genericMapper.
                MapCollection<ProductCategoryImage, ProductCategoryImageDto>(productCategoryImages);

            return productCategoryImagesDtos;
        }

        public async Task<long> GetCountOfAsync()
        {
            var count = await _unitOfWork.productCategoryImageRepository.GetCountAsync();
            return count;
        }

        public async Task<IEnumerable<ProductCategoryImageDto>> GetPagedDataAsync(int pageNumber, int pageSize)
        {
            ParamaterException.CheckIfIntIsBiggerThanZero(pageNumber, nameof(pageNumber));
            ParamaterException.CheckIfIntIsBiggerThanZero(pageSize, nameof(pageSize));

            var productCategoryImages = await _unitOfWork.
                productCategoryImageRepository.GetPagedDataAsNoTractingAsync(pageNumber, pageSize);

            var productCategoryImagesDtos = _genericMapper.
                MapCollection<ProductCategoryImage, ProductCategoryImageDto>(productCategoryImages);

            return productCategoryImagesDtos;
        }

        public async Task<bool> UpdateByIdAsync(long Id, ProductCategoryImageDto dto)
        {

            ParamaterException.CheckIfLongIsBiggerThanZero(Id, nameof(Id));
            ParamaterException.CheckIfObjectIfNotNull(dto, nameof(dto));

            var productCategoryImage = await _unitOfWork.
                productCategoryImageRepository.GetByIdAsTrackingAsync(Id);
            if (productCategoryImage is null) return false;


            _genericMapper.MapSingle(dto, productCategoryImage);

            await _unitOfWork.productCategoryImageRepository.UpdateAsync(Id, productCategoryImage);

            var IsUpdated = await _IsCompletedAsync();
            return IsUpdated;

        }

        public async Task<bool> DeleteAllProductCategoryImagesByProductCategoryIdAsync(long productCategoryId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(productCategoryId, nameof(productCategoryId));

            await _unitOfWork.productCategoryImageRepository.DeleteAllProductCategoryImagesByProductCategoryIdAsync(productCategoryId);

            var IsDeleted = await _IsCompletedAsync();

            return IsDeleted;
        }
    }
}
