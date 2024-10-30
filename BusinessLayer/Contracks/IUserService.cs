using BusinessLayer.Dtos;
using BusinessLayer.Servicese;
using DataAccessLayer.Identity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Contracks
{
    public interface IUserService 
    {
        public  Task<UserDto> AddAsync(UserDto dto);

        public Task<IEnumerable<UserDto>> AddRangeAsync(IEnumerable<UserDto> dtos);

        public  Task<bool> CheckEmailAndPasswordAsync(LoginDto loginDto);

        public Task<bool> DeleteByIdAsync(string Id);

        public Task<UserDto> FindByIdAsync(string Id);

        public Task<IEnumerable<UserDto>> GetAllAsync();

        public Task<long> GetCountOfAsync();

        public Task<IEnumerable<UserDto>> GetPagedDataAsync(int pageNumber, int pageSize);

        public  Task<bool> UpdateEmailAsync(string Id, string Email, string NewEmail);
        public  Task<bool> UpdatePasswordAsync(string Id, string password, string NewPassword);
        public Task<bool> DeleteRangeByIdAsync(IEnumerable<string> Ids);
        
    }
}
