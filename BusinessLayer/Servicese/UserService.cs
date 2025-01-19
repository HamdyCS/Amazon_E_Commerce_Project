using BusinessLayer.Contracks;
using BusinessLayer.Dtos;
using BusinessLayer.Exceptions;
using BusinessLayer.Mapper.Contracks;
using DataAccessLayer.Entities;
using DataAccessLayer.Identity.Entities;
using DataAccessLayer.UnitOfWork;
using DataAccessLayer.UnitOfWork.Contracks;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Servicese
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPersonService _personService;
        private readonly ILogger<UserService> _logger;
        private readonly IGenericMapper _genericMapper;
        private readonly IOtpService _otpService;

      
        public UserService(IUnitOfWork unitOfWork, IPersonService personService, ILogger<UserService> logger,
            IGenericMapper genericMapper, IOtpService otpService)
        {
            _unitOfWork = unitOfWork;
            _personService = personService;
            _logger = logger;
            _genericMapper = genericMapper;
            _otpService = otpService;
        }


        public async Task<UserDto> GetUserByEmailAndPasswordAsync(LoginDto loginDto)
        {
            ParamaterException.CheckIfObjectIfNotNull(loginDto, nameof(loginDto));

            try
            {
                var user = await _unitOfWork.userRepository.GetUserByEmailAndPasswordAsync(loginDto.Email, loginDto.Password);

                if (user is null) return null;

                var userDto = _genericMapper.MapSingle<User, UserDto>(user);

                return userDto;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> DeleteByIdAsync(string Id)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(Id, nameof(Id));
            try
            {
                
                var IsDeleted = await _unitOfWork.userRepository.DeleteAsync(Id);

                return IsDeleted;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<UserDto> FindByIdAsync(string Id)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(Id, nameof(Id));

            try
            {
                var user = await _unitOfWork.userRepository.GetByIdAsync(Id);

                if (user is null) return null;

                var userDto = _genericMapper.MapSingle<User, UserDto>(user);

                var person = await _unitOfWork.personRepository.GetByIdAsNoTrackingAsync(user.PersonId);

                if (user is null) return null;

                _genericMapper.MapSingle(person,userDto);

                return userDto;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            try
            {
                var users = await _unitOfWork.userRepository.GetAllNoTrackingAsync();

                if (users is null) return null;

                List<UserDto> usersDtos = new();
                foreach (var user in users)
                {
                    var userDto = _genericMapper.MapSingle<User,UserDto>(user);
                    if (userDto is null) return null;
                    
                    var person = await _personService.FindByIdAsync(user.PersonId);
                    if (person is null) return null;

                    _genericMapper.MapSingle(person, userDto);

                    usersDtos.Add(userDto);
                }
                return usersDtos;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<long> GetCountOfUsersAsync()
        {
            try
            {
                var count = await _unitOfWork.userRepository.GetCountAsync();
                return count;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<UserDto>> GetPagedDataAsync(int pageNumber, int pageSize)
        {
            ParamaterException.CheckIfIntIsBiggerThanZero(pageNumber, nameof(pageNumber));
            ParamaterException.CheckIfIntIsBiggerThanZero(pageSize, nameof(pageSize));

            try
            {
                var users = await _unitOfWork.userRepository.GetPagedDataAsNoTractingAsync(pageNumber, pageSize);

                if (users is null) return null;

                List<UserDto> usersDtos = new();
                foreach (var user in users)
                {
                    var userDto = _genericMapper.MapSingle<User, UserDto>(user);
                    if (userDto is null) return null;

                    var person = await _personService.FindByIdAsync(user.PersonId);
                    if (person is null) return null;

                    _genericMapper.MapSingle(person, userDto);

                    usersDtos.Add(userDto);
                }
                return usersDtos;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> UpdateEmailAsync(string Id, string NewEmail, string Code)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(Id, nameof(Id));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(NewEmail, nameof(NewEmail));


            try
            {
                var otpDto = new OtpDto { Code = Code ,Email = NewEmail};

                var IsOtpValid = await _otpService.CheckIfOtpActiveAndNotUsedAsync(otpDto);
                if(!IsOtpValid) return false;

                var user = await _unitOfWork.userRepository.GetByIdAsync(Id);

                if (user is null) return false;

                var NewUserName = new MailAddress(NewEmail).User;

                var result = await _unitOfWork.userRepository.UpdateEmailByIdAsync(Id, NewEmail, NewUserName);

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> UpdatePasswordAsync(string Id, string NewPassword)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(Id, nameof(Id));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(NewPassword, nameof(NewPassword));

            try
            {
                var user = await _unitOfWork.userRepository.GetByIdAsync(Id);
                if (user is null) return false;


                var IsPasswordUpdate = await _unitOfWork.userRepository.UpdatePasswordByEmailAsync(user.Email, NewPassword);
                return IsPasswordUpdate;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<RoleDto>> GetAllUserRolesByIdAsync(string userId)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(userId, nameof(userId));

            try
            {
                var user = await _unitOfWork.userRepository.GetByIdAsync(userId);

                if (user is null) return null;

                var userRoles = await _unitOfWork.userRepository.GetUserRolesByIdAsync(userId);

                var userRolesDtos = new List<RoleDto>();

                foreach (var role in userRoles)
                    userRolesDtos.Add(new RoleDto { Name = role });

                return userRolesDtos;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> IsUserInRoleByIdAsync(string UserID, RoleDto roleDto)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(UserID, nameof(UserID));
            ParamaterException.CheckIfObjectIfNotNull(roleDto, nameof(roleDto));

            try
            {
                var user = await _unitOfWork.userRepository.GetByIdAsync(UserID);

                if (user is null) return false;

                var result = await _unitOfWork.userRepository.CheckIfUserInRoleByIdAsync(UserID, roleDto.Name);

                return result;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> DeleteUserFromRoleByIdAsync(string UserID, RoleDto roleDto)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(UserID, nameof(UserID));
            ParamaterException.CheckIfObjectIfNotNull(roleDto, nameof(roleDto));

            try
            {
                var user = await _unitOfWork.userRepository.GetByIdAsync(UserID);

                if (user is null) return false;

                var result = await _unitOfWork.userRepository.DeleteUserFromRoleByIdAsync(UserID, roleDto.Name);

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> DeleteUserFromRolesByIdAsync(string UserID, IEnumerable<RoleDto> rolesDtos)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(UserID, nameof(UserID));
            ParamaterException.CheckIfIEnumerableIsNotNullOrEmpty(rolesDtos, nameof(rolesDtos));

            try
            {
                var user = await _unitOfWork.userRepository.GetByIdAsync(UserID);

                if (user is null) return false;

                var roles = new List<string>();

                foreach (var roleDto in rolesDtos)
                    roles.Add(roleDto.Name);

                var result = await _unitOfWork.userRepository.DeleteUserFromRolesByIdAsync(UserID, roles);

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> AddToRoleByIdAsync(string UserID, RoleDto roleDto)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(UserID, nameof(UserID));
            ParamaterException.CheckIfObjectIfNotNull(roleDto, nameof(roleDto));

            try
            {
                var user = await _unitOfWork.userRepository.GetByIdAsync(UserID);

                if (user is null) return false;

                var result = await _unitOfWork.userRepository.AddUserToRoleByIdAsync(UserID, roleDto.Name);

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> AddToRolesByIdAsync(string UserID, IEnumerable<RoleDto> rolesDtos)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(UserID, nameof(UserID));
            ParamaterException.CheckIfIEnumerableIsNotNullOrEmpty(rolesDtos, nameof(rolesDtos));

            try
            {
                var user = await _unitOfWork.userRepository.GetByIdAsync(UserID);

                if (user is null) return false;

                var roles = new List<string>();

                foreach (var roleDto in rolesDtos)
                    roles.Add(roleDto.Name);

                var result = await _unitOfWork.userRepository.AddUserToRolesByIdAsync(UserID, roles);

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<UserDto> AddNewUserByEmailAndCodeAsync(UserDto userDto, string Email, string code)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(Email, nameof(Email));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(code, nameof(code));
            ParamaterException.CheckIfObjectIfNotNull(userDto, nameof(userDto));

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var otpDto = new OtpDto { Email = Email, Code = code };

                var IsOtpValid = await _otpService.CheckIfOtpActiveAndNotUsedAsync(otpDto);
                if (!IsOtpValid) return null;

                var CheckIsEmailInSystem = await IsEmailExistAsync(Email);
                if (CheckIsEmailInSystem) return null;


                var personDto = _genericMapper.MapSingle<UserDto, PersonDto>(userDto);
                if (personDto is null) return null;

                personDto = await _personService.AddAsync(personDto);
                if (personDto.Id < 1) return null;

                var user = _genericMapper.MapSingle<UserDto, User>(userDto);
                if (user is null) return null;

                user.CreatedAt = DateTime.UtcNow;
                user.PersonId = personDto.Id;
                user.UserName = new MailAddress(userDto.Email).User;

                var IsUserAdded = await _unitOfWork.userRepository.AddAsync(user, userDto.Password);
                if (!IsUserAdded) return null;


                userDto = _genericMapper.MapSingle<User, UserDto>(user);

                var IsOtpUpdated = await _otpService.MakeOtpUsedAsync(otpDto);
                if (!IsOtpUpdated) return null;

                await _unitOfWork.CommitTransactionAsync();
                return userDto;

            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }

        }

        public async Task<bool> IsEmailExistAsync(string Email)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(Email, nameof(Email));

            try
            {
                var IsEmailExist = await _unitOfWork.userRepository.CheckIfEmailInSystemAsync(Email);
                return IsEmailExist;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> ResetPasswordByEmailAsync(string Email, string Password, string Code)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(Email, nameof(Email));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(Password, nameof(Password));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(Code, nameof(Code));

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var otpDto = new OtpDto { Email = Email, Code = Code };

                var user = await _unitOfWork.userRepository.GetByEmailAsync(Email);
                if (user is null) return false;

                var IsOtpValid = await _otpService.CheckIfOtpActiveAndNotUsedAsync(otpDto);
                if (!IsOtpValid) return false;


                var IsPasswordUpdate = await _unitOfWork.userRepository.UpdatePasswordByEmailAsync(Email, Password);
                if (!IsPasswordUpdate) return false;

                var IsotpUpdated = await _otpService.MakeOtpUsedAsync(otpDto);
                if (!IsotpUpdated) return false;

                await _unitOfWork.CommitTransactionAsync();
                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<bool> IsUserDeletedByIdAsync(string Id)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(Id, nameof(Id));

            try
            {
                var IsUserDeleted = await _unitOfWork.userRepository.IsUserDeletedByIdAsync(Id);
                return IsUserDeleted;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> UpdateUserByIdAsync(string Id, UserDto userDto)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(Id, nameof(Id));
            ParamaterException.CheckIfObjectIfNotNull(userDto, nameof(userDto));

            try
            {
                var user = await _unitOfWork.userRepository.GetByIdAsync(Id);
                if (user is null) return false;

                var personDto = _genericMapper.MapSingle<UserDto, PersonDto>(userDto);
                if (personDto is null) return false;

                var IsPersonUpdated = await _personService.UpdateByIdAsync(user.PersonId, personDto);

                
                return IsPersonUpdated;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<UserDto> FindByEmailAsync(string Email)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(Email, nameof(Email));

            try
            {
                var user = await _unitOfWork.userRepository.GetByEmailAsync(Email);

                if (user is null) return null;

                var userDto = _genericMapper.MapSingle<User, UserDto>(user);

                var person = await _unitOfWork.personRepository.GetByIdAsNoTrackingAsync(user.PersonId);

                if (user is null) return null;

                _genericMapper.MapSingle(person, userDto);

                return userDto;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
