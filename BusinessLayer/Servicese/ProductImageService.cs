using BusinessLayer.Contracks;

using BusinessLayer.Dtos;
using BusinessLayer.Exceptions;
using BusinessLayer.Mapper.Contracks;
using DataAccessLayer.Entities;
using DataAccessLayer.UnitOfWork.Contracks;
using Microsoft.Extensions.Logging;
namespace BusinessLayer.Servicese
{
    public class ProductImageService : IProductImageService
    {
        private readonly ILogger<UserAddressDto> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericMapper _genericMapper;

        public ProductImageService(ILogger<UserAddressDto> logger, IUnitOfWork unitOfWork, IGenericMapper genericMapper)
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

        public async Task<ProductImageDto> AddAsync(ProductImageDto dto)
        {
            ParamaterException.CheckIfObjectIfNotNull(dto, nameof(dto));

            var productImage = _genericMapper.MapSingle<ProductImageDto, ProductImage>(dto);
            if (productImage is null) return null;

            await _unitOfWork.productImageRepository.AddAsync(productImage);

            var IsAdded = await _IsCompletedAsync();
            if (!IsAdded) return null;

            _genericMapper.MapSingle(productImage, dto);
            return dto;
        }

        public async Task<IEnumerable<ProductImageDto>> AddRangeAsync(IEnumerable<ProductImageDto> dtos)
        {
            ParamaterException.CheckIfIEnumerableIsNotNullOrEmpty(dtos, nameof(dtos));

            List<ProductImageDto> newProductImageDtoList = new();
            foreach (var dto in dtos)
            {
                var newProductImageDto = await AddAsync(dto);
                if (newProductImageDto != null)
                    newProductImageDtoList.Add(newProductImageDto);
            }

            if (!newProductImageDtoList.Any()) return null;

            return newProductImageDtoList;
        }

        public async Task<bool> DeleteByIdAsync(long Id)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(Id, nameof(Id));

            var productImage = await _unitOfWork.productImageRepository.GetByIdAsTrackingAsync(Id);
            if (productImage is null) return false;

            await _unitOfWork.productImageRepository.DeleteAsync(Id);
            var IsDeleted = await _IsCompletedAsync();

            return IsDeleted;

        }

        public async Task<bool> DeleteRangeByIdAsync(IEnumerable<long> Ids)
        {
            ParamaterException.CheckIfIEnumerableIsNotNullOrEmpty(Ids, nameof(Ids));

            foreach (var Id in Ids)
            {
                var productImage = await _unitOfWork.productImageRepository.IsExistByIdAsync(Id);
                if (!productImage) return false;

            }

            await _unitOfWork.productImageRepository.DeleteRangeAsync(Ids);
            var IsDeleted = await _IsCompletedAsync();

            return IsDeleted;
        }

        public async Task<IEnumerable<ProductImageDto>> FindAllProductImagesByProductIdAsync(long productId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(productId, nameof(productId));

            var productImages = await _unitOfWork.
                productImageRepository.
                GetAllProductProductIdAsync(productId);

            if (productImages is null || !productImages.Any())
                return null;

            var productImagesDtoList = _genericMapper.
                MapCollection<ProductImage, ProductImageDto>(productImages);
            return productImagesDtoList;
        }

        public async Task<ProductImageDto> FindByIdAsync(long id)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(id, nameof(id));

            var productImage = await _unitOfWork.productImageRepository.GetByIdAsTrackingAsync(id);
            if (productImage is null) return null;

            var productImageDto = _genericMapper.MapSingle<ProductImage, ProductImageDto>(productImage);
            return productImageDto;

        }

        public async Task<IEnumerable<ProductImageDto>> GetAllAsync()
        {
            var productImages = await _unitOfWork.
                productImageRepository.GetAllNoTrackingAsync();

            var productImagesDtos = _genericMapper.
                MapCollection<ProductImage, ProductImageDto>(productImages);

            return productImagesDtos;
        }

        public async Task<long> GetCountOfAsync()
        {
            var count = await _unitOfWork.productImageRepository.GetCountAsync();
            return count;
        }

        public async Task<IEnumerable<ProductImageDto>> GetPagedDataAsync(int pageNumber, int pageSize)
        {
            ParamaterException.CheckIfIntIsBiggerThanZero(pageNumber, nameof(pageNumber));
            ParamaterException.CheckIfIntIsBiggerThanZero(pageSize, nameof(pageSize));

            var productImages = await _unitOfWork.
                productImageRepository.GetPagedDataAsNoTractingAsync(pageNumber, pageSize);

            var productImagesDtos = _genericMapper.
                MapCollection<ProductImage, ProductImageDto>(productImages);

            return productImagesDtos;
        }

        public async Task<bool> UpdateByIdAsync(long Id, ProductImageDto dto)
        {

            ParamaterException.CheckIfLongIsBiggerThanZero(Id, nameof(Id));
            ParamaterException.CheckIfObjectIfNotNull(dto, nameof(dto));

            var productImage = await _unitOfWork.
                productImageRepository.GetByIdAsTrackingAsync(Id);
            if (productImage is null) return false;


            _genericMapper.MapSingle(dto, productImage);

            await _unitOfWork.productImageRepository.UpdateAsync(Id, productImage);

            var IsUpdated = await _IsCompletedAsync();
            return IsUpdated;

        }

        public async Task<bool> DeleteAllProductImagesByProductIdAsync(long productId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(productId, nameof(productId));

            await _unitOfWork.productImageRepository.DeleteAllProductImagesByProductIdAsync(productId);

            var IsDeleted = await _IsCompletedAsync();

            return IsDeleted;
        }
    }
}
