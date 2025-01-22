using Marketplace.Shared.Models;
using Moq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SearchService.Models.Search;
using SearchService.Services.Background;
using System.Text;
using System.Text.Json;

namespace SearchServiceTests.Services.Background
{
    public class IndexingServiceTests
    {
        private Mock<IChannel> _channel;
        private Mock<ISearchRepository> _searchRepository;
        private IndexingService _indexingService;

        public IndexingServiceTests()
        {
            _channel = new Mock<IChannel>();
            _searchRepository = new Mock<ISearchRepository>();
            _indexingService = new IndexingService(_channel.Object, _searchRepository.Object);
        }

        [Fact]
        public async Task Consume_AddProduct_ShouldCallAddProduct()
        {
            // Arrange
            var request = new SearchServiceRequest()
            {
                Id = "1",
                Name = "Product 1",
                Description = "Description of product 1",
                Method = TypeMethod.Add
            };

            var encodedJson = Encoding.UTF8.GetBytes(
                JsonSerializer.Serialize(request)
            );

            var args = new BasicDeliverEventArgs("", 0, false, "", "", null, encodedJson);

            // Act

            await _indexingService.Consume(null, args);

            // Assert
            _searchRepository.Verify(x => x.AddProductAsync(It.IsAny<SearchedProduct>()), Times.Once);
        }

        [Fact]
        public async Task Consume_UpdateProduct_ShouldCallUpdateProduct()
        {
            // Arrange
            var request = new SearchServiceRequest()
            {
                Id = "1",
                Name = "Product 1",
                Description = "Description of product 1",
                Method = TypeMethod.Update
            };

            var encodedJson = Encoding.UTF8.GetBytes(
                JsonSerializer.Serialize(request)
            );

            var args = new BasicDeliverEventArgs("", 0, false, "", "", null, encodedJson);

            // Act

            await _indexingService.Consume(null, args);

            // Assert
            _searchRepository.Verify(x => x.UpdateProductAsync(request.Id, It.IsAny<SearchedProduct>()), Times.Once);
        }

        [Fact]
        public async Task Consume_DeleteProduct_ShouldCallDeleteProduct()
        {
            // Arrange
            var request = new SearchServiceRequest()
            {
                Id = "1",
                Name = "Product 1",
                Description = "Description of product 1",
                Method = TypeMethod.Delete
            };

            var encodedJson = Encoding.UTF8.GetBytes(
                JsonSerializer.Serialize(request)
            );

            var args = new BasicDeliverEventArgs("", 0, false, "", "", null, encodedJson);

            // Act

            await _indexingService.Consume(null, args);

            // Assert
            _searchRepository.Verify(x => x.DeleteProductAsync(request.Id), Times.Once);
        }
    }
}
