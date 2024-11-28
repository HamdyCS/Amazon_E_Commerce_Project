
using DataAccessLayer.Contracks;
using DataAccessLayer.Data;
using DataAccessLayer.Identity.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Printing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Exceptions;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity;

namespace DataAccessLayer.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UserRepository> _logger;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        private Exception _HandelDataBaseException(Exception ex)
        {
            _logger.LogError(ex, "Database error occurred while accessing {TableName}. Error: {ErrorMessage}", "Users", ex.Message);

            return new Exception($"Database error occurred while accessing Users. Error: {ex.Message}");
        }
        public UserRepository(AppDbContext context, ILogger<UserRepository> logger, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<bool> AddAsync(User user, string Password)
        {
            ParamaterException.CheckIfObjectIfNotNull(user, nameof(user));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(Password, nameof(Password));

            try
            {

                var IsCreated = await _userManager.CreateAsync(user, Password);

                return IsCreated.Succeeded;
            }
            catch (Exception ex)
            {

                throw _HandelDataBaseException(ex);
            }
        }

        public async Task<bool> CheckIfEmailInSystemAsync(string email)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(email, nameof(email));

            try
            {
                var user = await _userManager.FindByEmailAsync(email);

                return user != null;



            }
            catch (Exception ex)
            {
                throw _HandelDataBaseException(ex);
            }

        }

        public async Task<bool> DeleteAsync(string Id)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(Id, nameof(Id));


            try
            {
                var user = await _userManager.FindByIdAsync(Id);

                if (user is null) return false;

                user.IsDeleted = true;
                user.DateOfDeleted = DateTime.UtcNow;

                 _context.Users.Update(user);
               
                var RowsAffeted = await _context.SaveChangesAsync();

                return RowsAffeted>0 ;
            }
            catch (Exception ex)
            {

                throw _HandelDataBaseException(ex);
            }


        }

        public async Task<bool> DeleteUserByEmailAsync(string email)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(email, nameof(email));
            try
            {
                var user = await _userManager.FindByEmailAsync(email);

                if (user == null) return false;

                var IsDeleted = await _userManager.DeleteAsync(user);

                return IsDeleted.Succeeded;
            }
            catch (Exception ex)
            {

                throw _HandelDataBaseException(ex);
            }
        }

        public async Task<IEnumerable<User>> GetAllNoTrackingAsync()
        {
            try
            {
                var users = await _context.Users.AsNoTracking().ToListAsync();
                return users;
            }
            catch (Exception ex)
            {
                throw _HandelDataBaseException(ex);
            }
        }

        public async Task<IEnumerable<User>> GetPagedDataAsNoTractingAsync(int pageNumber, int pageSize)
        {
            ParamaterException.CheckIfIntIsBiggerThanZero(pageNumber, nameof(pageNumber));
            ParamaterException.CheckIfIntIsBiggerThanZero(pageSize, nameof(pageSize));


            try
            {
                var users = await _context.Users.AsNoTracking().Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
                return users;
            }
            catch (Exception ex)
            {
                throw _HandelDataBaseException(ex);
            }
        }

        public async Task<IEnumerable<User>> GetPagedDataAsTractingAsync(int pageNumber, int pageSize)
        {
            ParamaterException.CheckIfIntIsBiggerThanZero(pageNumber, nameof(pageNumber));
            ParamaterException.CheckIfIntIsBiggerThanZero(pageSize, nameof(pageSize));
            try
            {
                var users = await _context.Users.AsTracking().Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
                return users;
            }
            catch (Exception ex)
            {
                throw _HandelDataBaseException(ex);
            }
        }

        public async Task<IEnumerable<User>> GetAllTrackingAsync()
        {
            try
            {
                var users = await _context.Users.AsTracking().ToListAsync();
                return users;
            }
            catch (Exception ex)
            {

                throw _HandelDataBaseException(ex);
            }
        }

        public async Task<User> GetByIdAsync(string Id)
        {

            ParamaterException.CheckIfStringIsNotNullOrEmpty(Id, nameof(Id));
            try
            {
                var user = await _userManager.FindByIdAsync(Id);
                return user;
            }
            catch (Exception ex)
            {

                throw _HandelDataBaseException(ex);
            }
        }

        public async Task<long> GetCountAsync()
        {
            try
            {
                var result = await _context.Users.CountAsync();
                return result;
            }
            catch (Exception ex)
            {

                throw _HandelDataBaseException(ex);
            }
        }

        public async Task<User> GetUserByEmailAndPasswordAsync(string email, string password)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(email, nameof(email));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(password, nameof(password));
            try
            {
                var user = await _userManager.FindByEmailAsync(email);

                if (user == null)
                    return null;

                var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);

                if (result.Succeeded)
                    return user;

                return null;

            }
            catch (Exception ex)
            {

                throw _HandelDataBaseException(ex);
            }
        }

        public async Task<bool> UpdateEmailByIdAsync(string ID, string NewEmail, string NewUserName)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(ID, nameof(ID));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(NewEmail, nameof(NewEmail));


            try
            {
                var user = await _userManager.FindByIdAsync(ID);

                if (user == null)
                    return false;

                user.Email = NewEmail;
                user.UserName = NewUserName;

                var IsUpdated = await _userManager.UpdateAsync(user);


                if (!IsUpdated.Succeeded)
                {
                    var errorMessage = string.Join(" ", IsUpdated.Errors.Select(e => e.Description));

                    _logger.LogInformation($"Error: {errorMessage}");


                }
                return IsUpdated.Succeeded;
            }
            catch (Exception ex)
            {

                throw _HandelDataBaseException(ex);
            }
        }

        public async Task<bool> UpdatePasswordByIdAsync(string Id, string password, string Newpassword)
        {


            ParamaterException.CheckIfStringIsNotNullOrEmpty(Id, nameof(Id));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(password, nameof(password));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(Newpassword, nameof(password));


            try
            {
                var user = await _userManager.FindByIdAsync(Id);

                if (user is null) return false;

                var IsUpdated = await _userManager.ChangePasswordAsync(user, password, Newpassword);

                if (!IsUpdated.Succeeded)
                {
                    var errorMessage = string.Join(" ", IsUpdated.Errors.Select(e => e.Description));

                    _logger.LogInformation($"Error: {errorMessage}");


                }

                return IsUpdated.Succeeded;
            }
            catch (Exception ex)
            {

                throw _HandelDataBaseException(ex);
            }

        }

        public async Task<IEnumerable<string>> GetUserRolesByIdAsync(string Id)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(Id, nameof(Id));


            try
            {
                var user = await _userManager.FindByIdAsync(Id);

                if (user == null)
                {
                    var message = "user is null";
                    _logger.LogInformation(message);
                    return null;
                }
                var UserRoles = await _userManager.GetRolesAsync(user);
                return UserRoles;

            }
            catch (Exception ex)
            {
                throw _HandelDataBaseException(ex);
            }
        }

        public async Task<bool> CheckIfUserInRoleByIdAsync(string Id, string Role)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(Id, nameof(Id));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(Role, nameof(Role));

            try
            {
                var user = await _userManager.FindByIdAsync(Id);

                if (user == null)
                {
                    var message = "user is null";
                    _logger.LogInformation(message);
                    return false;
                }
                var IsInRole = await _userManager.IsInRoleAsync(user, Role);
                return IsInRole;

            }
            catch (Exception ex)
            {
                throw _HandelDataBaseException(ex);
            }
        }

        public async Task<bool> DeleteUserFromRolesByIdAsync(string Id, IEnumerable<string> roles)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(Id, nameof(Id));
            ParamaterException.CheckIfIEnumerableIsNotNullOrEmpty(roles, nameof(roles));

            try
            {
                var user = await _userManager.FindByIdAsync(Id);

                if (user == null)
                {
                    var message = "user is null";
                    _logger.LogInformation(message);
                    return false;
                }

                var IsRemovedFromRole = await _userManager.RemoveFromRolesAsync(user, roles);

                if (!IsRemovedFromRole.Succeeded)
                {
                    var errorMesssage = string.Join(" ", IsRemovedFromRole.Errors.Select(e => e.Description));

                    _logger.LogInformation("Error" + errorMesssage);


                }

                return IsRemovedFromRole.Succeeded;
            }
            catch (Exception ex)
            {
                throw _HandelDataBaseException(ex);
            }
        }

        public async Task<bool> DeleteUserFromRoleByIdAsync(string Id, string Role)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(Id, nameof(Id));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(Role, nameof(Role));

            try
            {
                var user = await _userManager.FindByIdAsync(Id);

                if (user == null)
                {
                    var message = "user is null";
                    _logger.LogInformation(message);
                    return false;
                }

                var IsRemovedFromRole = await _userManager.RemoveFromRoleAsync(user, Role);

                if (!IsRemovedFromRole.Succeeded)
                {
                    var errorMesssage = string.Join(" ", IsRemovedFromRole.Errors.Select(e => e.Description));

                    _logger.LogInformation("Error" + errorMesssage);

                }

                return IsRemovedFromRole.Succeeded;
            }
            catch (Exception ex)
            {
                throw _HandelDataBaseException(ex);
            }
        }

        public async Task<bool> AddUserToRoleByIdAsync(string Id, string Role)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(Id, nameof(Id));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(Role, nameof(Role));

            try
            {
                var user = await _userManager.FindByIdAsync(Id);

                if (user is null) return false;

                var IsAddedToRole = await _userManager.AddToRoleAsync(user, Role);

                if (!IsAddedToRole.Succeeded)
                {
                    var errorMesssage = string.Join(" ", IsAddedToRole.Errors.Select(e => e.Description));

                    _logger.LogInformation("Error" + errorMesssage);

                }

                return IsAddedToRole.Succeeded;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> AddUserToRolesByIdAsync(string Id, IEnumerable<string> roles)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(Id, nameof(Id));
            ParamaterException.CheckIfIEnumerableIsNotNullOrEmpty(roles, nameof(roles));

            try
            {
                var user = await _userManager.FindByIdAsync(Id);

                if (user is null) return false;

                var IsAddedToRole = await _userManager.AddToRolesAsync(user, roles);

                if (!IsAddedToRole.Succeeded)
                {
                    var errorMesssage = string.Join(" ", IsAddedToRole.Errors.Select(e => e.Description));

                    _logger.LogInformation("Error" + errorMesssage);

                }

                return IsAddedToRole.Succeeded;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> UpdatePasswordByEmailAsync(string Email, string NewPassword)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(Email, nameof(Email));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(NewPassword, nameof(NewPassword));

            try
            {
                var user = await _userManager.FindByEmailAsync(Email);

                if (user is null) return false;


                var IsPasswordDeleted = await _userManager.RemovePasswordAsync(user);

                if (!IsPasswordDeleted.Succeeded) return false;

                var IsAddedPassword = await _userManager.AddPasswordAsync(user, NewPassword);

                return IsAddedPassword.Succeeded;

            }
            catch (Exception ex)
            {
                throw _HandelDataBaseException(ex);
            }
        }

        public async Task<User> GetByEmailAsync(string Email)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(Email, nameof(Email));
            try
            {
                var user = await _userManager.FindByEmailAsync(Email);
                return user;
            }
            catch (Exception ex)
            {

                throw _HandelDataBaseException(ex);
            }
        }

        public async Task<bool> IsUserDeletedByIdAsync(string Id)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(Id, nameof(Id));
            try
            {
                var user = await _userManager.FindByIdAsync(Id);
                if(user is null) return true;

                return user.IsDeleted;
            }
            catch(Exception ex)
            {
                throw;
            }
        }
    }
}
