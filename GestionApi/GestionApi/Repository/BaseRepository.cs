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
        private readonly DbContext _context;

        public BaseRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddAsync<T>(T entity) where T : BaseModel
        {
            var _dbSet = _context.Set<T>();

            if(_dbSet == null)
            {
                return false;
            }

            await _dbSet.AddAsync(entity);
            return await SaveChanges();
        }

        public async Task<bool> DeleteAsync<T>(Guid id) where T : BaseModel
        {
            var _dbSet = _context.Set<T>();

            if (id == Guid.Empty)
            {
                return false;
            }   

            var commentModel = await _dbSet.FindAsync(id);

            if (commentModel == null)
            {
                return false;
            }

            _dbSet.Remove(commentModel);
            return await SaveChanges();
        }

        public async Task<bool> ExistAsync<T>(Expression<Func<T, bool>> predicate) where T : BaseModel
        {
            var _dbSet = _context.Set<T>();
            return await _dbSet.AnyAsync(predicate);
        }

        public async Task<T?> GetByFuncAsync<T>(Expression<Func<T, bool>> predicate) where T : BaseModel
        {
            var _dbSet = _context.Set<T>();
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
            var _dbSet = _context.Set<T>();
            return await _dbSet.ToListAsync();
        }

        public async Task<bool> UpdateAsync<T>(T entity) where T : BaseModel
        {
            var _dbSet = _context.Set<T>();
            _dbSet.Update(entity);
            return await SaveChanges();
        }

        private async Task<bool> SaveChanges()
        {
            var result = await _context.SaveChangesAsync();
            var success = result > 0;

            if (!success)
            {
                throw new CustomException(type: TypeException.Repository);
            }
                
            return success;
        }
    }
}
