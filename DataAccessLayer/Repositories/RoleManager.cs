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
    public class RoleManager : IRoleManager
    {
        private readonly AppDbContext _context;
        private readonly ILogger<RoleManager> _logger;
        private readonly RoleManager<User> _roleManager;

        public RoleManager(AppDbContext context,ILogger<RoleManager> logger,RoleManager<User> roleManager)
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
