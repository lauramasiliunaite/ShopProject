using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TestShop.Infrastructure.Persistence;

namespace TestShop.Tests.Utils
{
    public class CustomWebApplicationFactory : WebApplicationFactory<TestShop.Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
                if (descriptor is not null)
                    services.Remove(descriptor);

                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseInMemoryDatabase("ApiTestsDb");
                });

                using var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.EnsureCreated();

                if (!db.Products.Any())
                {
                    db.Products.AddRange(
                        Enumerable.Range(1, 10).Select(i =>
                            new TestShop.Domain.Entities.Product { Name = $"Prod{i}", Price = i * 10 })
                    );
                    db.SaveChanges();
                }
            });
        }
    }
}
