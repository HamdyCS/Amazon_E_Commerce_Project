using BusinessLayer.Dtos;
using BusinessLayer.Servicese;
using DataAccessLayer.Identity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Contracks
{
    public interface IUserService 
    {

        public  Task<UserDto> GetUserByEmailAndPasswordAsync(LoginDto loginDto);

        public Task<bool> DeleteByIdAsync(string Id);

        public Task<UserDto> FindByIdAsync(string Id);

        public Task<IEnumerable<UserDto>> GetAllAsync();

        public Task<long> GetCountOfUsersAsync();

        public Task<IEnumerable<UserDto>> GetPagedDataAsync(int pageNumber, int pageSize);

        public  Task<bool> UpdateEmailAsync(string Id,  string NewEmail);

        public  Task<bool> UpdatePasswordAsync(string Id,string password, string NewPassword);

        public Task<IEnumerable< RoleDto>> GetAllUserRolesByIdAsync(string userId);

        public Task<bool> IsUserInRoleByIdAsync(string UserID,RoleDto roleDto);

        public Task<bool> DeleteUserFromRoleByIdAsync(string UserID,RoleDto roleDto);

        public Task<bool> DeleteUserFromRolesByIdAsync(string UserID, IEnumerable<RoleDto> rolesDtos);

        public Task<bool> AddToRoleByIdAsync(string UserID, RoleDto roleDto);

        public Task<bool> AddToRolesByIdAsync(string UserID, IEnumerable<RoleDto> rolesDtos);

        public Task<UserDto> AddNewUserByEmailAndCodeAsync(string Email,string code);

        public Task<bool> IsEmailExistAsync(string Email);

        public Task<bool> ResetPasswordByEmailAsync(string Email,string Password);
    }
}
