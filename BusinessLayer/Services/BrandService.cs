
using BusinessLayer.Contracks;
using BusinessLayer.Dtos;
using BusinessLayer.Exceptions;
using BusinessLayer.Mapper.Contracks;
using DataAccessLayer.Entities;
using DataAccessLayer.UnitOfWork.Contracks;
using Microsoft.Extensions.Logging;

namespace BusinessLayer.Servicese
{
    public class BrandService : IBrandService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<BrandService> _logger;
        private readonly IGenericMapper _genericMapper;
        private readonly IUserService _userService;
        private readonly IImageService _imageService;

        public BrandService(IUnitOfWork unitOfWork, ILogger<BrandService> logger, IGenericMapper genericMapper,
            IUserService userService, IImageService imageService)
        {
            this._unitOfWork = unitOfWork;
            this._logger = logger;
            this._genericMapper = genericMapper;
            this._userService = userService;
            _imageService = imageService;
        }

        public async Task<BrandDto> AddAsync(CreateBrandDto createBrandDto, string UserId)
        {
            ParamaterException.CheckIfObjectIfNotNull(createBrandDto, nameof(createBrandDto));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(UserId, nameof(UserId));
            try
            {
                var userDto = await _userService.FindByIdAsync(UserId);
                if (userDto == null) return null;


                var NewBrand = _genericMapper.MapSingle<CreateBrandDto, Brand>(createBrandDto);
                if (NewBrand is null) return null;

                NewBrand.CreatedBy = UserId;

                //Upload Image
                var imageDto = await _imageService.UploadImageAsync(createBrandDto.Image);
                if (imageDto is null)
                {
                    throw new Exception("Image didnot upload successfuly.");
                }

                NewBrand.ImageUrl = imageDto.Url;
                NewBrand.PublicId = imageDto.PublicId;


                await _unitOfWork.brandRepository.AddAsync(NewBrand);

                var IsNewBrandAdded = await _CompleteAsync();
                if (!IsNewBrandAdded) return null;

                var brandDto = _genericMapper.MapSingle<Brand, BrandDto>(NewBrand);

                return brandDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while adding a new brand. {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<BrandDto>> AddRangeAsync(IEnumerable<CreateBrandDto> createBrandDtos, string UserId)
        {
            ParamaterException.CheckIfIEnumerableIsNotNullOrEmpty(createBrandDtos, nameof(createBrandDtos));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(UserId, nameof(UserId));

            var userDto = await _userService.FindByIdAsync(UserId);
            if (userDto == null) return null;

            var NewBrandsDtoList = new List<BrandDto>();
            foreach (var d in createBrandDtos)
            {
                var NewBrandDto = await AddAsync(d, UserId);
                if (NewBrandDto != null) NewBrandsDtoList.Add(NewBrandDto);
            }

            if (!NewBrandsDtoList.Any()) return null;
            return NewBrandsDtoList;
        }

        public async Task<bool> DeleteByIdAsync(long Id)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(Id, nameof(Id));

            var brand = await _unitOfWork.brandRepository.GetByIdAsNoTrackingAsync(Id);
            if (brand == null) return false;

            await _unitOfWork.brandRepository.DeleteAsync(Id);

            var IsDeleted = await _CompleteAsync();

            return IsDeleted;
        }

        public async Task<BrandDto> FindByIdAsync(long id)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(id, nameof(id));
            var brand = await _unitOfWork.brandRepository.GetByIdAsTrackingAsync(id);

            if (brand is null) return null;

            var brandDto = _genericMapper.MapSingle<Brand, BrandDto>(brand);

            return brandDto;

        }

        public async Task<BrandDto> FindByNameArAsync(string NameAr)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(NameAr, nameof(NameAr));
            var brand = await _unitOfWork.brandRepository.GetByNameArAsync(NameAr);

            if (brand is null) return null;

            var brandDto = _genericMapper.MapSingle<Brand, BrandDto>(brand);

            return brandDto;
        }

        public async Task<BrandDto> FindByNameEnAsync(string NameEn)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(NameEn, nameof(NameEn));
            var brand = await _unitOfWork.brandRepository.GetByNameEnAsync(NameEn);

            if (brand is null) return null;

            var brandDto = _genericMapper.MapSingle<Brand, BrandDto>(brand);

            return brandDto;
        }

        public async Task<IEnumerable<BrandDto>> GetAllAsync()
        {
            var brands = await _unitOfWork.
              brandRepository.GetAllAsNoTrackingAsync();

            var brandsDtos = _genericMapper.
                MapCollection<Brand, BrandDto>(brands);

            return brandsDtos;
        }

        public async Task<long> GetCountAsync()
        {
            var count = await _unitOfWork.brandRepository.GetCountAsync();
            return count;
        }

        public async Task<IEnumerable<BrandDto>> GetPagedDataAsync(int pageNumber, int pageSize)
        {

            ParamaterException.CheckIfIntIsBiggerThanZero(pageNumber, nameof(pageNumber));
            ParamaterException.CheckIfIntIsBiggerThanZero(pageSize, nameof(pageSize));

            var brands = await _unitOfWork.
            brandRepository.GetPagedDataAsNoTractingAsync(pageNumber, pageSize);

            var brandsDtos = _genericMapper.
                MapCollection<Brand, BrandDto>(brands);

            return brandsDtos;
        }

        public async Task<bool> UpdateByIdAsync(long Id, CreateBrandDto createBrandDto)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(Id, nameof(Id));
            ParamaterException.CheckIfObjectIfNotNull(createBrandDto, nameof(createBrandDto));

            try
            {
                var brand = await _unitOfWork.brandRepository.GetByIdAsTrackingAsync(Id);
                if (brand == null) return false;

                //delete old image
                var isImageDeleted = await _imageService.DeleteImageAsync(

                    new ImageDto
                    {
                        PublicId = brand.PublicId,
                        Url = brand.ImageUrl
                    }
                );

                if (!isImageDeleted)
                    throw new Exception("Failed to delete old images from image server during brand update.");

                //upload new image
                var uploadImageDto = await _imageService.UploadImageAsync(createBrandDto.Image);
                if (uploadImageDto is null)
                {
                    throw new Exception("New brand image didnot upload successfully");
                }

                brand.ImageUrl = uploadImageDto.Url;
                brand.PublicId = uploadImageDto.PublicId;

                _genericMapper.MapSingle(createBrandDto, brand);

                //update brand
                await _unitOfWork.brandRepository.UpdateAsync(Id, brand);

                var IsBrandUpdated = await _CompleteAsync();

                return IsBrandUpdated;
            }
            catch (Exception ex)
            {
                string errMesssage = $"An error occurred while Updateing Brand. {ex.Message}";
                _logger.LogError(errMesssage, ex);
                throw;
            }

        }

        private async Task<bool> _CompleteAsync()
        {
            var result = await _unitOfWork.CompleteAsync();
            return result > 0;

        }

    }
}
