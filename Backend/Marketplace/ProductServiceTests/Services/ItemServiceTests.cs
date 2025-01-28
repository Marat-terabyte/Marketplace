using Marketplace.Shared.Services;
using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Bson;
using Moq;
using ProductService.Models.Products;
using ProductService.Models.Products.Repositories;
using ProductService.Services;

namespace ProductServiceTests.Services
{
    public class ItemServiceTests
    {
        private readonly Mock<IProductRepository> _mockRepository;
        private readonly Mock<IDistributedCache> _mockCache;
        private readonly Mock<IMsgBroker> _mockBroker;
        private readonly ItemService _itemService;

        public ItemServiceTests()
        {
            _mockRepository = new Mock<IProductRepository>();
            _mockCache = new Mock<IDistributedCache>();
            _mockBroker = new Mock<IMsgBroker>();
            _itemService = new ItemService(_mockCache.Object, _mockRepository.Object, _mockBroker.Object);
        }

        [Fact]
        public async Task AddItem_ShouldCallAddAsync()
        {
            // Arrange
            var product = new Product()
            {
                Id = ObjectId.Parse("678a8143bb134e21f331fe07"),
                SellerId = "1",
                Name = "Product1",
                Description = "SomeDescription",
                CreatedAt = DateTime.UtcNow,
                Category = "Products",
                Price = 10,
                Stock = 10
            };

            // Act
            await _itemService.AddItemAsync(product);

            // Assert
            _mockRepository.Verify(r => r.AddAsync(product), Times.Once());
            _mockBroker.Verify(b => b.SendMessageAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
        }

        [Fact]
        public async Task GetById_ShouldGetProductFromRepository_WhenNotInCache()
        {
            // Arrange
            string id = "678a8143bb134e21f331fe07";
            var product = new Product()
            {
                Id = ObjectId.Parse(id),
                SellerId = "1",
                Name = "Product1",
                Description = "SomeDescription",
                CreatedAt = DateTime.UtcNow,
                Category = "Products",
                Price = 10,
                Stock = 10
            };

            _mockCache.Setup(c => c.GetAsync(It.IsAny<string>(), CancellationToken.None)).ReturnsAsync((byte[]?) null);
            _mockRepository.Setup(r => r.GetAsync(id)).ReturnsAsync(product);

            // Act
            var res = await _itemService.GetByIdAsync(id);

            // Assert
            Assert.NotNull(res);
            Assert.Equal(product.Id, res.Id);
            _mockRepository.Verify(r => r.GetAsync(id), Times.Once());
            _mockCache.Verify(c => c.GetAsync(It.IsAny<string>(), CancellationToken.None));
        }

        [Fact]
        public async Task GetBySellerId_ShouldCallGetBySellerId()
        {
            // Arrange
            string id = "678a8143bb134e21f331fe07";
            var product = new Product()
            {
                Id = ObjectId.Parse(id),
                SellerId = "1",
                Name = "Product1",
                Description = "SomeDescription",
                CreatedAt = DateTime.UtcNow,
                Category = "Products",
                Price = 10,
                Stock = 10,
                Images = ["https://url"],
            };

            _mockRepository.Setup(r => r.GetBySellerIdAsync("1")).ReturnsAsync([product]);

            // Act
            var res = await _itemService.GetBySellerIdAsync(product.SellerId);

            //Assert
            Assert.NotNull(res);
            Assert.Equal(product, product);
            _mockRepository.Verify(r => r.GetBySellerIdAsync("1"), Times.Once());
        }
        
        [Fact]
        public async Task UpdateAsync_ShouldUpdateProductAndCache()
        {
            // Arrange
            string id = "678a8143bb134e21f331fe07";
            var updatedProduct = new Product { Id = ObjectId.Parse(id), Name = "UpdatedProduct" };

            _mockRepository.Setup(repo => repo.UpdateAsync(updatedProduct)).Returns(Task.CompletedTask);

            // Act
            await _itemService.UpdateAsync(updatedProduct);

            // Assert
            _mockRepository.Verify(repo => repo.UpdateAsync(updatedProduct), Times.Once);
            _mockBroker.Verify(b => b.SendMessageAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveProductAndCache()
        {
            // Arrange
            string id = "678a8143bb134e21f331fe07";
            _mockRepository.Setup(repo => repo.DeleteAsync(id)).Returns(Task.CompletedTask);

            // Act
            await _itemService.DeleteAsync(id);

            // Assert
            _mockRepository.Verify(repo => repo.DeleteAsync(id), Times.Once);
            _mockCache.Verify(cache => cache.RemoveAsync(id, CancellationToken.None), Times.Once);
            _mockBroker.Verify(b => b.SendMessageAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
        }
    }
}
