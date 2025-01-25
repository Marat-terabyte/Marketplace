using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OrderService.Models.Orders
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public ObjectId Id { get; set; }
        public string ConsumerId { get; set; }
        public string SellerId { get; set; }
        public string ProductId { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public string DeliveryPlace { get; set; }
        public bool IsDelivered { get; set; }
        public DateTime? DeliveredAt { get; set; }
    }
}
