using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TestShop.Domain.Entities;

namespace TestShop.Infrastructure.Persistence
{
    public static class SeedData
    {
        public static async Task EnsureSeedAsync(IServiceProvider services)
        {
            var db = services.GetRequiredService<AppDbContext>();
            await db.Database.MigrateAsync();

            if (!db.Users.Any())
            {
                var hasher = services.GetRequiredService<IPasswordHasher<User>>();
                var demoUser = new User
                {
                    UserName = "demo",
                    Email = "demo@test.com"
                };
                demoUser.PasswordHash = hasher.HashPassword(demoUser, "Pass123!");
                db.Users.Add(demoUser);

                await db.SaveChangesAsync();
            }
        }
    }
}
