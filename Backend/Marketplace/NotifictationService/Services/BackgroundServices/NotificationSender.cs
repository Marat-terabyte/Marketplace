using Marketplace.Shared.Models;
using Marketplace.Shared.Services;
using Microsoft.AspNetCore.SignalR;
using NotifictationService.Hubs;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace NotifictationService.Services.BackgroundServices
{
    public class NotificationSender : IHostedService
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IMsgBroker _msgBroker;

        public NotificationSender(IHubContext<NotificationHub> hubContext, IMsgBroker msgBroker)
        {
            _hubContext = hubContext;
            _msgBroker = msgBroker;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _msgBroker.ReceiveMessageAsync(Consume, "notification_queue", false);
        }

        public async Task Consume(object ch, BasicDeliverEventArgs events)
        {
            IChannel channel = ((AsyncEventingBasicConsumer) ch).Channel;

            string json = Encoding.UTF8.GetString(events.Body.ToArray());

            Notification? notification = JsonSerializer.Deserialize<Notification>(json);
            if (notification == null)
            {
                await channel.BasicAckAsync(events.DeliveryTag, false);

                return;
            }

            try
            {
                IClientProxy clientProxy = _hubContext.Clients.Group(notification.UserId);
                await clientProxy.SendAsync("Receive", json);
                
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
