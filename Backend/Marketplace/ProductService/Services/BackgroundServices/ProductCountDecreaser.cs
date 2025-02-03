using Marketplace.Shared.Models;
using Marketplace.Shared.Services;
using ProductService.Models.Products.Repositories;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace ProductService.Services.BackgroundServices
{
    public class ProductCountDecreaser : IHostedService
    {
        private IServiceProvider _serviceProvider;
        private IMsgBroker _msgBroker;

        public ProductCountDecreaser(IMsgBroker msgBroker, IServiceProvider serviceProvider)
        {
            _msgBroker = msgBroker;
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _msgBroker.ReceiveMessageAsync(Consume, "decrease_count_queue", true);
        }

        public async Task Consume(object ch, BasicDeliverEventArgs events)
        {
            using var scope = _serviceProvider.CreateScope();
            IProductRepository productRepository = scope.ServiceProvider.GetRequiredService<IProductRepository>();
            ProductStockService stockService = scope.ServiceProvider.GetRequiredService<ProductStockService>();

            string json = Encoding.UTF8.GetString(events.Body.ToArray());

            BuyTransactionModel? transactionModel = JsonSerializer.Deserialize<BuyTransactionModel>(json);
            if (transactionModel == null)
                return;

            bool res = await stockService.ModifyStockAsync(transactionModel);
            if (!res)
            {
                CompensBuyTrans compensation = new CompensBuyTrans(transactionModel);
                compensation.ErrorMessage = "ProductServiceError";

                await _msgBroker.SendMessageAsync("", "decrease_count_compensate", JsonSerializer.Serialize(compensation));

                return;
            }

            await _msgBroker.SendMessageAsync("", "create_order_queue", json);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
