using BusinessLayer.Contracks;
using BusinessLayer.Dtos;
using BusinessLayer.Exceptions;
using BusinessLayer.Help;
using BusinessLayer.Mapper.Contracks;
using DataAccessLayer.Entities;
using DataAccessLayer.UnitOfWork.Contracks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Servicese
{
    public class OtpService : IOtpService
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<OtpService> _logger;
        private readonly IGenericMapper _genericMapper;

        public OtpService(IConfiguration configuration, IUnitOfWork unitOfWork, ILogger<OtpService> logger, IGenericMapper genericMapper)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _genericMapper = genericMapper;
        }

        private async Task<bool> _CompleteAsync()
        {
            try
            {
                var RowsAffeted = await _unitOfWork.CompleteAsync();

                return RowsAffeted > 0;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<OtpDto> AddNewOtpAsync(OtpDto otpDto)
        {
            ParamaterException.CheckIfObjectIfNotNull(otpDto,nameof(otpDto));

            try
            {
                Otp otp = new Otp
                {
                    Email =  otpDto.Email,
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Otp:LifeTimeMin"])),
                    Code = otpDto.Code,
                    IsUsed = false,

                };

                
                await _unitOfWork.otpRepository.AddAsync(otp);
                var IsAdded = await _CompleteAsync();

                if (!IsAdded) return null;

                var NewOtpDto = _genericMapper.MapSingle<Otp,OtpDto>(otp);

                return NewOtpDto;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> CheckIfOtpActiveAsync(OtpDto otpDto)
        {
            ParamaterException.CheckIfObjectIfNotNull(otpDto, nameof(otpDto));

            try
            {
                var otp = await _unitOfWork.otpRepository.GetTheLastByEmailAndCodeAsync(otpDto.Email,otpDto.Code);

                if (otp is null) return false;

                return otp.IsActive;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> CheckIfOtpUsedAsync(OtpDto otpDto)
        {
            ParamaterException.CheckIfObjectIfNotNull(otpDto, nameof(otpDto));

            try
            {
                var otp = await _unitOfWork.otpRepository.GetTheLastByEmailAndCodeAsync(otpDto.Email, otpDto.Code);

                if (otp is null) return false;

                return otp.IsUsed;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<OtpDto> GetByIdAsync(long id)
        {
           ParamaterException.CheckIfLongIsBiggerThanZero(id,nameof(id));

            try
            {
                var otp = await _unitOfWork.otpRepository.GetByIdAsTrackingAsync(id);

                if (otp is null) return null;

                var otpDto = _genericMapper.MapSingle<Otp,OtpDto>(otp);
                return otpDto;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> MakeOtpUsedAsync(OtpDto otpDto)
        {
            ParamaterException.CheckIfObjectIfNotNull(otpDto, nameof(otpDto));

            try
            {
                var otp = await _unitOfWork.otpRepository.GetTheLastByEmailAndCodeAsync(otpDto.Email, otpDto.Code);

                if (otp is null) return false;

                otp.IsUsed = true;

                var IsUpdated = await _CompleteAsync();

                return IsUpdated;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> CheckIfOtpActiveAndNotUsedAsync(OtpDto otpDto)
        {
            bool IsCodeActive = await CheckIfOtpActiveAsync(otpDto);
            bool IsCodeUsed = await CheckIfOtpUsedAsync(otpDto);
            return IsCodeActive && !IsCodeUsed;
        }
    }
}
