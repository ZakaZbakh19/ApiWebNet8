using GestionApi.Data;
using GestionApi.Models;
using GestionApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;

namespace GestionApi.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDBContext _context;

        public OrderRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<bool> AddOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            return await SaveChanges();
        }

        public async Task<bool> DeleteOrderAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                return false;
            }

            var commentModel = await _context.Orders.FindAsync(id);

            if (commentModel == null)
            {
                return false;
            }

            _context.Orders.Remove(commentModel);
            return await SaveChanges();
        }

        public async Task<bool> ExistOrderAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                return false;
            }

            return await _context.Orders.AnyAsync(x => x.Id == id);
        }

        public async Task<Order?> GetOrderAsync(Expression<Func<Order, bool>> predicate)
        {
            return await _context.Orders.FirstOrDefaultAsync(predicate);
        }

        public async Task<Order?> GetOrderByIdAsync(Guid id)
        {
            return await _context.Orders.FindAsync(id);
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync()
        {
            return await _context.Orders.ToListAsync();
        }

        public async Task<bool> UpdateOrderAsync(Guid id, Order order)
        {
            var existingOrder = await _context.Orders.FindAsync(id);

            if (existingOrder == null)
            {
                return false;
            }

            existingOrder.Name = order.Name;
            existingOrder.Description = order.Description;

            return await SaveChanges();
        }

        private async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
