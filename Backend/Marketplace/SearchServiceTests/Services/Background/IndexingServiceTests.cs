using Marketplace.Shared.Models;
using Marketplace.Shared.Services;
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
        private readonly AsyncEventingBasicConsumer _asyncEventingBasicConsumer;
        private readonly Mock<ISearchRepository> _searchRepository;
        private readonly Mock<IMsgBroker> _mockBroker;
        private readonly IndexingService _indexingService;

        public IndexingServiceTests()
        {
            _searchRepository = new Mock<ISearchRepository>();
            _mockBroker = new Mock<IMsgBroker>();
            _indexingService = new IndexingService(_searchRepository.Object, _mockBroker.Object);
            _asyncEventingBasicConsumer = new AsyncEventingBasicConsumer(new Mock<IChannel>().Object);
        }

        [Fact]
        public async Task Consume_AddProduct_ShouldCallAddProduct()
        {            
            // Arrange
            var request = new SearchIndexRequest()
            {
                Id = "1",
                Name = "Product 1",
                Description = "Description of product 1",
                Type = RequestType.Index
            };

            var encodedJson = Encoding.UTF8.GetBytes(
                JsonSerializer.Serialize(request)
            );

            var args = new BasicDeliverEventArgs("", 0, false, "", "", null, encodedJson);

            // Act
            await _indexingService.Consume(_asyncEventingBasicConsumer, args);

            // Assert
            _searchRepository.Verify(x => x.AddProductAsync(It.IsAny<SearchedProduct>()), Times.Once);
        }

        [Fact]
        public async Task Consume_UpdateProduct_ShouldCallUpdateProduct()
        {
            // Arrange
            var request = new SearchIndexRequest()
            {
                Id = "1",
                Name = "Product 1",
                Description = "Description of product 1",
                Type = RequestType.Update
            };

            var encodedJson = Encoding.UTF8.GetBytes(
                JsonSerializer.Serialize(request)
            );

            var args = new BasicDeliverEventArgs("", 0, false, "", "", null, encodedJson);

            // Act
            await _indexingService.Consume(_asyncEventingBasicConsumer, args);

            // Assert
            _searchRepository.Verify(x => x.UpdateProductAsync(request.Id, It.IsAny<SearchedProduct>()), Times.Once);
        }

        [Fact]
        public async Task Consume_DeleteProduct_ShouldCallDeleteProduct()
        {
            // Arrange
            var request = new SearchIndexRequest()
            {
                Id = "1",
                Name = "Product 1",
                Description = "Description of product 1",
                Type = RequestType.Deindex
            };

            var encodedJson = Encoding.UTF8.GetBytes(
                JsonSerializer.Serialize(request)
            );

            var args = new BasicDeliverEventArgs("", 0, false, "", "", null, encodedJson);

            // Act
            await _indexingService.Consume(_asyncEventingBasicConsumer, args);

            // Assert
            _searchRepository.Verify(x => x.DeleteProductAsync(request.Id), Times.Once);
        }
    }
}
