
using DataAccessLayer.Contracks;
using DataAccessLayer.Data;
using DataAccessLayer.Identity.Entities;
using Microsoft.AspNetCore.Identity;
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
            ParamaterException.CheckIfStringIsValid(Password, nameof(Password));

            try
            {

                var result = await _userManager.CreateAsync(user, Password);

                return result.Succeeded;
            }
            catch (Exception ex)
            {

                throw _HandelDataBaseException(ex);
            }
        }

        public async Task<bool> CheckIfEmailInSystemAsync(string email)
        {
            ParamaterException.CheckIfStringIsValid(email, nameof(email));

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

        public async Task DeleteAsync(string Id)
        {
            ParamaterException.CheckIfStringIsValid(Id, nameof(Id));


            try
            {
                var existingEntity = await _userManager.FindByIdAsync(Id);

                if (existingEntity is null) throw new KeyNotFoundException("Not found by id");

                await _userManager.DeleteAsync(existingEntity);
            }
            catch (Exception ex)
            {

                throw _HandelDataBaseException(ex);
            }


        }

        public async Task DeleteUserByEmailAsync(string email)
        {
            ParamaterException.CheckIfStringIsValid(email, nameof(email));
            try
            {
                var user = await _userManager.FindByEmailAsync(email);

                if (user == null)
                {
                    _logger.LogInformation("User is null");
                    throw new Exception("User is null");
                }
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
            ParamaterException.CheckIfIntsValid(pageNumber, nameof(pageNumber));
            ParamaterException.CheckIfIntsValid(pageSize, nameof(pageSize));


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
            ParamaterException.CheckIfIntsValid(pageNumber, nameof(pageNumber));
            ParamaterException.CheckIfIntsValid(pageSize, nameof(pageSize));
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

            ParamaterException.CheckIfStringIsValid(Id, nameof(Id));
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
            ParamaterException.CheckIfStringIsValid(email, nameof(email));
            ParamaterException.CheckIfStringIsValid(password, nameof(password));
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
            ParamaterException.CheckIfStringIsValid(ID, nameof(ID));
            ParamaterException.CheckIfStringIsValid(NewEmail, nameof(NewEmail));


            try
            {
                var user = await _userManager.FindByIdAsync(ID);

                if (user == null)
                {
                    var message = $"User not Found";
                    _logger.LogInformation(message);
                    throw new Exception(message);
                }

                user.Email = NewEmail;
                user.UserName = NewUserName;

                var result = await _userManager.UpdateAsync(user);


                if (!result.Succeeded)
                {
                    var errorMessage = string.Join(" ", result.Errors.Select(e => e.Description));

                    _logger.LogInformation($"Error: {errorMessage}");


                }
                return result.Succeeded;
            }
            catch (Exception ex)
            {

                throw _HandelDataBaseException(ex);
            }
        }

        public async Task<bool> UpdatePasswordByIdAsync(string Id, string password, string Newpassword)
        {


            ParamaterException.CheckIfStringIsValid(Id, nameof(Id));
            ParamaterException.CheckIfStringIsValid(password, nameof(password));
            ParamaterException.CheckIfStringIsValid(Newpassword, nameof(password));


            try
            {
                var user = await _userManager.FindByIdAsync(Id);

                if (user is null) return false;

                var result = await _userManager.ChangePasswordAsync(user, password, Newpassword);

                if (!result.Succeeded)
                {
                    var errorMessage = string.Join(" ", result.Errors.Select(e => e.Description));

                    _logger.LogInformation($"Error: {errorMessage}");


                }

                return result.Succeeded;
            }
            catch (Exception ex)
            {

                throw _HandelDataBaseException(ex);
            }

        }

        public async Task<IEnumerable<string>> GetUserRolesByIdAsync(string Id)
        {
            ParamaterException.CheckIfStringIsValid(Id, nameof(Id));


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
            ParamaterException.CheckIfStringIsValid(Id, nameof(Id));
            ParamaterException.CheckIfStringIsValid(Role, nameof(Role));

            try
            {
                var user = await _userManager.FindByIdAsync(Id);

                if (user == null)
                {
                    var message = "user is null";
                    _logger.LogInformation(message);
                    return false;
                }
                var result = await _userManager.IsInRoleAsync(user, Role);
                return result;

            }
            catch (Exception ex)
            {
                throw _HandelDataBaseException(ex);
            }
        }

        public async Task<bool> DeleteUserFromRolesByIdAsync(string Id, IEnumerable<string> roles)
        {
            ParamaterException.CheckIfStringIsValid(Id, nameof(Id));
            ParamaterException.CheckIfIEnumerableIsValid(roles, nameof(roles));

            try
            {
                var user = await _userManager.FindByIdAsync(Id);

                if (user == null)
                {
                    var message = "user is null";
                    _logger.LogInformation(message);
                    return false;
                }

                var result = await _userManager.RemoveFromRolesAsync(user, roles);

                if (!result.Succeeded)
                {
                    var errorMesssage = string.Join(" ", result.Errors.Select(e => e.Description));

                    _logger.LogInformation("Error" + errorMesssage);


                }

                return result.Succeeded;
            }
            catch (Exception ex)
            {
                throw _HandelDataBaseException(ex);
            }
        }

        public async Task<bool> DeleteUserFromRoleByIdAsync(string Id, string Role)
        {
            ParamaterException.CheckIfStringIsValid(Id, nameof(Id));
            ParamaterException.CheckIfStringIsValid(Role, nameof(Role));

            try
            {
                var user = await _userManager.FindByIdAsync(Id);

                if (user == null)
                {
                    var message = "user is null";
                    _logger.LogInformation(message);
                    return false;
                }

                var result = await _userManager.RemoveFromRoleAsync(user, Role);

                if (!result.Succeeded)
                {
                    var errorMesssage = string.Join(" ", result.Errors.Select(e => e.Description));

                    _logger.LogInformation("Error" + errorMesssage);

                }

                return result.Succeeded;
            }
            catch (Exception ex)
            {
                throw _HandelDataBaseException(ex);
            }
        }

        public async Task<bool> AddUserToRoleByIdAsync(string Id, string Role)
        {
            ParamaterException.CheckIfStringIsValid(Id, nameof(Id));
            ParamaterException.CheckIfStringIsValid(Role, nameof(Role));

            try
            {
                var user = await  _userManager.FindByIdAsync(Id);

                if (user is null) return false;

                var result = await _userManager.AddToRoleAsync(user, Role);

                if (!result.Succeeded)
                {
                    var errorMesssage = string.Join(" ", result.Errors.Select(e => e.Description));

                    _logger.LogInformation("Error" + errorMesssage);

                }

                return result.Succeeded;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> AddUserToRolesByIdAsync(string Id, IEnumerable<string> roles)
        {
            ParamaterException.CheckIfStringIsValid(Id, nameof(Id));
            ParamaterException.CheckIfIEnumerableIsValid(roles, nameof(roles));

            try
            {
                var user = await _userManager.FindByIdAsync(Id);

                if (user is null) return false;

                var result = await _userManager.AddToRolesAsync(user, roles);

                if (!result.Succeeded)
                {
                    var errorMesssage = string.Join(" ", result.Errors.Select(e => e.Description));

                    _logger.LogInformation("Error" + errorMesssage);

                }

                return result.Succeeded;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
