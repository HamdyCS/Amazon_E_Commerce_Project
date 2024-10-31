using BusinessLayer.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Contracks
{
    public interface IRoleService 
    {
        Task<IEnumerable<RoleDto>> GetAllRolesAsync();

        Task<bool> CheckIfRoleInSystemByNameAsync(string roleName);
    }
}
