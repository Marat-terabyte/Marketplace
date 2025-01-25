using MongoDB.Bson;
using MongoDB.Driver;

namespace OrderService.Models.Orders.Repositories
{
    public class MongoOrderRepository : IOrderRepository
    {
        private readonly IMongoCollection<Order> _collection;

        public MongoOrderRepository(IMongoCollection<Order> collection)
        {
            _collection = collection;
        }

        public async Task<Order?> GetOrderAsync(string id)
        {
            return await _collection.Find(o => o.Id == ObjectId.Parse(id)).FirstOrDefaultAsync();
        }

        public async Task<List<Order>> GetOrdersByConsumerIdAsync(string consumerId)
        {
            return await _collection.Find(o => o.ConsumerId == consumerId).ToListAsync();
        }

        public async Task<List<Order>> GetOrdersBySellerIdAsync(string sellerId)
        {
            return await _collection.Find(o => o.SellerId == sellerId).ToListAsync();
        }

        public async Task<bool> UpdateDeliveryStatus(string sellerId, string orderId, bool status)
        {
            var o = new Order();
            var filter = new BsonDocument
            {
                { "_id", ObjectId.Parse(orderId) },
                { nameof(o.SellerId), sellerId }
            };

            BsonDocument updateSettings;
            if (status)
                updateSettings = new BsonDocument("$set", new BsonDocument { { nameof(o.IsDelivered), status }, { nameof(o.DeliveredAt), DateTime.Now }});
            else
                updateSettings = new BsonDocument("$set", new BsonDocument { { nameof(o.IsDelivered), status }, { nameof(o.DeliveredAt), BsonNull.Value } });

            var result = await _collection.UpdateOneAsync(filter, updateSettings);

            return result.ModifiedCount == 0 ? false : true;
        }
    }
}
