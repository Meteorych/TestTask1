using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Enums;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _dbContext;

        public UserService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> GetUser()
        { 
            // Calculate total order value for each user and order by descending
            var userId = await _dbContext.Orders
                .Where(order => order.CreatedAt.Year == 2003 && order.Status == OrderStatus.Delivered)
                .GroupBy(order => order.UserId)
                .Select(group => new
                {
                    UserId = group.Key,
                    TotalValue = group.Sum(order => order.Quantity * order.Price)
                })
                .OrderByDescending(group => group.TotalValue)
                .Select(group => group.UserId)
                .FirstOrDefaultAsync();

            // Fetch the user based on the userId
            var user = await _dbContext.Users
                .FirstOrDefaultAsync(user => user.Id == userId);

            // Handle potential null value
            if (user == null) throw new ArgumentNullException(nameof(user));

            return user;   
        }

        public async Task<List<User>> GetUsers()
        {
            var users = await _dbContext.Users
                .Where(user => user.Orders.Any(order => order.Status == OrderStatus.Paid && order.CreatedAt.Year == 2010))
                .ToListAsync();
            return users;
        }
    }
}