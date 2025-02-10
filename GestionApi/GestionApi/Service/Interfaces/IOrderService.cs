using GestionApi.Models;

namespace GestionApi.Service.Interfaces
{
    public interface IOrderService
    {
        Task<bool> AddOrderAsync(Order order);

    }
}
