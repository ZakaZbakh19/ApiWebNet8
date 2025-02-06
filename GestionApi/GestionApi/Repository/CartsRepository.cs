using GestionApi.Data;
using GestionApi.Models;
using GestionApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;

namespace GestionApi.Repository
{
    public class CartsRepository : ICartsRepository
    {
        private readonly ApplicationDBContext _context;

        public CartsRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<bool> AddCartAsync(Carts cart)
        {
            await _context.Carts.AddAsync(cart);
            return await SaveChanges();
        }

        public async Task<bool> DeleteCartAsync(int id)
        {
            var commentModel = await _context.Carts.FirstOrDefaultAsync(x => x.Id == id);

            if (commentModel == null)
            {
                return false;
            }

            _context.Carts.Remove(commentModel);
            return await SaveChanges();
        }

        public async Task<bool> ExistCartAsync(int id)
        {
            return await _context.Carts.AnyAsync(x => x.Id == id);
        }

        public async Task<Carts?> GetCartAsync(Expression<Func<Carts, bool>> predicate)
        {
            return await _context.Carts.FirstOrDefaultAsync(predicate);
        }

        public async Task<Carts?> GetCartByIdAsync(int id)
        {
            return await _context.Carts.FindAsync(id);
        }

        public async Task<IEnumerable<Carts>> GetCartsAsync()
        {
            return await _context.Carts.ToListAsync();
        }

        public async Task<bool> UpdateCartAsync(int id, Carts cart)
        {
            var existingCart = await _context.Carts.FindAsync(id);

            if (existingCart == null)
            {
                return false;
            }

            existingCart.Name = cart.Name;
            existingCart.Description = cart.Description;

            return await SaveChanges();
        }

        private async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
