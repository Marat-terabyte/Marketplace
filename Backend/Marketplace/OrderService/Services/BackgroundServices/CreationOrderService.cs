using Marketplace.Shared.Models;
using Marketplace.Shared.Services;
using OrderService.Models.Orders;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace OrderService.Services.BackgroundServices
{
    public class CreationOrderService : IHostedService
    {
        private IServiceProvider _serviceProvider;
        private IMsgBroker _msgBroker;

        public CreationOrderService(IMsgBroker msgBroker, IServiceProvider serviceProvider)
        {
            _msgBroker = msgBroker;
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _msgBroker.ReceiveMessageAsync(Consume, "create_order_queue", false);
        }

        public async Task Consume(object ch, BasicDeliverEventArgs events)
        {
            IChannel channel = ((AsyncEventingBasicConsumer) ch).Channel;

            using var scope = _serviceProvider.CreateScope();
            OrderManager orderManager = scope.ServiceProvider.GetRequiredService<OrderManager>();

            string json = Encoding.UTF8.GetString(events.Body.ToArray());
            
            BuyTransactionModel? buyTransaction = JsonSerializer.Deserialize<BuyTransactionModel>(json);
            if (buyTransaction == null)
            {
                await channel.BasicAckAsync(events.DeliveryTag, false);
                
                return;
            }

            try
            {
                await CreateOrdersAsync(buyTransaction, orderManager);
                await channel.BasicAckAsync(events.DeliveryTag, false);
                await SendNotificationAsync(buyTransaction);
            }
            catch
            {
                return;
            }
        }

        private async Task CreateOrdersAsync(BuyTransactionModel buyTransaction, OrderManager orderManager)
        {
            for (int i = 0; i < buyTransaction.ProductIds.Count; i++) // Enumerates all products
            {
                for (int j = 0; j < buyTransaction.Counts[i]; j++) // Adds orders of selected product
                {
                    Order order = new Order()
                    {
                        ProductId = buyTransaction.ProductIds[i],
                        ConsumerId = buyTransaction.ConsumerIds[i],
                        SellerId = buyTransaction.SellerIds[i],
                        Price = buyTransaction.Prices[i],
                        DeliveryPlace = buyTransaction.DeliveryPlace,
                        CreatedAt = DateTime.UtcNow,
                    };

                    await orderManager.AddOrderAsync(order);
                }
            }
        }

        private async Task SendNotificationAsync(BuyTransactionModel buyTransaction)
        {
            Notification notification = new Notification()
            {
                UserId = buyTransaction.ConsumerIds[0],
                Message = "OrderCreated"
            };

            await _msgBroker.SendMessageAsync("", "notification_queue", JsonSerializer.Serialize<Notification>(notification));
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
