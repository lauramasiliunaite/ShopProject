using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TestShop.Infrastructure.Persistence;
using TestShop.Infrastructure.Repositories;

namespace TestShop.Tests
{
    public class ProductRepositoryTests
    {
        [Fact]
        public async Task GetPagedAsync_Returns_Correct_Page_And_Total()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "products-inmemory")
                .Options;

            await using var db = new AppDbContext(options);
            db.Products.AddRange(
                Enumerable.Range(1, 25).Select(i => new TestShop.Domain.Entities.Product
                {
                    Name = $"P{i}",
                    Price = i
                })
            );
            await db.SaveChangesAsync();

            var repo = new ProductRepository(db);

            // Act
            var page2 = await repo.GetPagedAsync(page: 2, pageSize: 10, default);

            // Assert
            page2.Items.Count.Should().Be(10);
            page2.Page.Should().Be(2);
            page2.PageSize.Should().Be(10);
            page2.TotalCount.Should().Be(25);
        }
    }
}