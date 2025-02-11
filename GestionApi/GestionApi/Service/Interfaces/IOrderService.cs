using GestionApi.Dtos;
using GestionApi.Exceptions;
using GestionApi.Models;
using System.Linq.Expressions;

namespace GestionApi.Service.Interfaces
{
    public interface IOrderService
    {
        Task<ResultModel<OrderDto>> AddOrderAsync(OrderDto order);
        Task<ResultModel<IEnumerable<OrderDto>>> GetOrdersAsync();
        Task<ResultModel<OrderDto>> GetOrderByIdAsync(Guid id);
        Task<ResultModel<OrderDto>> GetOrderAsync(Expression<Func<OrderDto, bool>> predicate);
        Task<ResultModel<OrderDto>> UpdateOrderAsync(Guid id, Order order);
        Task<ResultModel<bool>> DeleteOrderAsync(Guid id);
        Task<ResultModel<bool>> ExistOrderAsync(Guid id);
    }
}
