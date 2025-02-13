using GestionApi.Dtos;
using GestionApi.Exceptions;
using GestionApi.Models;
using System.Linq.Expressions;

namespace GestionApi.Service.Interfaces
{
    public interface IOrderService
    {
        Task<ResultModel<OrderDto>> AddOrderAsync(CreateOrderDto orderDto);
        Task<ResultModel<IEnumerable<OrderDto>>> GetOrdersAsync();
        Task<ResultModel<OrderDto>> GetOrderByIdAsync(Guid id);
        Task<ResultModel<OrderDto>> GetOrderAsync(OrderQuery query = null);
        Task<ResultModel<OrderDto>> UpdateOrderAsync(OrderDto orderDto);
        Task<ResultModel<bool>> DeleteOrderAsync(OrderQuery query = null);
        Task<bool> ExistOrderAsync(OrderQuery query = null);
    }
}
