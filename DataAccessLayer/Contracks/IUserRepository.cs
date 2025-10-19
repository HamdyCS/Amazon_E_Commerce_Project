using DataAccessLayer.Identity.Entities;
using Microsoft.AspNetCore.Authentication;

namespace DataAccessLayer.Contracks
{
    public interface IUserRepository
    {
        public Task<bool> AddAsync(User user, string Password);

        public Task<bool> CheckIfEmailInSystemAsync(string email);

        public Task<bool> DeleteAsync(string Id);

        public Task<bool> DeleteUserByEmailAsync(string email);

        public Task<IEnumerable<User>> GetAllNoTrackingAsync();

        public Task<IEnumerable<User>> GetPagedDataAsNoTractingAsync(int pageNumber, int pageSize);

        public Task<IEnumerable<User>> GetPagedDataAsTractingAsync(int pageNumber, int pageSize);

        public Task<IEnumerable<User>> GetAllTrackingAsync();

        public Task<User> GetByIdAsync(string id);

        public Task<User> GetByEmailAsync(string Email);


        public Task<long> GetCountAsync();

        public Task<User> GetUserByEmailAndPasswordAsync(string email, string password);

        public Task<bool> UpdateEmailByIdAsync(string Id, string NewEmail, string NewUserName);

        public Task<bool> UpdatePasswordByIdAsync(string Id, string password, string Newpassword);

        public Task<IEnumerable<string>> GetUserRolesByIdAsync(string Id);

        public Task<bool> CheckIfUserInRoleByIdAsync(string Id, string Role);

        Task<bool> DeleteUserFromRolesByIdAsync(string Id, IEnumerable<string> roles);

        Task<bool> DeleteUserFromRoleByIdAsync(string Id, string Role);

        Task<bool> AddUserToRoleByIdAsync(string Id, string Role);

        public Task<bool> AddUserToRolesByIdAsync(string Id, IEnumerable<string> roles);

        public Task<bool> UpdatePasswordByEmailAsync(string Email, string Password);

        Task<bool> IsUserDeletedByIdAsync(string Id);

        AuthenticationProperties CreateAuthenticationProperties(string provider, string redirectUrl);

        Task<User> LoginByProviderAsync(string role);
    }
}
