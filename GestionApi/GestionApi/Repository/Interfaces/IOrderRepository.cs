using GestionApi.Models;
using System.Linq.Expressions;

namespace GestionApi.Repository.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetOrdersAsync();
        Task<Order> GetOrderByIdAsync(Guid id);
        Task<Order?> GetOrderAsync(Expression<Func<Order, bool>> predicate);
        Task<bool> AddOrderAsync(Order order);
        Task<bool> UpdateOrderAsync(Guid id, Order order);
        Task<bool> DeleteOrderAsync(Guid id);
        Task<bool> ExistOrderAsync(Guid id);
    }
}
