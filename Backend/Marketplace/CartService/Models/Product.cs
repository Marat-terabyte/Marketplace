using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ProductService.JsonConverters;
using System.Text.Json.Serialization;

namespace CartService.Models
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public ObjectId Id { get; set; }

        [JsonPropertyName("sellerid")]
        public string SellerId { get; set; }
        
        [JsonPropertyName("name")]
        public string Name { get; set; }
        
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
        
        [JsonPropertyName("stock")]
        public int Stock { get; set; }

        [JsonPropertyName("createdat")]
        public DateTime CreatedAt { get; set; }
        
        [JsonPropertyName("updatedat")]
        public DateTime? UpdatedAt {  get; set; }
    }
}
