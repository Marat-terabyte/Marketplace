using CartService.Models;
using CartService.Repositories.CartRepositories;
using MongoDB.Driver;

namespace CartService.Extensions
{
    public static class MongoDbExtensions
    {
        public static void AddMongoDbServices(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddSingleton(_ =>
            {
                MongoClient client = new MongoClient(configuration.GetConnectionString("MongoDB"));
                IMongoDatabase database = client.GetDatabase(configuration["MongoDB:Database"]);

                return database.GetCollection<CartProduct>("cart");
            });

            services.AddScoped<ICartRepository, MongoCartRepository>();
        }
    }
}
