using Microsoft.EntityFrameworkCore;
using TestShop.Application.DTOs.Notifications;
using TestShop.Application.Interfaces;
using TestShop.Domain.Entities;
using TestShop.Infrastructure.Persistence;

namespace TestShop.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly AppDbContext _dbContext;

        public NotificationService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> EnqueueLoginNotificationAsync(int userId, CancellationToken cancellationToken)
        {
            var notification = new Notification
            {
                UserId = userId,
                Type = "Login",
                Status = "Queued",
                CreatedAt = DateTime.UtcNow
            };

            _dbContext.Notifications.Add(notification);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return notification.Id;
        }

        public async Task<NotificationDto?> GetLatestForUserAsync(int userId, CancellationToken cancellationToken)
        {
            var n = await _dbContext.Notifications
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefaultAsync(cancellationToken);

            return n is null ? null : new NotificationDto(n.Id, n.Type, n.Status, n.CreatedAt, n.ProcessedAt);
        }

        public async Task ProcessPendingAsync(CancellationToken cancellationToken)
        {
            var queued = await _dbContext.Notifications.Where(n => n.Status == "Queued").ToListAsync(cancellationToken);
            foreach (var n in queued)
            {
                n.Status = "Done";
                n.ProcessedAt = DateTime.UtcNow;
            }
            if (queued.Count > 0)
                await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
