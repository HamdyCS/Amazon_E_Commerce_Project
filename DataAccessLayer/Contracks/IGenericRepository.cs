using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Contracks
{
    public interface IGenericRepository<T> where T : class
    {
        public Task<T> GetByIdAsTrackingAsync(long id);

        public Task<T> GetByIdAsNoTrackingAsync(long id);

        public Task<IEnumerable<T>> GetAllAsNoTrackingAsync();

        public Task<IEnumerable<T>> GetAllAsTrackingAsync();

        public Task<long> GetCountAsync();

        public Task AddAsync(T entity);

        public Task AddRangeAsync(IEnumerable<T> entities);

        public Task UpdateAsync(long Id,T entity);

        public Task DeleteAsync(long id);

        public Task DeleteRangeAsync(IEnumerable<long> Ids);

        public Task<IEnumerable<T>> GetPagedDataAsNoTractingAsync(int pageNumber, int pageSize);

        public Task<IEnumerable<T>> GetPagedDataAsTractingAsync(int pageNumber, int pageSize);

        Task<bool> IsExistByIdAsync(long Id);

    }

}
