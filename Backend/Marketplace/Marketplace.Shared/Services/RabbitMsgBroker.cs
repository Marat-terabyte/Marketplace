using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Marketplace.Shared.Services
{
    internal class RabbitMsgBroker : IMsgBroker
    {
        protected IChannel _channel { get; set; }

        public RabbitMsgBroker(IConnection connection)
        {
            _channel = connection.CreateChannelAsync().Result;
        }

        public async Task SendMessageAsync(string exchange, string routingKey, string message)
        {
            await _channel.BasicPublishAsync(exchange, routingKey, Encoding.UTF8.GetBytes(message));
        }

        public async Task ReceiveMessageAsync(AsyncEventHandler<BasicDeliverEventArgs> handler, string queue, bool autoAck)
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += handler;

            await _channel.BasicConsumeAsync(queue, autoAck, consumer);
        }
    }
}
