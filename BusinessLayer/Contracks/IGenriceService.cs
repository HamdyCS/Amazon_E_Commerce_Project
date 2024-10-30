using BusinessLayer.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Contracks
{
    public interface IGenriceService<T> where T : class
    {
        public Task<T> FindByIdAsync(long id);

        public Task<IEnumerable<T>> GetAllAsync();

        public Task<long> GetCountOfAsync();

        public Task<T> AddAsync(T dto);

        public Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> dtos);

        public Task<bool> UpdateByIdAsync(long Id,T dto);

        public Task<bool> DeleteByIdAsync(long Id);

        public Task<bool> DeleteRangeByIdAsync(IEnumerable<long> Ids);

        public Task<IEnumerable<T>> GetPagedDataAsync(int pageNumber, int pageSize);
    }
}
