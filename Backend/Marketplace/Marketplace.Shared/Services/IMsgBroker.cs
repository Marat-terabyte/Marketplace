using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Shared.Services
{
    public interface IMsgBroker
    {
        Task SendMessageAsync(string exchange, string routingKey, string message);
        Task ReceiveMessageAsync(AsyncEventHandler<BasicDeliverEventArgs> handler, string queue, bool autoAck);
    }
}
