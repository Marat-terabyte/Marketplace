namespace ProductService.Extensions
{
    public static class CacheExtensions
    {
        public static void AddCache(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("Redis");
            });
        }
    }
}
