using MongoDB.Driver;
using ProductService.Models.Products;
using ProductService.Models.Products.Repositories;

namespace ProductService.Extensions
{
    public static class MongoDbExtenstions
    {
        public static void AddMongoDbServices(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddSingleton(_ =>
            {
                MongoClient client = new MongoClient(configuration.GetConnectionString("MongoDB"));
                IMongoDatabase database = client.GetDatabase(configuration["MongoDB:Database"]);

                return database.GetCollection<Product>("products");
            });

            services.AddScoped<IProductRepository, MongoProductRepository>();
        }
    }
}
