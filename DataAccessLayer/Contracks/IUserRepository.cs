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

        public Task<bool> CheckIfEmailInSystem(string email);

        public void Delete(User entity);

        public Task DeleteUserByEmail(string email);


        public Task<IEnumerable<User>> GetAllNoTrackingAsync();


        public Task<IEnumerable<User>> GetAllPagedAsNoTractingAsync(int pageNumber, int pageSize);


        public Task<IEnumerable<User>> GetAllPagedAsTractingAsync(int pageNumber, int pageSize);


        public Task<IEnumerable<User>> GetAllTrackingAsync();


        public Task<User> GetById(string id);

        public Task<long> GetCountAsync();


        public Task<User> GetUserByEmailAndPasswordAsync(string email, string password);


        public Task UpdateEmailByEmailAsync(string email, string NewEmail);


        public Task UpdatePasswordByEmailAsync(User user, string password, string Newpassword);

        public Task<IEnumerable<string>> GetUserRolesByEmailAsync(string Email);

        public Task<bool> CheckIfUserInRoleByEmailAsync(string Email,string Role);

        Task<  bool> DeleteUserFromRolesByEmailAsync(IEnumerable<string> roles,string Email);

        Task<bool> DeleteUserFromRoleByEmailAsync(string Role,string Email);
    }
}
