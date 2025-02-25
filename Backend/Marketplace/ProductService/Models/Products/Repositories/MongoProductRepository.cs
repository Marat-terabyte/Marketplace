using MongoDB.Bson;
using MongoDB.Driver;

namespace ProductService.Models.Products.Repositories
{
    public class MongoProductRepository : IProductRepository
    {
        private readonly IMongoCollection<Product> _collection;

        public MongoProductRepository(IMongoCollection<Product> collection)
        {
            _collection = collection;
        }

        public async Task AddAsync(Product product)
        {
            await _collection.InsertOneAsync(product);

        }

        public async Task DeleteAsync(string id)
        {
            await _collection.DeleteOneAsync(p => p.Id == ObjectId.Parse(id));
        }

        public async Task<Product?> GetAsync(string id)
        {
            return await _collection.Find(p => p.Id == ObjectId.Parse(id)).FirstOrDefaultAsync();
        }

        public async Task<List<Product>> GetBySellerIdAsync(string sellerId, int from, int to)
        {
            return await _collection.Find(p => p.SellerId == sellerId)
                .Skip(from)
                .Limit(to - from)
                .ToListAsync();
        }

        public async Task<List<Product>> GetProducts(int from, int to)
        {
            return await _collection.Find(Builders<Product>.Filter.Empty)
                .Skip(from)
                .Limit(to - from)
                .ToListAsync();
        }

        public async Task UpdateAsync(Product updatedProduct)
        {
            await _collection.ReplaceOneAsync(p => p.Id == updatedProduct.Id, updatedProduct);
        }
    }
}
