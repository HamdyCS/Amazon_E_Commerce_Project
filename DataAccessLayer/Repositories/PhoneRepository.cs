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
    public class PhoneRepository : GenericRepository<Phone>, IPhoneRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<PhoneRepository> _logger;
        private string _TableName = "Phones";

        public PhoneRepository(AppDbContext context, ILogger<PhoneRepository> logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Phone> GetPhoneAsNoTrackingByUserEmailAsync(string email)
        {
            if(string.IsNullOrEmpty(email)) throw new ArgumentException("Email cannot be null or empty");

            try
            {
                var phone = await _context.Phones.AsNoTracking().FirstOrDefaultAsync(p => p.user.Email == email);
                return phone;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex, _TableName);
            }
        }

        public async Task<Phone> GetPhoneAsNoTrackingByUserIdAsync(string UserId)
        {
            if (string.IsNullOrEmpty(UserId)) throw new ArgumentException("UserId cannot be null or empty");

            try
            {
                var phone = await _context.Phones.AsNoTracking().FirstOrDefaultAsync(p => p.userId == UserId);
                return phone;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex, _TableName);
            }
        }

        public async Task<Phone> GetPhoneAsTrackingByUserEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email)) throw new ArgumentException("Email cannot be null or empty");

            try
            {
                var phone = await _context.Phones.AsTracking().FirstOrDefaultAsync(p => p.user.Email == email);
                return phone;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex, _TableName);
            }
        }
 
        public async Task<Phone> GetPhoneAsTrackingByUserIdAsync(string UserId)
        {
            if (string.IsNullOrEmpty(UserId)) throw new ArgumentException("UserId cannot be null or empty");

            try
            {
                var phone = await _context.Phones.AsTracking().FirstOrDefaultAsync(p => p.userId==UserId);
                return phone;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex, _TableName);
            }
        }
    }
}
