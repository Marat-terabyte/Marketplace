using CartService.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Servers;

namespace CartService.Repositories.CartRepositories
{
    public class MongoCartRepository : ICartRepository
    {
        private readonly IMongoCollection<CartProduct> _products;

        public MongoCartRepository(IMongoCollection<CartProduct> products)
        {
            _products = products;
        }

        public async Task AddCartProductAsync(CartProduct product)
        {
            await _products.InsertOneAsync(product);
        }

        public async Task<CartProduct?> GetProductAsync(string id)
        {
            return await _products.Find(p => p.Id == ObjectId.Parse(id)).FirstOrDefaultAsync();
        }

        public async Task<CartProduct?> GetProductByProductIdAsync(string productId, string userId)
        {
            return await _products.Find(p => p.ProductId == productId && p.ConsumerId == userId).FirstOrDefaultAsync();
        }

        public async Task<List<CartProduct>> GetUserCartProductsAsyn(string userId)
        {
            return await _products.Find(p => p.ConsumerId == userId).ToListAsync();
        }

        public async Task<bool> RemoveCartProductAsync(string id)
        {
            var response = await _products.DeleteOneAsync(p => p.Id == ObjectId.Parse(id));
            
            return response.DeletedCount != 0;
        }

        public async Task<bool> UpdateCountCartProductAsync(string productId, int count)
        {
            var c = new CartProduct();
            var filter = new BsonDocument
            {
                { "_id", productId }
            };

            BsonDocument updateSettings = new BsonDocument("$set", new BsonDocument { { nameof(c.Count), count } });

            var result = await _products.UpdateOneAsync(filter, updateSettings);

            return result.ModifiedCount == 0 ? false : true;
        }
    }
}
