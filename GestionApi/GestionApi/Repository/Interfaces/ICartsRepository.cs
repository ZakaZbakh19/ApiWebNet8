using GestionApi.Models;

namespace GestionApi.Repository.Interfaces
{
    public interface ICartsRepository
    {
        Task<IEnumerable<Carts>> GetCartsAsync();
        Task<Carts> GetCartByIdAsync(int id);
        Task<Carts> GetCartAsync(Func<Carts, bool> predicate);
        Task<Carts> AddCartAsync(Carts cart);
        Task<Carts> UpdateCartAsync(Carts cart);
        Task<Carts?> DeleteCartAsync(int id);
        Task<bool> ExistCartAsync(int id);
    }
}
