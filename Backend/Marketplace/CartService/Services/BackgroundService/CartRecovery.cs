using Marketplace.Shared.Models;
using Marketplace.Shared.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace CartService.Services.BackgroundService
{
    public class CartRecovery : IHostedService
    {
        private IMsgBroker _msgBroker;

        public CartRecovery(IMsgBroker msgBroker)
        {
            _msgBroker = msgBroker;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _msgBroker.ReceiveMessageAsync(Consume, "decrease_balance_compensate", false);
        }

        public async Task Consume(object ch, BasicDeliverEventArgs events)
        {
            IChannel channel = ((AsyncEventingBasicConsumer)ch).Channel;

            string json = Encoding.UTF8.GetString(events.Body.ToArray());
            
            CompensBuyTrans? compensation = JsonSerializer.Deserialize<CompensBuyTrans>(json);
            if (compensation == null)
                return;

            Notification notification = new Notification()
            {
                UserId = compensation.ConsumerIds[0],
                Message = compensation.ErrorMessage,
            };

            try
            {
                await _msgBroker.SendMessageAsync("", "notification_queue", JsonSerializer.Serialize(notification));
                await channel.BasicAckAsync(events.DeliveryTag, false);
            }
            catch
            {

            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
