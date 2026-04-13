using DataAccessLayer.Contracks;
using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using DataAccessLayer.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class UserAddressRepository : GenericRepository<UserAddress>, IUserAdderssRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UserAddressRepository> _logger;
        public UserAddressRepository(AppDbContext context, ILogger<UserAddressRepository> logger) : base(context, logger, "UsersAddresses")
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<UserAddress>> GetAllUserAddressesAsNoTrackinByUserEmailAsync(string Email)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(Email, nameof(Email));
            try
            {
                var result = await _context.UserAddresses.AsNoTracking().Where(u => u.user.Email == Email).ToListAsync();
                return result;
            }
            catch(Exception ex) 
            {
                throw HandleDatabaseException(ex);
            }
        }

        public async Task<IEnumerable<UserAddress>> GetAllUserAddressesAsNoTrackinByUserIdAsync(string UserId)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(UserId, nameof(UserId));


            try
            {
                var result = await _context.UserAddresses.AsNoTracking().Where(u => u.UserId == UserId).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }

        public async Task<IEnumerable<UserAddress>> GetAllUserAddressesAsTrackinByUserEmailAsync(string Email)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(Email, nameof(Email));


            try
            {
                var result = await _context.UserAddresses.AsTracking().Where(u => u.user.Email == Email).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }

        public async Task<IEnumerable<UserAddress>> GetAllUserAddressesAsTrackinByUserIdAsync(string UserId)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(UserId, nameof(UserId));

            try
            {
                var result = await _context.UserAddresses.AsTracking().Where(u => u.UserId == UserId).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }

        public async Task<int> GetCountOfUserAddressesByUserIdAsync(string userId)
        {
           ParamaterException.CheckIfStringIsNotNullOrEmpty(userId, nameof(userId));
            try
            {
                var count = await _context.UserAddresses.Where(e=>e.UserId == userId).CountAsync();
                return count;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }

        public async Task<UserAddress> GetByIdAndUserIdAsync(long Id, string userId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(Id, nameof(Id));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(userId, nameof(userId));

            try
            {
                var userAddress = await  _context.UserAddresses.AsNoTracking().FirstOrDefaultAsync(e=>e.Id == Id
                    && e.UserId == userId);

                return userAddress;
            }
            catch(Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }

        public async Task<bool> UpdateIsDefaultToFalseForAllUserAddressesByUserIdAsync(string userId)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(userId, nameof(userId));

            try
            {
                // Update all user addresses of the user to set IsDefault to false
                var rowsEffected = await _context.UserAddresses.Where(x => x.UserId == userId && x.IsDefault)
                   .ExecuteUpdateAsync(x => x.SetProperty(x => x.IsDefault, false));


                return rowsEffected > 0;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }

        public async Task<UserAddress> GetOldestUserAddressByUserIdAsync(string userId)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(userId, nameof(userId));

            var userAddress =await _context.UserAddresses.Where(e=>e.UserId == userId)
                .OrderBy(e=>e.CreatedAt).FirstOrDefaultAsync();

            return userAddress;
        }
    }
}
