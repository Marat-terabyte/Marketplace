using Marketplace.Shared.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SearchService.Models.Search;
using System.Text;
using System.Text.Json;

namespace SearchService.Services.Background
{
    public class IndexingService : IHostedService
    {
        private IChannel _channel;
        private ISearchRepository _searchRepository;

        public IndexingService(IChannel channel, ISearchRepository searchRepository)
        {
            _channel = channel;
            _searchRepository = searchRepository;
        }

        public async Task Consume(object ch, BasicDeliverEventArgs events)
        {
            string json =  Encoding.UTF8.GetString(events.Body.ToArray());
            var request = JsonSerializer.Deserialize<SearchServiceRequest>(json);
            if (request == null)
                return;

            if (request.Method == TypeMethod.Add)
            {
                SearchedProduct product = new SearchedProduct()
                {
                    Id = request.Id,
                    Name = request.Name,
                    Description = request.Description,
                };

                await _searchRepository.AddProductAsync(product);
            }
            else if (request.Method == TypeMethod.Delete)
            {
                 await _searchRepository.DeleteProductAsync(request.Id);
            }
            else if (request.Method == TypeMethod.Update)
            {
                SearchedProduct product = new SearchedProduct()
                {
                    Id = request.Id,
                    Name = request.Name,
                    Description = request.Description
                };

                await _searchRepository.UpdateProductAsync(request.Id, product);
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += Consume;

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
