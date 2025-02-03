using Marketplace.Shared.Models;
using Marketplace.Shared.Services;
using PaymentService.Models.Repositories;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace PaymentService.Services.BackgroundServices
{
    public class RestoreBalanceService : IHostedService
    {
        private IServiceProvider _serviceProvider;
        private IMsgBroker _msgBroker;

        public RestoreBalanceService(IMsgBroker msgBroker, IServiceProvider serviceProvider)
        {
            _msgBroker = msgBroker;
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _msgBroker.ReceiveMessageAsync(Consume, "decrease_count_compensate", false);
        }

        public async Task Consume(object ch, BasicDeliverEventArgs events)
        {
            using var scope = _serviceProvider.CreateScope();
            IBalanceRepository balanceRepository = scope.ServiceProvider.GetRequiredService<IBalanceRepository>();

            string json = Encoding.UTF8.GetString(events.Body.ToArray());
            CompensBuyTrans? compensation = JsonSerializer.Deserialize<CompensBuyTrans>(json);
            if (compensation == null)
                return;
            
            bool isSuccess = await balanceRepository.RestorePaymentProcessAsync(compensation);
            if (isSuccess)
            {
                await _msgBroker.SendMessageAsync("", "decrease_balance_compensate", json);
                await ((AsyncEventingBasicConsumer)ch).Channel.BasicAckAsync(events.DeliveryTag, false);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
