using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Bson;
using ProductService.Models.Products;
using ProductService.Models.Products.Repositories;
using System.Text.Json;

namespace ProductService.Services
{
    public class ItemService
    {
        private readonly IProductRepository _repository;
        private readonly IDistributedCache _cache;

        public ItemService(IDistributedCache cache, IProductRepository repository)
        {
            _cache = cache;
            _repository = repository;
        }

        public async Task AddItemAsync(Product product)
        {
            await _repository.AddAsync(product);
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

        public async Task<List<Product>> GetBySellerIdAsync(string sellerId)
        {
            return await _repository.GetBySellerIdAsync(sellerId);
        }

        public async Task UpdateAsync(Product updatedProduct)
        {
            await _repository.UpdateAsync(updatedProduct);
            await SetProductToCacheAsync(updatedProduct, expirationHours: 1);
        }

        public async Task DeleteAsync(string id)
        {
            await _repository.DeleteAsync(id);
            await _cache.RemoveAsync(id);
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
