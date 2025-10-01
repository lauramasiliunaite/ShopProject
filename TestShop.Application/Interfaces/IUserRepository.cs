using TestShop.Domain.Entities;

namespace TestShop.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByUsernameAsync(string userName, CancellationToken cancellationToken);
        Task<User> AddAsync(User user, CancellationToken cancellationToken);
    }
}
