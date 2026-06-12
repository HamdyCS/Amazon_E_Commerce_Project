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
        private readonly IMailService _mailService;
        private readonly IEmailQueue _emailQueue;

        public OtpService(IConfiguration configuration, IUnitOfWork unitOfWork, ILogger<OtpService> logger,
            IGenericMapper genericMapper, IMailService mailService, IEmailQueue emailQueue)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _genericMapper = genericMapper;
            _mailService = mailService;
            _emailQueue = emailQueue;
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
            ParamaterException.CheckIfObjectIfNotNull(otpDto, nameof(otpDto));


            Otp otp = new Otp
            {
                Email = otpDto.Email,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Otp:LifeTimeMin"])),
                Code = otpDto.Otp,
                IsUsed = false,

            };

            //deactive all old opts with email
            await _unitOfWork.otpRepository.DeactiveAllEmailOtpsAsync(otpDto.
                  Email);

            //add otp
            await _unitOfWork.otpRepository.AddAsync(otp);
            var IsAdded = await _CompleteAsync();

            if (!IsAdded) throw new InvalidOperationException("Failed To Add Otp to Database.");

            //save otp in background service queue
            var emailQueueDto = new EmailQueueDto
            {
                Email = otpDto.Email,
                OTP = otpDto.Otp
            };
            await _emailQueue.EnQueueAsync(emailQueueDto);


            var NewOtpDto = _genericMapper.MapSingle<Otp, OtpDto>(otp);
            return NewOtpDto;

        }

        public async Task<bool> CheckIsOtpValidAsync(OtpDto otpDto)
        {
            ParamaterException.CheckIfObjectIfNotNull(otpDto, nameof(otpDto));

            try
            {
                var otp = await _unitOfWork.otpRepository.GetTheLastByEmailAndCodeAsync(otpDto.Email, otpDto.Otp);

                if (otp is null) return false;

                return otp.IsActive && !otp.IsUsed;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<OtpDto> GetByIdAsync(long id)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(id, nameof(id));


            var otp = await _unitOfWork.otpRepository.GetByIdAsNoTrackingAsync(id);

            if (otp is null) throw new KeyNotFoundException($"Otp not found.Id = ${id}");

            var otpDto = _genericMapper.MapSingle<Otp, OtpDto>(otp);
            return otpDto;

        }

        public async Task<bool> MakeOtpUsedAsync(OtpDto otpDto)
        {
            ParamaterException.CheckIfObjectIfNotNull(otpDto, nameof(otpDto));


            var otp = await _unitOfWork.otpRepository.GetTheLastByEmailAndCodeAsync(otpDto.Email, otpDto.Otp);
            if (otp is null) return false;
            otp.IsUsed = true;

            _unitOfWork.otpRepository.Update(otp);

            var IsUpdated = await _CompleteAsync();
            return IsUpdated;


        }

    }
}
