using Marketplace.Shared.Models;
using Marketplace.Shared.Services;
using PaymentService.Models.Repositories;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace PaymentService.Services.BackgroundServices
{
    public class BalanceWithdrawService : IHostedService
    {
        private IServiceProvider _serviceProvider;
        private IMsgBroker _msgBroker;

        public BalanceWithdrawService(IMsgBroker msgBroker,  IServiceProvider serviceProvider)
        {
            _msgBroker = msgBroker;
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _msgBroker.ReceiveMessageAsync(Consume, "decrease_balance_queue", true);
        }

        public async Task Consume(object ch, BasicDeliverEventArgs events)
        {
            using var scope = _serviceProvider.CreateScope();
            IBalanceRepository balanceRepository = scope.ServiceProvider.GetRequiredService<IBalanceRepository>();

            string json = Encoding.UTF8.GetString(events.Body.ToArray());
            
            BuyTransactionModel? transactionModel = JsonSerializer.Deserialize<BuyTransactionModel>(json);
            if (transactionModel == null)
                return;

            var result = await balanceRepository.ProcessPaymentAsync(transactionModel);

            bool isSuccess = result.Item1;
            if (!isSuccess)
            {
                CompensBuyTrans compensation = new(transactionModel);
                compensation.ErrorMessage = result.Item2!;

                await _msgBroker.SendMessageAsync("", "decrease_balance_compensate", JsonSerializer.Serialize(compensation));
                return;
            }

            await _msgBroker.SendMessageAsync("", "decrease_count_queue", json);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
