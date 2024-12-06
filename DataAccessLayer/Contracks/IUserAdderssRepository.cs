using DataAccessLayer.Entities;
using DataAccessLayer.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Contracks
{
    public interface IUserAdderssRepository : IGenericRepository<UserAddress>
    {
        Task<IEnumerable<UserAddress>> GetAllUserAddressesAsTrackinByUserEmailAsync(string Email);
        Task<IEnumerable<UserAddress>> GetAllUserAddressesAsNoTrackinByUserEmailAsync(string Email);
        Task<IEnumerable<UserAddress>> GetAllUserAddressesAsTrackinByUserIdAsync(string UserId);
        Task<IEnumerable<UserAddress>> GetAllUserAddressesAsNoTrackinByUserIdAsync(string UserId);
        Task<int> GetCountOfUserAddressesByUserIdAsync(string  userId);  

        Task<UserAddress> GetByIdAndUserIdAsync(long Id,string userId);
    }
}
