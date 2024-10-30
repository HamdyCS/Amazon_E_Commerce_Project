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

        public UserService(IUnitOfWork unitOfWork, IPersonService personService, ILogger<UserService> logger,
            IGenericMapper genericMapper)
        {
            _unitOfWork = unitOfWork;
            _personService = personService;
            _logger = logger;
            _genericMapper = genericMapper;
        }


        public async Task<UserDto> AddAsync(UserDto dto)
        {
            ParamaterException.CheckIfObjectIfNotNull(dto, nameof(dto));

            try
            {
                var personDto = _genericMapper.MapSingle<UserDto, PersonDto>(dto);

                if (personDto == null) return null;


                var NewPersonDtoResult = await _personService.AddAsync(personDto);

                if (NewPersonDtoResult is null || NewPersonDtoResult.Id < 1) return null;


                var NewUser = _genericMapper.MapSingle<UserDto, User>(dto);

                if (NewUser is null) return null;

                NewUser.PersonId = NewPersonDtoResult.Id;


                await _unitOfWork.userRepository.AddAsync(NewUser, dto.Password);

                var IsCompleted = await _CompleteAsync();


                if (IsCompleted)
                    _genericMapper.MapSingle(NewUser, dto);


                return IsCompleted ? dto : null;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<UserDto>> AddRangeAsync(IEnumerable<UserDto> dtos)
        {
            ParamaterException.CheckIfIEnumerableIsValid(dtos, nameof(dtos));
            try
            {
                List<UserDto> result = new List<UserDto>();

                foreach (var dto in dtos)
                {
                    var userDto = await AddAsync(dto);

                    if (userDto == null) continue;

                    result.Add(userDto);
                }

                return result.Any() ? result : null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> CheckEmailAndPasswordAsync(LoginDto loginDto)
        {
            ParamaterException.CheckIfObjectIfNotNull(loginDto, nameof(loginDto));

            try
            {
                var result = await _unitOfWork.userRepository.GetUserByEmailAndPasswordAsync(loginDto.Email, loginDto.Password);

                return result != null;
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

        public async Task<long> GetCountOfAsync()
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
            ParamaterException.CheckIfIntValueIsValid(pageNumber, nameof(pageNumber));
            ParamaterException.CheckIfIntValueIsValid(pageSize, nameof(pageSize));

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

        public async Task<bool> UpdateEmailAsync(string Id, string Email, string NewEmail)
        {
            ParamaterException.CheckIfStringIsValid(Id, nameof(Id));
            ParamaterException.CheckIfStringIsValid(Email, nameof(Email));
            ParamaterException.CheckIfStringIsValid(NewEmail, nameof(NewEmail));


            try
            {
                var result = await _unitOfWork.userRepository.UpdateEmailByEmailAsync(Email, NewEmail);

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Task<bool> UpdatePasswordAsync(string Id, string password, string NewPassword)
        {
            throw new NotImplementedException();
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
    }
}
