using DataAccessLayer.Contracks;
using DataAccessLayer.Data;
using DataAccessLayer.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DataAccessLayer.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UserRepository> _logger;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        private Exception _HandelException(Exception ex, string message)
        {
            _logger.LogError(ex, message);

            return new Exception(message);
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
            try
            {
               
                await _userManager.CreateAsync(user,Password);

            }
            catch (Exception ex)
            {
                var Message = "Error in Added new user to database";
                throw _HandelException(ex, Message);
            }
        }
        public async Task<bool> CheckIfEmailInSystem(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);

                return user != null;


            }
            catch (Exception ex)
            {
                string Message = "Error in Found User By Email from database";
                throw _HandelException(ex, Message);
            }


        }

        public void Delete(User entity)
        {
            try
            {
                _context.Users.Remove(entity);
            }
            catch (Exception ex)
            {
                var Message = "Error in deleted user from database";
                throw _HandelException(ex, Message);
            }
        }

        public async Task DeleteUserByEmail(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);

                if (user == null)
                {
                    _logger.LogWarning($"User is null , email = {email}");
                    return;
                }
            }
            catch (Exception ex)
            {
                var Message = "Error in deleted user from database";
                throw _HandelException(ex, Message);
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
                var message = "Error in get all users No Tracking";
                throw _HandelException(ex, message);
            }
        }

        public async Task<IEnumerable<User>> GetAllPagedAsNoTractingAsync(int pageNumber, int pageSize)
        {
            try
            {
                var users = await _context.Users.AsNoTracking().Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
                return users;
            }
            catch (Exception ex)
            {
                var message = $"Error in got Page of users as No Tracking, Page number = {pageNumber}, page size = {pageSize}";
                throw _HandelException(ex, message);
            }
        }

        public async Task<IEnumerable<User>> GetAllPagedAsTractingAsync(int pageNumber, int pageSize)
        {
            try
            {
                var users = await _context.Users.AsTracking().Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
                return users;
            }
            catch (Exception ex)
            {
                var message = $"Error in got Page of users as No Tracking, Page number = {pageNumber}, page size = {pageSize}";
                throw _HandelException(ex, message);
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
                var message = $"Error in got all users as tracking";
                throw _HandelException(ex, message);
            }
        }

        public async Task<User> GetById(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                return user;
            }
            catch (Exception ex)
            {
                var message = $"Error in got user by id {id}";
                throw _HandelException(ex, message);
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
                var message = $"Error in got count of users";
                throw _HandelException(ex, message);
            }
        }

        public async Task<User> GetUserByEmailAndPassword(string email, string password)
        {
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
                var message = $"Error in got user by email = {email} and password = {password}";
                throw _HandelException(ex, message);
            }
        }

        public async Task UpdateEmailByEmail(string email, string NewEmail)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);

                if (user == null)
                {
                    var message = $"User not found by email {email}";
                    _logger.LogWarning(message);
                    throw new Exception(message);
                }

                user.Email = NewEmail;

                var result = await _userManager.UpdateAsync(user);


                if (!result.Succeeded)
                {
                    var message = "Cannot update email";
                    throw new Exception(message);

                }

            }
            catch (Exception ex)
            {
                var message = $"error on updated email, email = {email}";
                throw _HandelException(ex, message);
            }
        }

        public async Task UpdatePasswordByEmail(User user,string password, string Newpassword)
        {
            try
            {

                var result = await _userManager.ChangePasswordAsync(user,password,Newpassword);

                if (!result.Succeeded)
                {
                    var message = "Cannot update password";
                    throw new Exception(message);

                }
            }
            catch (Exception ex)
            {
                var message = $"error on updated password";
                throw _HandelException(ex, message);
            }

        }
        

       
    }
}
