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

        public Task<IEnumerable<T>> GetAllNoTrackingAsync();

        public Task<IEnumerable<T>> GetAllTrackingAsync();

        public Task<long> GetCountAsync();

        public Task AddAsync(T entity);

        public Task AddRangeAsync(IEnumerable<T> entities);

        public void Update(T entity);

        public void UpdateRange(IEnumerable<T> entities);

        public void Delete(T entity);

        public void DeleteRange(IEnumerable<T> entities);

        public Task<IEnumerable<T>> GetAllPagedAsNoTractingAsync(int pageNumber, int pageSize);

        public Task<IEnumerable<T>> GetAllPagedAsTractingAsync(int pageNumber, int pageSize);

        public Task<bool> IsExistById(long id);
    }

}
