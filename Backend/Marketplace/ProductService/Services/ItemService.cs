using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Bson;
using ProductService.Models.Products;
using ProductService.Models.Products.Repositories;
using System.Text.Json;
using RabbitMQ.Client;
using Marketplace.Shared.Models;
using System.Text;
using Marketplace.Shared.Services;

namespace ProductService.Services
{
    public class ItemService
    {
        private readonly IProductRepository _repository;
        private readonly IDistributedCache _cache;
        private readonly IMsgBroker _msgBroker;

        public ItemService(IDistributedCache cache, IProductRepository repository, IMsgBroker broker)
        {
            _cache = cache;
            _repository = repository;
            _msgBroker = broker;
        }

        public async Task AddItemAsync(Product product)
        {
            await _repository.AddAsync(product);
            await IndexProduct(product);
        }

        /// <summary>
        /// Publishes a product to a message broker for deindexing in the search service
        /// </summary>
        private async Task IndexProduct(Product product)
        {
            SearchIndexRequest request = new SearchIndexRequest()
            {
                Id = product.Id.ToString(),
                Name = product.Name,
                Description = product.Description,
                Type = RequestType.Index
            };

            await _msgBroker.SendMessageAsync(exchange: "", routingKey: "search_indexing_queue", JsonSerializer.Serialize(request));
        }

        public async Task<Product?> GetByIdAsync(string id)
        {
            Product? product = null;

            byte[]? data = await _cache.GetAsync(id);
            if (data == null)
            {
                product = await _repository.GetAsync(id);
                if (product == null)
                    return null;
                else
                    await SetProductToCacheAsync(product, expirationHours: 2);
            }
            else
            {
                product = JsonSerializer.Deserialize<Product>(data);
                product!.Id = ObjectId.Parse(id);
            }

            return product;
        }

        public async Task<List<Product>> GetAllProductsAsync(int from, int to)
        {
            List<Product> products = await _repository.GetProducts(from, to);

            return products;
        }

        public async Task<List<Product>> GetBySellerIdAsync(string sellerId, int from, int to)
        {
            return await _repository.GetBySellerIdAsync(sellerId, from, to);
        }

        public async Task UpdateAsync(Product updatedProduct)
        {
            await _repository.UpdateAsync(updatedProduct);
            await SetProductToCacheAsync(updatedProduct, expirationHours: 1);
            await UpdateIndexProduct(updatedProduct);
        }

        private async Task UpdateIndexProduct(Product updatedProduct)
        {
            SearchIndexRequest request = new SearchIndexRequest()
            {
                Id = updatedProduct.Id.ToString(),
                Name = updatedProduct.Name,
                Description = updatedProduct.Description,
                Type = RequestType.Update
            };

            await _msgBroker.SendMessageAsync(exchange: "", routingKey: "search_indexing_queue", JsonSerializer.Serialize(request));
        }

        public async Task DeleteAsync(string id)
        {
            await _repository.DeleteAsync(id);
            await _cache.RemoveAsync(id);
            await DeindexProduct(id);
        }

        /// <summary>
        /// Publishes a product to a message broker for deindexing in the search service
        /// </summary>
        private async Task DeindexProduct(string id)
        {
            SearchIndexRequest request = new SearchIndexRequest()
            {
                Id = id,
                Type = RequestType.Deindex
            };

            await _msgBroker.SendMessageAsync(exchange: "", routingKey: "search_indexing_queue", JsonSerializer.Serialize(request));
        }

        private async Task SetProductToCacheAsync(Product product, int expirationHours)
        {
            string id = product.Id.ToString();
            string serializedProduct = JsonSerializer.Serialize(product);
            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
            {
                AbsoluteExpiration = DateTime.Now.AddHours(expirationHours)
            };

            await _cache.SetStringAsync(id, serializedProduct, options);
        }
    }
}
