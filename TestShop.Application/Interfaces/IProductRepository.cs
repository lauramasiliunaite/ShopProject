using TestShop.Application.DTOs.Common;
using TestShop.Domain.Entities;

namespace TestShop.Application.Interfaces
{
    public interface IProductRepository
    {
        Task<PagedResult<Product>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken);
        Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken);
    }
}
