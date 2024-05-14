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
            var userId = await _dbContext.Orders
                .GroupBy(order => order.UserId)
                .OrderByDescending(group => group.Count())
                .Select(group => group.Key)
                .FirstOrDefaultAsync();
            var user = await _dbContext.Users
               .FirstOrDefaultAsync(user => user.Id == userId);
            if (user == null) throw new ArgumentNullException(nameof(user));
            return user;
        }

        public async Task<List<User>> GetUsers()
        {
            var users = await _dbContext.Users
                .Where(user => user.Status == UserStatus.Inactive)
                .ToListAsync();
            return users;
        }
    }
}