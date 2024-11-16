using DataAccessLayer.Contracks;
using DataAccessLayer.Data;
using DataAccessLayer.Entities;
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
        private string _TableName = "UsersAddresses";
        public UserAddressRepository(AppDbContext context, ILogger<UserAddressRepository> logger) : base(context, logger, "UsersAddresses")
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<UserAddress>> GetAllUserAddressesAsNoTrackinByUserEmailAsync(string Email)
        {
            if (string.IsNullOrWhiteSpace(Email)) throw new ArgumentException("Email cannot be null or empty");

            try
            {
                var result = await _context.UsersAddresses.AsNoTracking().Where(u => u.user.Email == Email).ToListAsync();
                return result;
            }
            catch(Exception ex) 
            {
                throw HandleDatabaseException(ex);
            }
        }

        public async Task<IEnumerable<UserAddress>> GetAllUserAddressesAsNoTrackinByUserIdAsync(string UserId)
        {
            if (string.IsNullOrWhiteSpace(UserId)) throw new ArgumentException("UserId cannot be null or empty");

            try
            {
                var result = await _context.UsersAddresses.AsNoTracking().Where(u => u.UserId == UserId).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }

        public async Task<IEnumerable<UserAddress>> GetAllUserAddressesAsTrackinByUserEmailAsync(string Email)
        {
            if (string.IsNullOrWhiteSpace(Email)) throw new ArgumentException("Email cannot be null or empty");

            try
            {
                var result = await _context.UsersAddresses.AsTracking().Where(u => u.user.Email == Email).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }

        public async Task<IEnumerable<UserAddress>> GetAllUserAddressesAsTrackinByUserIdAsync(string UserId)
        {
            if (string.IsNullOrWhiteSpace(UserId)) throw new ArgumentException("UserId cannot be null or empty");

            try
            {
                var result = await _context.UsersAddresses.AsTracking().Where(u => u.UserId == UserId).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }
    }
}
