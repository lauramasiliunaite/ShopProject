using Microsoft.EntityFrameworkCore;
using TestShop.Application.Interfaces;
using TestShop.Domain.Entities;
using TestShop.Infrastructure.Persistence;

namespace TestShop.Infrastructure.Repositories
{
    public class CartRepository : ICartRepository
    {
            private readonly AppDbContext _dbContext;

            public CartRepository(AppDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<IReadOnlyList<CartItem>> GetUserCartAsync(int userId, CancellationToken cancellationToken)
            {
                return await _dbContext.CartItems
                    .Include(cartItem => cartItem.Product)
                    .Where(cartItem => cartItem.UserId == userId)
                    .ToListAsync(cancellationToken);
            }

            public async Task UpsertAsync(int userId, int productId, int quantity, CancellationToken cancellationToken)
            {
                var existing = await _dbContext.CartItems.FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId, cancellationToken);

                if (quantity <= 0)
                {
                    if (existing != null)
                    {
                        _dbContext.CartItems.Remove(existing);
                        await _dbContext.SaveChangesAsync(cancellationToken);
                    }
                    return;
                }

                if (existing == null)
                {
                    _dbContext.CartItems.Add(new CartItem { UserId = userId, ProductId = productId, Quantity = quantity });
                }
                else
                {
                    existing.Quantity = quantity;
                }

                await _dbContext.SaveChangesAsync(cancellationToken);
            }

            public async Task RemoveAsync(int userId, int productId, CancellationToken cancellationToken)
            {
                var item = await _dbContext.CartItems.FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId, cancellationToken);
                if (item != null)
                {
                    _dbContext.CartItems.Remove(item);
                    await _dbContext.SaveChangesAsync(cancellationToken);
                }
            }
        }
    }
