using DataAccessLayer.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Contracks
{
    public interface IUserRepository
    {
        public Task AddAsync(User user, string Password);

        public Task<bool> CheckIfEmailInSystemAsync(string email);

        public Task DeleteAsync(string Id);

        public Task DeleteUserByEmailAsync(string email);


        public Task<IEnumerable<User>> GetAllNoTrackingAsync();


        public Task<IEnumerable<User>> GetPagedDataAsNoTractingAsync(int pageNumber, int pageSize);


        public Task<IEnumerable<User>> GetPagedDataAsTractingAsync(int pageNumber, int pageSize);


        public Task<IEnumerable<User>> GetAllTrackingAsync();


        public Task<User> GetByIdAsync(string id);

        public Task<long> GetCountAsync();


        public Task<User> GetUserByEmailAndPasswordAsync(string email, string password);


        public Task<bool> UpdateEmailByEmailAsync(string email, string NewEmail);


        public Task<bool> UpdatePasswordByEmailAsync(User user, string password, string Newpassword);

        public Task<IEnumerable<string>> GetUserRolesByEmailAsync(string Email);

        public Task<bool> CheckIfUserInRoleByEmailAsync(string Email,string Role);

        Task<  bool> DeleteUserFromRolesByEmailAsync(IEnumerable<string> roles,string Email);

        Task<bool> DeleteUserFromRoleByEmailAsync(string Role,string Email);
    }
}
