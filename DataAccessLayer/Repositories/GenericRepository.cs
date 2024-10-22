using DataAccessLayer.Contracks;
using DataAccessLayer.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDbContext _context;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }

        public  async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<T> entites)
        {
            await _context.Set<T>().AddRangeAsync(entites);
        }

        public void Delete(T entity)
        {
             _context.Set<T>().Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entites)
        {
            _context.Set<T>().RemoveRange(entites);
        }

        public async Task<IEnumerable<T>> GetAllNoTrackingAsync()
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllPagedAsNoTractingAsync(int pageNumber, int pageSize)
        {
            return await _context.Set<T>().AsNoTracking().Skip(pageNumber).Take(pageSize).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllTrackingAsync()
        {
            return await _context.Set<T>().AsTracking().ToListAsync();
        }

        public async Task<T> GetByIdAsync(long id)
        {         
           return await _context.Set<T>().FindAsync(id);
        }

        public async Task<long> GetCountAsync()
        {
            return await _context.Set<T>().CountAsync();
        }

        public void Update(T entity)
        {
             _context.Set<T>().Update(entity);
        }

        public void UpdateRange(IEnumerable<T> entites)
        {
            _context.Set<T>().UpdateRange(entites);
        }
    }
}
