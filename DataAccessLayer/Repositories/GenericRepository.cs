using DataAccessLayer.Contracks;
using DataAccessLayer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<GenericRepository<T>> _logger;

        public GenericRepository(AppDbContext context, ILogger<GenericRepository<T>> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddAsync(T entity)
        {
            try
            {
                await _context.Set<T>().AddAsync(entity);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on added a new entity ,The entity name is {nameof(T)}");
            }

        }

        public async Task AddRangeAsync(IEnumerable<T> entites)
        {
            
            try
            {
                await _context.Set<T>().AddRangeAsync(entites);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on added a new Range Entities ,The entity name is {nameof(T)}");
            }
        }

        public void Delete(T entity)
        {
            try
            {
                _context.Set<T>().Remove(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on Removed a Entity ,The entity name is {nameof(T)}");
            }
        }

        public void DeleteRange(IEnumerable<T> entites)
        {
            try
            {
                _context.Set<T>().RemoveRange(entites);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on Removed a Range Entities ,The entity name is {nameof(T)}");
            }
        }

        public async Task<IEnumerable<T>> GetAllNoTrackingAsync()
        {
            try
            {
                return await _context.Set<T>().AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on got all as no tracking ,The entity name is {nameof(T)}");

                return null;
            }
        }

        public async Task<IEnumerable<T>> GetAllPagedAsNoTractingAsync(int pageNumber, int pageSize)
        {
            try
            {
                return await _context.Set<T>().AsNoTracking().Skip(pageNumber).Take(pageSize).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on got all Paged As No Tracting ,The entity name is {nameof(T)}");

                return null;
            }
            
        }

        public async Task<IEnumerable<T>> GetAllPagedAsTractingAsync(int pageNumber, int pageSize)
        {
            try
            {
                return await _context.Set<T>().AsTracking().Skip(pageNumber).Take(pageSize).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on got all Paged As Tracting ,The entity name is {nameof(T)}");

                return null;
            }
        }

        public async Task<IEnumerable<T>> GetAllTrackingAsync()
        {
            try
            {
                return await _context.Set<T>().AsTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on got all as tracking ,The entity name is {nameof(T)}");

                return null;
            }
        }

        public async Task<T> GetByIdAsTrackingAsync(long id)
        {
            try
            {
                return await _context.Set<T>().FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on got by id as tracking ,Id = {id} ,The entity name is {nameof(T)}");

                return null;
            }
        }

        public async Task<T> GetByIdAsNoTrackingAsync(long id)
        {
            try
            {
                return await _context.Set<T>().FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on got by id as no tracking ,Id = {id} ,The entity name is {nameof(T)}");

                return null;
            }
        }

        public async Task<long> GetCountAsync()
        {
            try
            {
                return await _context.Set<T>().CountAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on got Number of rows ,The entity name is {nameof(T)}");

                return -1;
            }            
        }

        public void Update(T entity)
        {
            try
            {
                _context.Set<T>().Update(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on updated entity ,The entity name is {nameof(T)}");

            }
            
        }

        public void UpdateRange(IEnumerable<T> entites)
        {
            try
            {
                _context.Set<T>().UpdateRange(entites);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on updated range of entites ,The entity name is {nameof(T)}");

            }
        }
    }
}
