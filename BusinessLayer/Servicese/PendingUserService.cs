using BusinessLayer.Contracks;
using BusinessLayer.Dtos;
using BusinessLayer.Exceptions;
using BusinessLayer.Help;
using BusinessLayer.Mapper.Contracks;
using DataAccessLayer.Entities;
using DataAccessLayer.UnitOfWork.Contracks;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Servicese
{
    public class PendingUserService : IPendingUserService
    {
        private readonly IMailService _mailService;
        private readonly ILogger<PendingUserService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericMapper _genericMapper;

        public PendingUserService(IMailService mailService,ILogger<PendingUserService> logger,IUnitOfWork unitOfWork,IGenericMapper genericMapper)
        {
            _mailService = mailService;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _genericMapper = genericMapper;
        }

        private async Task<bool> _CompleteAsync()
        {
            try
            {
                var RowsAffected = await _unitOfWork.CompleteAsync();
                return RowsAffected > 0;

            }
            catch (Exception ex) { throw; }
        }

        public async Task<bool> AddNewPendingUserAsync(UserDto userDto, string RoleName)
        {
            ParamaterException.CheckIfObjectIfNotNull(userDto, nameof(userDto));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(RoleName, nameof(RoleName));

            try
            {
                var IsEmailExist = await _unitOfWork.userRepository.CheckIfEmailInSystemAsync(userDto.Email);

                if (IsEmailExist) return false;

                var pendingUser = _genericMapper.MapSingle<UserDto,PendingUser>(userDto);

                if (pendingUser == null) return false;

                pendingUser.RoleName = RoleName;
                pendingUser.Id = Guid.NewGuid().ToString();
                pendingUser.Code = Helper.GenerateRandomSixDigitNumber().ToString();

                await _mailService.SendEmailAsync(userDto.Email, "Confirmation code", pendingUser.Code);

                await _unitOfWork.PendingUserRepository.AddAsync(pendingUser);

                var IsCompleted = await _CompleteAsync();

                return IsCompleted;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<PendingUser> FindByEmailAndCodeAsync(string email, string code)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(email, nameof(email));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(code, nameof(code));

            try
            {
                var PendingUser = await _unitOfWork.PendingUserRepository.GetByEmailAndCodeAsync(email, code);
                return PendingUser;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> RemoveByIdAsync(string id)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(id, nameof(id));

            try
            {
                 await _unitOfWork.PendingUserRepository.DeleteByIdAsync(id);
                var IsCompleted = await _CompleteAsync();
                return IsCompleted;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<string> GetPendingUserRoleNameByEmailAndCode(string email, string code)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(email, nameof(email));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(code, nameof(code));

            try
            {
                var RoleName = await _unitOfWork.PendingUserRepository.GetPendingUserRoleNameByEmailAndCode(email, code);

                return RoleName;
            }
            catch(Exception ex)
            {
                throw;
            }

        }

        public async Task<string> GetIdByEmailAndCodeAsync(string email, string code)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(email, nameof(email));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(code, nameof(code));

            try
            {
                var pendingUser = await FindByEmailAndCodeAsync(email, code);

                if (pendingUser is null) return null;

                return pendingUser.Id;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
