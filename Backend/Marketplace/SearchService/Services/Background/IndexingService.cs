using Marketplace.Shared.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SearchService.Models.Search;
using System.Text;
using System.Text.Json;
using Marketplace.Shared.Services;

namespace SearchService.Services.Background
{
    public class IndexingService : IHostedService
    {
        private ISearchRepository _searchRepository;
        private IMsgBroker _msgBroker;

        public IndexingService(ISearchRepository searchRepository, IMsgBroker broker)
        {
            _searchRepository = searchRepository;
            _msgBroker = broker;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _msgBroker.ReceiveMessageAsync(Consume, "search_indexing_queue", false);
        }

        public async Task Consume(object ch, BasicDeliverEventArgs events)
        {
            string json = Encoding.UTF8.GetString(events.Body.ToArray());
            var request = JsonSerializer.Deserialize<SearchIndexRequest>(json);
            if (request == null)
                return;

            if (request.Type == RequestType.Index)
            {
                SearchedProduct product = new SearchedProduct()
                {
                    Id = request.Id,
                    Name = request.Name,
                    Description = request.Description,
                };

                await _searchRepository.AddProductAsync(product);
            }
            else if (request.Type == RequestType.Deindex)
            {
                await _searchRepository.DeleteProductAsync(request.Id);
            }
            else if (request.Type == RequestType.Update)
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

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
