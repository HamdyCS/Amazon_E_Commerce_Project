
using BusinessLayer.Contracks;
using BusinessLayer.Dtos;
using BusinessLayer.Exceptions;
using BusinessLayer.Mapper.Contracks;
using DataAccessLayer.Entities;
using DataAccessLayer.Identity.Entities;
using DataAccessLayer.UnitOfWork.Contracks;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Servicese
{
    public class BrandService : IBrandService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<BrandService> _logger;
        private readonly IGenericMapper _genericMapper;
        private readonly IUserService _userService;

        public BrandService(IUnitOfWork unitOfWork, ILogger<BrandService> logger, IGenericMapper genericMapper,
            IUserService userService)
        {
            this._unitOfWork = unitOfWork;
            this._logger = logger;
            this._genericMapper = genericMapper;
            this._userService = userService;
        }

        public async Task<BrandDto> AddAsync(BrandDto dto, string UserId)
        {
            ParamaterException.CheckIfObjectIfNotNull(dto, nameof(dto));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(UserId, nameof(UserId));

            var userDto = await _userService.FindByIdAsync(UserId);
            if (userDto == null) return null;


            var NewBrand = _genericMapper.MapSingle<BrandDto, Brand>(dto);

            if (NewBrand is null) return null;

            NewBrand.CreatedBy = UserId;

            await _unitOfWork.brandRepository.AddAsync(NewBrand);

            var IsNewBrandAdded = await _CompleteAsync();
            if (!IsNewBrandAdded) return null;

            _genericMapper.MapSingle(NewBrand, dto);

            return dto;
        }

        public async Task<IEnumerable<BrandDto>> AddRangeAsync(IEnumerable<BrandDto> dtos, string UserId)
        {
            ParamaterException.CheckIfIEnumerableIsNotNullOrEmpty(dtos, nameof(dtos));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(UserId, nameof(UserId));

            var userDto = await _userService.FindByIdAsync(UserId);
            if (userDto == null) return null;

            var NewBrandsDtoList = new List<BrandDto>();
            foreach (var d in dtos)
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
            brandRepository.GetPagedDataAsNoTractingAsync(pageNumber,pageSize);

            var brandsDtos = _genericMapper.
                MapCollection<Brand, BrandDto>(brands);

            return brandsDtos;
        }

        public async Task<bool> UpdateByIdAsync(long Id, BrandDto dto)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(Id, nameof(Id));
            ParamaterException.CheckIfObjectIfNotNull(dto, nameof(dto));

            var brand = await _unitOfWork.brandRepository.GetByIdAsTrackingAsync(Id);

            if (brand == null) return false;

            _genericMapper.MapSingle(dto, brand);

            await _unitOfWork.brandRepository.UpdateAsync(Id,brand);

            var IsBrandUpdated = await _CompleteAsync();

            return IsBrandUpdated;
        }

        private async Task<bool> _CompleteAsync()
        {
            var result = await _unitOfWork.CompleteAsync();
            return result > 0;

        }
        
    }
}
