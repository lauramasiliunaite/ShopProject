using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TestShop.Application.Interfaces;
using TestShop.Domain.Entities;
using TestShop.Infrastructure.Persistence;
using TestShop.Infrastructure.Repositories;
using TestShop.Infrastructure.Services;

namespace TestShop.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices( this IServiceCollection services, 
            IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICartRepository, CartRepository>();

            services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<INotificationService, NotificationService>();

            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

            return services;
        }
    }
}
