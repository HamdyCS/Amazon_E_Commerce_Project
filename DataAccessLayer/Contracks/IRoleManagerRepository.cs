using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Contracks
{
    public interface IRoleManagerRepository
    {
        Task<IEnumerable<string>> GetAllRolesAsync();

        Task<bool> IsRoleExistByName(string roleName);

        //Task<bool>  AddAsync(string roleName);

        //Task<bool>  AddRangeAsync(IEnumerable<string> RolesNames);

        //Task<bool> DeleteAsync(string role);

        //Task<bool> DeleteRangeAsync(string role);

        //Task<bool> UpdateRoleNameByNameAsync(string roleName ,string NewRoleName);

    }
}
