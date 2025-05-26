using GestionApi.Data;
using GestionApi.Exceptions;
using GestionApi.Exceptions.Types;
using GestionApi.Models;
using GestionApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GestionApi.Repository
{
    public class BaseRepository : IBaseRepository
    {
        private readonly ApplicationDBContext _context;

        public BaseRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<T?> AddAsync<T>(T entity) where T : BaseModel
        {
            var _dbSet = _context.Set<T>();

            if (_dbSet == null)
            {
                return null;
            }

            var entityAdded = (await _dbSet.AddAsync(entity)).Entity;

            return await SaveChanges() ? entityAdded : null;
        }

        public async Task<bool> DeleteAsync<T>(Expression<Func<T, bool>>? predicate = null) where T : BaseModel
        {
            var _dbSet = _context.Set<T>();

            if (predicate == null)
            {
                return false;
            }

            var entity = await _dbSet.FirstOrDefaultAsync(predicate);

            if (entity == null)
            {
                return false;
            }

            _dbSet.Remove(entity);
            return await SaveChanges();
        }

        public async Task<bool> ExistAsync<T>(Expression<Func<T, bool>>? predicate = null) where T : BaseModel
        {
            var _dbSet = _context.Set<T>();

            if(predicate == null)
            {
                return false;
            }

            return await _dbSet.AnyAsync(predicate);
        }

        public async Task<T?> GetByFuncAsync<T>(Expression<Func<T, bool>>? predicate = null) where T : BaseModel
        {
            var _dbSet = _context.Set<T>();

            if (predicate == null)
            {
                return null;
            }

            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        public async Task<T?> GetByIdAsync<T>(Guid id) where T : BaseModel
        {
            var _dbSet = _context.Set<T>();

            if (id == Guid.Empty)
            {
                return null;
            }

            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>() where T : BaseModel
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T?> UpdateAsync<T>(T entity) where T : BaseModel
        {
            var _dbSet = _context.Set<T>();

            var existingEntity = await _dbSet.FindAsync(entity.Id);

            if (existingEntity == null)
            {
                return null;
            }

            _context.Entry(existingEntity).CurrentValues.SetValues(entity);

            return await SaveChanges() ? existingEntity : null;
        }

        private async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
