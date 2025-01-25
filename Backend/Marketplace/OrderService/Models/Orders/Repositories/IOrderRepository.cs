using System.Net;

namespace OrderService.Models.Orders.Repositories
{
    public interface IOrderRepository
    {
        Task<Order?> GetOrderAsync(string id);
        Task<List<Order>> GetOrdersByConsumerIdAsync(string consumerId);
        Task<List<Order>> GetOrdersBySellerIdAsync(string sellerId);
        Task<bool> UpdateDeliveryStatus(string sellerId, string orderId, bool status);
    }
}
