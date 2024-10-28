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

        public Task<bool> UpdateAsync(T dto);

        public Task<bool> DeleteByIdAsync(long Id);

        public Task<IEnumerable<T>> GetPagedAllAsync(int pageNumber, int pageSize);
    }
}
