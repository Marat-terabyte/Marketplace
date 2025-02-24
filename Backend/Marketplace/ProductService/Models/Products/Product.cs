using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ProductService.JsonConverters;
using System.Text.Json.Serialization;

namespace ProductService.Models.Products
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public ObjectId Id { get; set; }

        public string SellerId { get; set; }
        public string Storename {  get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt {  get; set; }
        public string[] Images { get; set; }
        public Dictionary<string, string>? Attributes { get; set; }
    }
}
