using GestionApi.Models;
using System.Linq.Expressions;

namespace GestionApi.Repository.Interfaces
{
    public interface IBaseRepository 
    {
        Task<IEnumerable<T>> GetAllAsync<T>() where T : BaseModel; 
        Task<T> GetByIdAsync<T>(Guid id) where T : BaseModel;
        Task<T?> AddAsync<T>(T entity) where T : BaseModel;
        Task<T?> UpdateAsync<T>(T entity) where T : BaseModel;
        Task<bool> DeleteAsync<T>(Guid id) where T : BaseModel;
        Task<T?> GetByFuncAsync<T>(Expression<Func<T, bool>> predicate) where T : BaseModel;
        Task<bool> ExistAsync<T>(Expression<Func<T, bool>> predicate) where T : BaseModel;
    }
}
