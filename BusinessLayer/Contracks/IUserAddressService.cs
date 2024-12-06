using BusinessLayer.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Contracks
{
    public interface IUserAddressService 
    {
        public Task<UserAddressDto> AddAsync(string UserId,UserAddressDto UserAddressdto);


        public Task<bool> DeleteByIdAndUserIdAsync(long Id,string userId);

        public Task<UserAddressDto> FindByIdAndUserIdAsync(long id,string userid);

        public Task<IEnumerable<UserAddressDto>> GetAllUserAddressesByUserIdAsync(string userId);

        public Task<int> GetCountOfUserAddressesByUserId(string userId);


        public Task<bool> UpdateByIdAndUserIdAsync(long Id,string UserId, UserAddressDto UserAddressdto);
        
    }
}
