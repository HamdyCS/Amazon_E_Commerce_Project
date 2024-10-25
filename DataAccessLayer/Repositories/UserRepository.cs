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
            _logger.LogError(ex, "Database error occurred while accessing {TableName}. Error: {ErrorMessage}","Users",ex.Message);

            return new Exception($"Database error occurred while accessing Users. Error: {ex.Message}");
        }
        public UserRepository(AppDbContext context, ILogger<UserRepository> logger, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task AddAsync(User user, string Password)
        {
            if(user == null) throw new ArgumentNullException("User");
            if(string.IsNullOrEmpty(Password)) throw new ArgumentException("Password cannot be null or empty");

            try
            {
               
                await _userManager.CreateAsync(user,Password);

            }
            catch (Exception ex)
            {
                
                throw _HandelDataBaseException(ex);
            }
        }
        public async Task<bool> CheckIfEmailInSystem(string email)
        {
            

            if (string.IsNullOrEmpty(email)) throw new ArgumentException("Email cannot be null or empty");

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

        public void Delete(User user)
        {
            if (user == null) throw new ArgumentNullException("User");
            try
            {
                _context.Users.Remove(user);
            }
            catch (Exception ex)
            {

                throw _HandelDataBaseException(ex);
            }

           
        }

        public async Task DeleteUserByEmail(string email)
        {
            if (string.IsNullOrEmpty(email)) throw new ArgumentException("Email cannot be null or empty");

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

        public async Task<IEnumerable<User>> GetAllPagedAsNoTractingAsync(int pageNumber, int pageSize)
        {
            if (pageNumber < 1) throw new ArgumentException("Page number cannot be smaller than one");
            if (pageNumber < 1) throw new ArgumentException("Page size cannot be smaller than one");

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

        public async Task<IEnumerable<User>> GetAllPagedAsTractingAsync(int pageNumber, int pageSize)
        {
            if (pageNumber < 1) throw new ArgumentException("Page number cannot be smaller than one");
            if (pageNumber < 1) throw new ArgumentException("Page size cannot be smaller than one");
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

        public async Task<User> GetById(string Id)
        {
          
            if (string.IsNullOrEmpty(Id)) throw new ArgumentException("ID cannot be null or empty");
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
            if (string.IsNullOrEmpty(email)) throw new ArgumentException("Email cannot be null or empty");
            if (string.IsNullOrEmpty(password)) throw new ArgumentException("Password cannot be null or empty");

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

        public async Task UpdateEmailByEmailAsync(string email, string NewEmail)
        {
            if (string.IsNullOrEmpty(email)) throw new ArgumentException("Email cannot be null or empty");
            if (string.IsNullOrEmpty(NewEmail)) throw new ArgumentException("New Email cannot be null or empty");

            try
            {
                var user = await _userManager.FindByEmailAsync(email);

                if (user == null)
                {
                    var message = $"User not Found";
                    _logger.LogInformation(message);
                    throw new Exception(message);
                }

                user.Email = NewEmail;

                var result = await _userManager.UpdateAsync(user);


                if (!result.Succeeded)
                {
                    var errorMessage = string.Join(" ", result.Errors.Select(e => e.Description));

                    _logger.LogInformation($"Error: {errorMessage}");
                    throw new InvalidOperationException($"Cannot update email. Error : {errorMessage}");// InvalidOperationException  افضل للوضوح

                }

            }
            catch (Exception ex)
            {

                throw _HandelDataBaseException(ex);
            }
        }

        public async Task UpdatePasswordByEmailAsync(User user,string password, string Newpassword)
        {
            if(user == null) throw new ArgumentNullException("User");

            if (string.IsNullOrEmpty(password)) throw new ArgumentException("Password cannot be null or empty");
            if (string.IsNullOrEmpty(Newpassword)) throw new ArgumentException("New Password cannot be null or empty");


            try
            {      

                var result = await _userManager.ChangePasswordAsync(user,password,Newpassword);

                if (!result.Succeeded)
                {
                    var errorMessage = string.Join(" ", result.Errors.Select(e => e.Description));

                    _logger.LogInformation($"Error: {errorMessage}");
                    throw new InvalidOperationException($"Cannot update password. Error : {errorMessage}");// InvalidOperationException  افضل للوضوح

                }
            }
            catch (Exception ex)
            {

                throw _HandelDataBaseException(ex);
            }

        }
      
        public async Task<IEnumerable<string>> GetUserRolesByEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email)) throw new ArgumentException("Email cannot be null or empty");

            User user = null;
            try
            {
                user = await _userManager.FindByEmailAsync(email);

                if(user == null)
                {
                    var message = "user is null";
                    _logger.LogInformation(message);
                    throw new Exception(message);
                }
            }
            catch(Exception ex)
            {
                throw _HandelDataBaseException(ex);
            }

            try
            {
                var UserRoles = await  _userManager.GetRolesAsync(user);
                return UserRoles;

            }
            catch (Exception ex)
            {
                throw _HandelDataBaseException(ex);
            }
        }

        public async Task<bool> CheckIfUserInRoleByEmailAsync(string Email, string Role)
        {
            if (string.IsNullOrEmpty(Email)) throw new ArgumentException("Email cannot be null or empty");
            if (string.IsNullOrEmpty(Role)) throw new ArgumentException("Role cannot be null or empty");

            User user = null;
            try
            {
                user = await _userManager.FindByEmailAsync(Email);

                if (user == null)
                {
                    var message = "user is null";
                    _logger.LogInformation(message);
                    throw new Exception(message);
                }
            }
            catch (Exception ex)
            {
                throw _HandelDataBaseException(ex);
            }

            try
            {
                var result = await _userManager.IsInRoleAsync(user,Role);
                return result;

            }
            catch (Exception ex)
            {
                throw _HandelDataBaseException(ex);
            }
        }

        public async Task<bool> DeleteUserFromRolesByEmailAsync(IEnumerable<string> roles, string Email)
        {
            if (string.IsNullOrEmpty(Email)) throw new ArgumentException("Email cannot be null or empty");
            if (roles==null || !roles.Any()) throw new ArgumentException("roles cannot be null or empty");

            User user = null;
            try
            {
                user = await _userManager.FindByEmailAsync(Email);

                if (user == null)
                {
                    var message = "user is null";
                    _logger.LogInformation(message);
                    throw new Exception(message);
                }
            }
            catch (Exception ex)
            {
                throw _HandelDataBaseException(ex);
            }

            try
            {
                var result = await _userManager.RemoveFromRolesAsync(user,roles);
                
                if(!result.Succeeded)
                {
                    var errorMesssage = string.Join(" ", result.Errors.Select(e => e.Description));

                    _logger.LogInformation("Error"+errorMesssage);

                    throw new InvalidOperationException(errorMesssage);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw _HandelDataBaseException(ex);
            }
        }

        public async Task<bool> DeleteUserFromRoleByEmailAsync(string Role, string Email)
        {
            if (string.IsNullOrEmpty(Email)) throw new ArgumentException("Email cannot be null or empty");
            if (string.IsNullOrEmpty(Role)) throw new ArgumentException("ROle cannot be null or empty");
            User user = null;
            try
            {
                user = await _userManager.FindByEmailAsync(Email);

                if (user == null)
                {
                    var message = "user is null";
                    _logger.LogInformation(message);
                    throw new Exception(message);
                }
            }
            catch (Exception ex)
            {
                throw _HandelDataBaseException(ex);
            }

            try
            {
                var result = await _userManager.RemoveFromRoleAsync(user, Role);

                if (!result.Succeeded)
                {
                    var errorMesssage = string.Join(" ", result.Errors.Select(e => e.Description));

                    _logger.LogInformation("Error" + errorMesssage);

                    throw new InvalidOperationException(errorMesssage);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw _HandelDataBaseException(ex);
            }
        }
    }
}
