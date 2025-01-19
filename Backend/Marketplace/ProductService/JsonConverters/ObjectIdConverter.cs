using Newtonsoft.Json;
using MongoDB.Bson;
using System;

namespace ProductService.JsonConverters
{
    public class ObjectIdConverter : JsonConverter<ObjectId>
    {
        public override void WriteJson(JsonWriter writer, ObjectId value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }

        public override ObjectId ReadJson(JsonReader reader, Type objectType, ObjectId existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return ObjectId.Parse((string) objectType.ToString());
        }
    }
}
