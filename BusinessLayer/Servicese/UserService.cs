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
using System.Net.Mail;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Servicese
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPendingUserService _pendingUserService;
        private readonly IPersonService _personService;
        private readonly ILogger<UserService> _logger;
        private readonly IGenericMapper _genericMapper;

        private async Task<bool> _CompleteAsync()
        {
            try
            {
                var result = await _unitOfWork.CompleteAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public UserService(IUnitOfWork unitOfWork, IPendingUserService pendingUserService, IPersonService personService, ILogger<UserService> logger,
            IGenericMapper genericMapper)
        {
            _unitOfWork = unitOfWork;
            _pendingUserService = pendingUserService;
            _personService = personService;
            _logger = logger;
            _genericMapper = genericMapper;
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
            ParamaterException.CheckIfStringIsValid(Id, nameof(Id));
            try
            {
                await _unitOfWork.userRepository.DeleteAsync(Id);

                var rssult = await _CompleteAsync();

                return rssult;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<UserDto> FindByIdAsync(string Id)
        {
            ParamaterException.CheckIfStringIsValid(Id, nameof(Id));

            try
            {
                var user = await _unitOfWork.userRepository.GetByIdAsync(Id);

                if (user is null) return null;

                var userDto = _genericMapper.MapSingle<User, UserDto>(user);

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

                var usersDtos = _genericMapper.MapCollection<User, UserDto>(users);

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
            ParamaterException.CheckIfIntIsValid(pageNumber, nameof(pageNumber));
            ParamaterException.CheckIfIntIsValid(pageSize, nameof(pageSize));

            try
            {
                var users = await _unitOfWork.userRepository.GetPagedDataAsNoTractingAsync(pageNumber, pageSize);

                if (users is null) return null;

                var usersDtos = _genericMapper.MapCollection<User, UserDto>(users);

                return usersDtos;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> UpdateEmailAsync(string Id, string NewEmail)
        {
            ParamaterException.CheckIfStringIsValid(Id, nameof(Id));
            ParamaterException.CheckIfStringIsValid(NewEmail, nameof(NewEmail));


            try
            {
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

        public async Task<bool> UpdatePasswordAsync(string Id, string password, string NewPassword)
        {
            ParamaterException.CheckIfStringIsValid(Id, nameof(Id));
            ParamaterException.CheckIfStringIsValid(password, nameof(password));
            ParamaterException.CheckIfStringIsValid(NewPassword, nameof(NewPassword));

            try
            {
                var user = await _unitOfWork.userRepository.GetByIdAsync(Id);

                if (user is null) return false;

                var result = await _unitOfWork.userRepository.UpdatePasswordByIdAsync(Id, password, NewPassword);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> DeleteRangeByIdAsync(IEnumerable<string> Ids)
        {
            ParamaterException.CheckIfIEnumerableIsValid(Ids, nameof(Ids));
            try
            {
                foreach (var id in Ids)
                {
                    await _unitOfWork.userRepository.DeleteAsync(id);

                }
                var result = await _CompleteAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<RoleDto>> GetAllUserRolesByIdAsync(string userId)
        {
            ParamaterException.CheckIfStringIsValid(userId, nameof(userId));

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
            ParamaterException.CheckIfStringIsValid(UserID, nameof(UserID));
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
            ParamaterException.CheckIfStringIsValid(UserID, nameof(UserID));
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
            ParamaterException.CheckIfStringIsValid(UserID, nameof(UserID));
            ParamaterException.CheckIfIEnumerableIsValid(rolesDtos, nameof(rolesDtos));

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
            ParamaterException.CheckIfStringIsValid(UserID, nameof(UserID));
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
            ParamaterException.CheckIfStringIsValid(UserID, nameof(UserID));
            ParamaterException.CheckIfIEnumerableIsValid(rolesDtos, nameof(rolesDtos));

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

        public async Task<UserDto> ConfirmEmailByEmailAndCodeAsync(string Email, string code)
        {
            ParamaterException.CheckIfStringIsValid(Email, nameof(Email));
            ParamaterException.CheckIfStringIsValid(code, nameof(code));

            try
            {
                var pendingUser = await _pendingUserService.FindByEmailAndCodeAsync(Email, code);
                if (pendingUser is null) return null;


                var userDto = _genericMapper.MapSingle<PendingUser, UserDto>(pendingUser);
                if (userDto is null) return null;


                var personDto = _genericMapper.MapSingle<UserDto, PersonDto>(userDto);
                if (personDto == null) return null;


                await _unitOfWork.BeginTransactionAsync();


                var NewPersonDtoResult = await _personService.AddAsync(personDto);
                if (NewPersonDtoResult is null || NewPersonDtoResult.Id < 1) return null;


                var NewUser = _genericMapper.MapSingle<UserDto, User>(userDto);
                if (NewUser is null) return null;


                NewUser.PersonId = NewPersonDtoResult.Id;
                NewUser.UserName = new MailAddress(NewUser.Email).User;

                

                var IsCompleted = await _unitOfWork.userRepository.AddAsync(NewUser, userDto.Password); ;


                if (IsCompleted)
                {
                    await _unitOfWork.CommitTransactionAsync();
                    _genericMapper.MapSingle(NewUser, userDto);

                }

                return IsCompleted ? userDto : null;

            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }

        }

        public async Task<bool> IsEmailExistAsync(string Email)
        {
            ParamaterException.CheckIfStringIsValid(Email,nameof(Email));

            try
            {
                var IsEmailExist = await _unitOfWork.userRepository.CheckIfEmailInSystemAsync(Email);
                return IsEmailExist;
            }
            catch(Exception ex)
            {
                throw;
            }
        }
    }
}
