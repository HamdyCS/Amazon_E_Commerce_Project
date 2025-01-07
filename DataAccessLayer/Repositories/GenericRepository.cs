using DataAccessLayer.Contracks;
using DataAccessLayer.Data;
using DataAccessLayer.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly ILogger<GenericRepository<T>> _logger;
        private readonly string _tableName;

        public GenericRepository(AppDbContext context, ILogger<GenericRepository<T>> logger, string TableName)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _tableName = TableName;
        }

        protected Exception HandleDatabaseException(Exception ex)
        {
            _logger.LogError(ex, "Database error occurred while accessing {TableName}. Error: {ErrorMessage}", _tableName, ex.Message);
            return new Exception($"Database error occurred while accessing {_tableName}. Error: {ex.Message}");
        }

        public async Task AddAsync(T entity)
        {
            ParamaterException.CheckIfObjectIfNotNull(entity, nameof(entity));

            try
            {
                await _context.Set<T>().AddAsync(entity);
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            ParamaterException.CheckIfIEnumerableIsNotNullOrEmpty(entities, nameof(entities));


            try
            {
                await _context.Set<T>().AddRangeAsync(entities);
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }

        public async Task DeleteAsync(long Id)
        {

            ParamaterException.CheckIfLongIsBiggerThanZero(Id, nameof(Id));

            try
            {
                var entity = await GetByIdAsTrackingAsync(Id);

                if (entity is null)
                    return;

                var IsDeletedProperty = _context.Set<T>().Entry(entity).Property("IsDeleted");

                if (IsDeletedProperty is null)
                {
                    _context.Set<T>().Remove(entity);
                }
                else
                {
                    IsDeletedProperty.CurrentValue = true;

                    var DateOfDeletionProperty = _context.Set<T>().Entry(entity).Property("DateOfDeletion");

                    if(DateOfDeletionProperty is not null)
                        DateOfDeletionProperty.CurrentValue = DateTime.UtcNow;

                }
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }

        public async Task DeleteRangeAsync(IEnumerable<long> Ids)
        {
            ParamaterException.CheckIfIEnumerableIsNotNullOrEmpty(Ids, nameof(Ids));

            try
            {
                foreach (long Id in Ids)
                {
                   await DeleteAsync(Id);
                }
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }

        public async Task<IEnumerable<T>> GetAllAsNoTrackingAsync()
        {
            try
            {
                return await _context.Set<T>().AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }

        public async Task<IEnumerable<T>> GetPagedDataAsNoTractingAsync(int pageNumber, int pageSize)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(pageNumber, nameof(pageNumber));
            ParamaterException.CheckIfLongIsBiggerThanZero(pageSize, nameof(pageSize));

            try
            {
                return await _context.Set<T>().AsNoTracking().Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }

        public async Task<IEnumerable<T>> GetPagedDataAsTractingAsync(int pageNumber, int pageSize)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(pageNumber, nameof(pageNumber));
            ParamaterException.CheckIfLongIsBiggerThanZero(pageSize, nameof(pageSize));

            try
            {
                return await _context.Set<T>().AsTracking().Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }

        public async Task<IEnumerable<T>> GetAllAsTrackingAsync()
        {
            try
            {
                return await _context.Set<T>().AsTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }

        public async Task<T> GetByIdAsTrackingAsync(long id)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(id, nameof(id));

            try
            {
                var entity = await _context.Set<T>().FindAsync(id);

                return entity;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }

        public async Task<T> GetByIdAsNoTrackingAsync(long id)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(id, nameof(id));

            try
            {
                var entity = await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(e => EF.Property<long>(e, "Id") == id);
                return entity;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
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
                throw HandleDatabaseException(ex);
            }
        }

        public async Task UpdateAsync(long Id, T entity)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(Id, nameof(Id));
            ParamaterException.CheckIfObjectIfNotNull(entity, nameof(entity));


            try
            {
                var existingEntity = await GetByIdAsTrackingAsync(Id);

                if (existingEntity == null) return;

                _context.Set<T>().Entry(existingEntity).CurrentValues.SetValues(entity);// بيعمل مقارنة وبيعدل المختلف
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }

        public async Task<bool> IsExistByIdAsync(long Id)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(Id,nameof(Id));

            var existingEntity = await _context.Set<T>().FindAsync(Id);

            return existingEntity is not null;
        }

    }

}
