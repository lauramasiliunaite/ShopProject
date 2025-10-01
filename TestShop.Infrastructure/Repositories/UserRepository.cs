using Microsoft.EntityFrameworkCore;
using TestShop.Application.Interfaces;
using TestShop.Domain.Entities;
using TestShop.Infrastructure.Persistence;

namespace TestShop.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _dbContext;

        public UserRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == username, cancellationToken);
        }

        public async Task<User> AddAsync(User user, CancellationToken cancellationToken)
        {
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return user;
        }
    }
}
