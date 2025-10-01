namespace TestShop.Application.DTOs.Notifications
{
    public record NotificationDto(int Id, string Type, string Status, DateTime CreatedAt, DateTime? ProcessedAt);
}
