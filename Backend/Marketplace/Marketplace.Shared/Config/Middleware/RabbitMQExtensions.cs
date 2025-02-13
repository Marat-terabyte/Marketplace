using Marketplace.Shared.Services;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace Marketplace.Shared.Config.Middleware
{
    public static class RabbitMQExtensions
    {
        public static async Task AddRabbitMQ(this IServiceCollection services, ConfigurationManager config)
        {
            IConnection? amqpConnection = null;
            while (amqpConnection == null)
            {
                try
                {
                    amqpConnection = await new ConnectionFactory()
                    {
                        HostName = config.GetConnectionString("RabbitMQ") ?? throw new ArgumentNullException("'RabbitMQ' connection string is empty"),
                        UserName = config["RabbitMQ:Username"] ?? throw new ArgumentNullException("'RabbitMQ:Username' string is empty"),
                        Password = config["RabbitMQ:Password"] ?? throw new ArgumentNullException("'RabbitMQ:Password' string is empty")
                    }.CreateConnectionAsync();
                }
                catch
                {
                    await Task.Delay(TimeSpan.FromMinutes(2));
                }
            }

            // TODO: Replace 
            // Creating queues
            IChannel channel = await amqpConnection.CreateChannelAsync();
            await channel.QueueDeclareAsync("search_indexing_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
            
            await channel.QueueDeclareAsync("decrease_balance_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
            await channel.QueueDeclareAsync("decrease_balance_compensate", durable: false, exclusive: false, autoDelete: false, arguments: null);
            
            await channel.QueueDeclareAsync("decrease_count_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
            await channel.QueueDeclareAsync("decrease_count_compensate", durable: false, exclusive: false, autoDelete: false, arguments: null);

            await channel.QueueDeclareAsync("create_order_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
            await channel.QueueDeclareAsync("create_order_compensate", durable: false, exclusive: false, autoDelete: false, arguments: null);

            await channel.QueueDeclareAsync("notification_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);

            services.AddSingleton(amqpConnection);
            services.AddSingleton<IMsgBroker, RabbitMsgBroker>();
        }
    }
}
