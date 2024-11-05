using BusinessLayer.Dtos;
using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Contracks
{
    public interface IPendingUserService
    {
        public Task <PendingUser> FindByEmailAndCodeAsync(string email,string code);

        public Task<bool> RemoveByIdAsync(string id);

        public Task<bool> AddNewPendingUserAsync(UserDto userDto,string RoleName);

        public Task<string> GetPendingUserRoleNameByEmailAndCode(string email, string code);

        Task<string> GetIdByEmailAndCodeAsync(string email, string code);
    }
}
