using Marketplace.Shared.Models;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using System.Text.Json;

namespace NotifictationService.Services.Cahce
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _memoryCache;
        
        public CacheService(IDistributedCache cache)
        {
            _memoryCache = cache;
        }

        public async Task<List<Notification>> GetNotificationsAsync(string userId)
        {
            string key = ConvertKey(userId);

            byte[]? bytes = await _memoryCache.GetAsync(key);
            if (bytes == null)
                return new List<Notification>();

            string json = Encoding.UTF8.GetString(bytes);
            List<Notification>? notifications = JsonSerializer.Deserialize<List<Notification>>(json);

            await _memoryCache.RemoveAsync(key);

            if (notifications == null) 
                return new List<Notification>();

            return notifications;
        }

        public async Task SetNotificationAsync(Notification notification)
        {
            string key = ConvertKey(notification.UserId);

            List<Notification> notifications = await GetNotificationsAsync(notification.UserId);
            notifications.Add(notification);

            string json = JsonSerializer.Serialize(notifications);
            
            await _memoryCache.SetStringAsync(key, json);
        }

        public async Task SetNotificationsAsync(IEnumerable<Notification> notifications)
        {
            if (notifications.Count() == 0)
                return;

            string key = ConvertKey(notifications.First().UserId);
            
            List<Notification> cachedNotifications = await GetNotificationsAsync(key);
            foreach (var notification in cachedNotifications) 
                cachedNotifications.Add(notification);

            string json = JsonSerializer.Serialize(cachedNotifications);
            
            await _memoryCache.SetStringAsync(key, json);
        }

        private string ConvertKey(string userId) => $"notification:{userId}";
    }
}
