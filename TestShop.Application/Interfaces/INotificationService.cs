using TestShop.Application.DTOs.Notifications;

namespace TestShop.Application.Interfaces
{
    public interface INotificationService
    {
        Task<int> EnqueueLoginNotificationAsync(int userId, CancellationToken cancellationToken);
        Task<NotificationDto?> GetLatestForUserAsync(int userId, CancellationToken cancellationToken);
        Task ProcessPendingAsync(CancellationToken cancellationToken);
    }
}
