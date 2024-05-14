using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _dbContext;

        public OrderService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Order> GetOrder()
        {
            var order = await _dbContext.Orders
                .OrderByDescending(order => order.CreatedAt)
                .Where(order => order.Quantity > 1)
                .FirstOrDefaultAsync();
            if (order == null) throw new ArgumentNullException(nameof(order));
            return order;
        }

        public async Task<List<Order>> GetOrders()
        {
            var orders = await _dbContext.Orders
                .Where(order => order.User.Status == Enums.UserStatus.Active)
                .OrderByDescending(order => order.CreatedAt)
                .ToListAsync();
            return orders;
        }
    }
}
