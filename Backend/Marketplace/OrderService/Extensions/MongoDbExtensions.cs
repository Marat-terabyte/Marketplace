using MongoDB.Driver;
using OrderService.Models.Orders;
using OrderService.Models.Orders.Repositories;
using System.Runtime.CompilerServices;

namespace OrderService.Extensions
{
    public static class MongoDbExtensions
    {
        public static void AddMongoDb(this IServiceCollection service, ConfigurationManager config)
        {
            service.AddSingleton<IMongoCollection<Order>>(provider =>
            {
                MongoClient client = new MongoClient(config.GetConnectionString("MongoDB"));
                IMongoDatabase database = client.GetDatabase(config["MongoDB:Database"]);

                return database.GetCollection<Order>("orders");
            });
        }

        public static void AddMongoRepositories(this IServiceCollection service)
        {
            service.AddScoped<IOrderRepository, MongoOrderRepository>();
        }
    }
}
