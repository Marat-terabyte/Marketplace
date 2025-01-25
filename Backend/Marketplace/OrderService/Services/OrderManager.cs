using OrderService.Models.Orders;
using OrderService.Models.Orders.Repositories;

namespace OrderService.Services
{
    public class OrderManager
    {
        private readonly IOrderRepository _repository;

        public OrderManager(IOrderRepository repository)
        {
            _repository = repository;
        }

        public async Task<Order?> GetOrderAsync(string id)
        {
            return await _repository.GetOrderAsync(id);
        }

        public async Task<List<Order>> GetOrdersByConsumer(string consumerId)
        {
            return await _repository.GetOrdersByConsumerIdAsync(consumerId);
        }

        public async Task<List<Order>> GetOrdersBySeller(string sellerId)
        {
            return await _repository.GetOrdersBySellerIdAsync(sellerId);
        }

        public async Task<bool> UpdateDeliveryStatus(string sellerId, string orderId, bool status)
        {
            return await _repository.UpdateDeliveryStatus(sellerId, orderId, status);
        }
    }
}
