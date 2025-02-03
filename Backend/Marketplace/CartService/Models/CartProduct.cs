using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CartService.Models
{
    public class CartProduct
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public ObjectId Id { get; set; }
        public string ConsumerId { get; set; }
        public string SellerId { get; set; }
        public string ProductId {  get; set; }
        public int Count {  get; set; }
        public decimal PriceOfProduct { get; set; }
        public DateTime CreatedAt { get; set; }
        [BsonIgnore]
        public decimal Amount { get => PriceOfProduct * Count; }
    }
}
