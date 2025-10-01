using Microsoft.EntityFrameworkCore;
using TestShop.Application.DTOs.Common;
using TestShop.Application.Interfaces;
using TestShop.Domain.Entities;
using TestShop.Infrastructure.Persistence;

namespace TestShop.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _dbContext;

        public ProductRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _dbContext.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<PagedResult<Product>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken)
        {
            var query = _dbContext.Products.AsNoTracking().OrderBy(p => p.Id);
            var total = await query.CountAsync(cancellationToken);
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

            return new PagedResult<Product>(items, page, pageSize, total);
        }
    }
}
