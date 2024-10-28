using DataAccessLayer.Contracks;
using DataAccessLayer.Data;
using DataAccessLayer.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class RoleManagerRepository : IRoleManagerRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<RoleManagerRepository> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleManagerRepository(AppDbContext context,ILogger<RoleManagerRepository> logger,RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _logger = logger;
            _roleManager = roleManager;
        }

        private Exception _HandleDataBaseException(Exception ex)
        {
            _logger.LogError(ex, "Database error occurred while accessing {TableName}. Error {Error Message}", "Roles", ex.Message);
            return new Exception($"Database error occurred while accessing Roles. Error {ex.Message}");
        }

        public async Task<IEnumerable<string>> GetAllRolesAsync()
        {
            try
            {
                var result = await _context.Roles.Select(r=>r.Name).ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                throw _HandleDataBaseException(ex);
            }
        }

        public async Task<bool> IsRoleExistByName(string roleName)
        {
            if (string.IsNullOrEmpty(roleName)) throw new ArgumentException("RoleName cannot be null or empty");
            try
            {
                var result = await _roleManager.FindByNameAsync(roleName);

                return result != null;
            }
            catch (Exception ex)
            {
                throw _HandleDataBaseException(ex);
            }
        }
    }
}
