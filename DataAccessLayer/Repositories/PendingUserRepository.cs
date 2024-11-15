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
    public class PendingUserRepository : IPendingUserRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<PendingUserRepository> _logger;

        public PendingUserRepository(AppDbContext context, ILogger<PendingUserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        private Exception _HandelDataBaseException(Exception ex)
        {
            _logger.LogError(ex, "Database error occurred while accessing {TableName}. Error: {ErrorMessage}", "PendingUsers", ex.Message);

            return new Exception($"Database error occurred while accessing Users. Error: {ex.Message}");
        }

        public async Task<PendingUser> GetByEmailAndCodeAsync(string email, string code)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(email, nameof(email));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(code, nameof(code));

            try
            {
                var pendingUser = await _context.PendingUsers.FirstOrDefaultAsync(u => u.Email == email && u.Code == code);
                return pendingUser;
            }
            catch (Exception ex)
            {
                throw _HandelDataBaseException(ex);
            }
        }

        public async Task DeleteByIdAsync(string Id)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(Id, nameof(Id));

            try
            {
                var pendingUser = await _context.PendingUsers.FirstOrDefaultAsync(u => u.Id == Id);

                if (pendingUser is null) throw new KeyNotFoundException("Not found by id");

                _context.PendingUsers.Remove(pendingUser);
            }
            catch (Exception ex)
            {
                throw _HandelDataBaseException(ex);
            }
        }

        public async Task<PendingUser> GetByIdAsync(string Id)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(Id, nameof(Id));

            try
            {
                var pendingUser = await _context.PendingUsers.FirstOrDefaultAsync(u => u.Id == Id);
                return pendingUser;
            }
            catch (Exception ex)
            {
                throw _HandelDataBaseException(ex);
            }
        }

        public async Task AddAsync(PendingUser pendingUser)
        {
            ParamaterException.CheckIfObjectIfNotNull(pendingUser, nameof(pendingUser));

            try
            {
                await _context.PendingUsers.AddAsync(pendingUser);
            }
            catch (Exception ex)
            {
                throw _HandelDataBaseException(ex);
            }
        }

        public async Task<string> GetPendingUserRoleNameByEmailAndCode(string email, string code)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(email, nameof(email));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(code, nameof(code));

            try
            {
                var pendingUser = await GetByEmailAndCodeAsync(email, code);

                if (pendingUser is null) return null;

                return pendingUser.RoleName;
            }
            catch (Exception ex)
            {
                throw _HandelDataBaseException(ex);
            }

        }
    }
}
