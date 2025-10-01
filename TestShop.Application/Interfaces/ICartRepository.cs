using TestShop.Domain.Entities;

namespace TestShop.Application.Interfaces
{
    public interface ICartRepository
    {
        Task<IReadOnlyList<CartItem>> GetUserCartAsync(int userId, CancellationToken cancellationToken);
        Task UpsertAsync(int userId, int productId, int quantity, CancellationToken cancellationToken);
        Task RemoveAsync(int userId, int productId, CancellationToken cancellationToken);
    }
}
