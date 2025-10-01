using Microsoft.EntityFrameworkCore;
using TestShop.Domain.Entities;

namespace TestShop.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<CartItem> CartItems => Set<CartItem>();
        public DbSet<Notification> Notifications => Set<Notification>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(u => u.UserName).IsUnique();

            modelBuilder.Entity<CartItem>()
                .HasIndex(ci => new { ci.UserId, ci.ProductId })
                .IsUnique();

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Laptop", Price = 1200, Description = "Lightweight laptop", ImageUrl = "https://picsum.photos/200?1" },
                new Product { Id = 2, Name = "Phone", Price = 800, Description = "Latest smartphone", ImageUrl = "https://picsum.photos/200?2" },
                new Product { Id = 3, Name = "Headphones", Price = 200, Description = "Noise cancelling headphones", ImageUrl = "https://picsum.photos/200?3" },
                new Product { Id = 4, Name = "Monitor", Price = 300, Description = "27-inch 4K monitor", ImageUrl = "https://picsum.photos/200?4" },
                new Product { Id = 5, Name = "Keyboard", Price = 100, Description = "Mechanical keyboard", ImageUrl = "https://picsum.photos/200?5" },
                new Product { Id = 6, Name = "Mouse", Price = 50, Description = "Wireless mouse", ImageUrl = "https://picsum.photos/200?6" },
                new Product { Id = 7, Name = "Tablet", Price = 600, Description = "10-inch Android tablet", ImageUrl = "https://picsum.photos/200?7" },
                new Product { Id = 8, Name = "Smartwatch", Price = 250, Description = "Fitness smartwatch", ImageUrl = "https://picsum.photos/200?8" },
                new Product { Id = 9, Name = "Printer", Price = 150, Description = "All-in-one printer", ImageUrl = "https://picsum.photos/200?9" },
                new Product { Id = 10, Name = "External SSD", Price = 180, Description = "1TB external SSD", ImageUrl = "https://picsum.photos/200?10" }
            );
        }
    }
}