using Marketplace.Shared.Models;

namespace NotifictationService.Services.Cahce
{
    public interface ICacheService
    {
        Task SetNotificationAsync(Notification notification);
        Task<List<Notification>> GetNotificationsAsync(string userId);
        Task SetNotificationsAsync(IEnumerable<Notification> notifications);
    }
}
