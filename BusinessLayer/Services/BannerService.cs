using BusinessLayer.Contracks;
using BusinessLayer.Dtos;
using BusinessLayer.Exceptions;
using BusinessLayer.Mapper.Contracks;
using DataAccessLayer.Entities;
using DataAccessLayer.UnitOfWork.Contracks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class BannerService : IBannerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericMapper _genericMapper;
        private readonly IImageService _imageService;

        private async Task<bool> _CompleteAsync()
        {
            var result = await _unitOfWork.CompleteAsync();

            return result > 0;
        }

        public BannerService(IUnitOfWork unitOfWork, IGenericMapper genericMapper, IImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _genericMapper = genericMapper;
            _imageService = imageService;
        }
        public async Task<BannerDto> AddBannerAsync(CreateBannerDto createBannerDto)
        {
            ParamaterException.CheckIfObjectIfNotNull(createBannerDto, nameof(createBannerDto));

            //get overlapping banners 
            var overrLappingBanners = await _unitOfWork.bannerRepository.GetOverLappingBannersOrderByDisplayOrderAsc(createBannerDto.StartDate, createBannerDto.EndDate);

            //get last display order 
            var lastDisplayOrder = 0;

            if (overrLappingBanners.Any())
                lastDisplayOrder = overrLappingBanners.Max(x => x.DisplayOrder);


            //mapping createBannerDto to banner
            var banner = _genericMapper.MapSingle<CreateBannerDto, Banner>(createBannerDto);

            //upload image to server and set image url to banner
            var imageDto = await _imageService.UploadImageAsync(createBannerDto.Image);
            if (imageDto is null)
                throw new InvalidOperationException("Failed to upload banner image.");

            //add banner information
            banner.ImageUrl = imageDto.Url;
            banner.PublicId = imageDto.PublicId;

            //set banner display order to last display order + 1
            banner.DisplayOrder = lastDisplayOrder + 1;


            //add banners to database
            await _unitOfWork.bannerRepository.AddAsync(banner);

            //save changes to database
            var isAdded = await _CompleteAsync();

            if (!isAdded)
            {
                //delete uploaded image from server if failed to add banner to database
                await _imageService.DeleteImageAsync(imageDto);
                throw new InvalidOperationException("Failed to add banner.");
            }


            var bannerDto = _genericMapper.MapSingle<Banner, BannerDto>(banner);
            return bannerDto;

        }

        public async Task<IEnumerable<BannerDto>> AddBannersAsync(IEnumerable<CreateBannerDto> createBannerDtos)
        {
            ParamaterException.CheckIfIEnumerableIsNotNullOrEmpty(createBannerDtos, nameof(createBannerDtos));

            var bannerDtos = new List<BannerDto>();
            foreach (var createBannerDto in createBannerDtos)
            {
                var bannerDto = await AddBannerAsync(createBannerDto);
                bannerDtos.Add(bannerDto);
            }
            return bannerDtos;
        }

        public async Task<bool> DeleteAllActiveBannersAsync()
        {
            //get active banners from database
            var activeBanners = await _unitOfWork.bannerRepository.GetActiveBannersOrderByDisplayOrderAsc();
            if (!activeBanners.Any())
                return false;

            //get active banner ids
            var activeBannerIds = activeBanners.Select(x => x.Id).ToList();

            //delete active banners from database
            await _unitOfWork.bannerRepository.DeleteRangeAsync(activeBannerIds);

            //commit changes to database
            var isDeleted = await _CompleteAsync();

            if (!isDeleted)
                throw new InvalidOperationException("Failed to delete active banners.");

            return true;
        }

        public async Task<bool> DeleteBannerByIdAsync(long id)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(id, nameof(id));

            //delete banner from database by id
            await _unitOfWork.bannerRepository.DeleteAsync(id);

            //commit changes to database
            var isDeleted = await _CompleteAsync();

            if (!isDeleted)
                throw new InvalidOperationException("Failed to delete banner.");

            return true;
        }

        public async Task<IEnumerable<BannerDto>> GetActiveBannersAsync()
        {
            //get active banners from database
            var activeBanners = await _unitOfWork.bannerRepository.GetActiveBannersOrderByDisplayOrderAsc();

            if (!activeBanners.Any())
                return Enumerable.Empty<BannerDto>();

            //mapping banners to bannerDtos
            var activeBannerDtos = _genericMapper.MapCollection<Banner, BannerDto>(activeBanners);
            return activeBannerDtos;
        }

        public async Task<IEnumerable<BannerDto>> GetAllBannersPagesAsync(int pageNumber, int pageSize)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(pageNumber, nameof(pageNumber));
            ParamaterException.CheckIfLongIsBiggerThanZero(pageSize, nameof(pageSize));

            //get banners from database with pagination
            var banners = await _unitOfWork.bannerRepository.GetPagedDataAsNoTractingAsync(pageNumber, pageSize);

            if (!banners.Any())
                return Enumerable.Empty<BannerDto>();

            //mapping banners to bannerDtos
            var bannerDtos = _genericMapper.MapCollection<Banner, BannerDto>(banners);
            return bannerDtos;
        }

        public async Task<BannerDto> GetBannerByIdAsync(long id)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(id, nameof(id));

            //get banner from database by id
            var banner = await _unitOfWork.bannerRepository.GetByIdAsNoTrackingAsync(id);
            if (banner is null)
                throw new KeyNotFoundException($"Banner with id {id} not found.");

            //mapping banner to bannerDto
            var bannerDto = _genericMapper.MapSingle<Banner, BannerDto>(banner);
            return bannerDto;

        }

        public async Task<BannerDto> UpdateBannerAsync(UpdateBannerDto updateBannerDto)
        {
            ParamaterException.CheckIfObjectIfNotNull(updateBannerDto, nameof(updateBannerDto));


            //get banner from database by id
            var banner = await _unitOfWork.bannerRepository.GetByIdAsNoTrackingAsync(updateBannerDto.Id);

            if (banner is null)
                throw new KeyNotFoundException($"Banner with id {updateBannerDto.Id} not found.");

            //get display order before update
            var oldDisplayOrder = banner.DisplayOrder;
            var newDisplayOrder = updateBannerDto.DisplayOrder;

            //update banner info
            _genericMapper.MapSingle(updateBannerDto, banner);

            //ovelapping banners
            var overlapingBanners = await _unitOfWork.bannerRepository.GetOverLappingBannersOrderByDisplayOrderAsc(banner.StartDate, banner.EndDate);

            //remove current banner from overlaping banners
            overlapingBanners = overlapingBanners.Where(x => x.Id != banner.Id).ToList();

            //update overlapping banners display order

            if (newDisplayOrder > oldDisplayOrder)
            {
                foreach (var overlapingBanner in overlapingBanners)
                {
                    if (overlapingBanner.DisplayOrder > oldDisplayOrder &&
                        overlapingBanner.DisplayOrder <= newDisplayOrder)
                    {
                        overlapingBanner.DisplayOrder--;
                    }
                }
            }

            if (newDisplayOrder < oldDisplayOrder)
            {
                foreach (var overlapingBanner in overlapingBanners)
                {
                    if (overlapingBanner.DisplayOrder >= newDisplayOrder &&
                        overlapingBanner.DisplayOrder < oldDisplayOrder)
                    {
                        overlapingBanner.DisplayOrder++;
                    }
                }
            }

            //update banners
            try
            {
                //start transaction
                await _unitOfWork.BeginTransactionAsync();

                //update current banner
                _unitOfWork.bannerRepository.Update(banner);

                //update overlaping banners
                _unitOfWork.bannerRepository.UpdateRange(overlapingBanners);

                var isUpdated = await _CompleteAsync();

                if (!isUpdated)
                    throw new InvalidOperationException("Failed to update banner.");

                //commit transaction
                await _unitOfWork.CommitTransactionAsync();


                //mapping banner to bannerDto
                var bannerDto = _genericMapper.MapSingle<Banner, BannerDto>(banner);
                return bannerDto;
            }
            catch
            {
                //rollback transaction if any error occurs
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<IEnumerable<BannerDto>> UpdateBannersAsync(IEnumerable<UpdateBannerDto> updateBannersDto)
        {
            ParamaterException.CheckIfIEnumerableIsNotNullOrEmpty(updateBannersDto, nameof(updateBannersDto));

            var updatedBannerDtos = new List<BannerDto>();
            foreach (var updateBannerDto in updateBannersDto)
            {
                var bannerDto = await UpdateBannerAsync(updateBannerDto);
                updatedBannerDtos.Add(bannerDto);
            }

            return updatedBannerDtos;
        }

    }
}
