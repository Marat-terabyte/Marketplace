using Marketplace.Shared.Models;
using Marketplace.Shared.Services;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using ProductService.Models.Products.Repositories;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace ProductService.Services.BackgroundServices
{
    public class ProductCountRestorer : IHostedService
    {
        private IServiceProvider _serviceProvider;
        private IMsgBroker _msgBroker;

        public ProductCountRestorer(IMsgBroker msgBroker, IServiceProvider serviceProvider)
        {
            _msgBroker = msgBroker;
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _msgBroker.ReceiveMessageAsync(Consume, "create_order_compensate", false);
        }

        public async Task Consume(object ch, BasicDeliverEventArgs events)
        {
            IChannel channel = ((AsyncEventingBasicConsumer) ch).Channel;

            using var scope = _serviceProvider.CreateScope();
            ProductStockService stockService = _serviceProvider.GetRequiredService<ProductStockService>();

            string json = Encoding.UTF8.GetString(events.Body.ToArray());
            
            CompensBuyTrans? compensation = JsonSerializer.Deserialize<CompensBuyTrans>(json);
            if (compensation == null)
                return;

            bool res = await stockService.RestoreStockAsync(compensation);
            if (res)
            {
                await _msgBroker.SendMessageAsync("", "decrease_balance_compensate", json);
                await channel.BasicAckAsync(events.DeliveryTag, false);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
