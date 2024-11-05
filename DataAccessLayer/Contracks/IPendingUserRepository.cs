using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Contracks
{
    public interface IPendingUserRepository 
    {
        Task<PendingUser> GetByEmailAndCodeAsync(string email,string code);

        Task DeleteByIdAsync(string Id);

        Task<PendingUser> GetByIdAsync(string Id);

        Task AddAsync(PendingUser pendingUser);

        public Task<string> GetPendingUserRoleNameByEmailAndCode(string email, string code);

    }
}
