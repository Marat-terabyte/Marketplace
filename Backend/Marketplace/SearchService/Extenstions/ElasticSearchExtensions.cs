using Elastic.Clients.Elasticsearch;

namespace SearchService.Extenstions
{
    public static class ElasticSearchExtensions
    {
        public static void AddElasticSearch(this IServiceCollection services, ConfigurationManager config)
        {
            Uri uri = new Uri(config.GetConnectionString("ElasticSearch") ?? throw new ArgumentNullException("ElasticSearch connection string is empty"));

            var settings = new ElasticsearchClientSettings(uri).DefaultIndex("products");

            services.AddSingleton<ElasticsearchClient>(s => {
                var elastic = new ElasticsearchClient(uri);
                
                return elastic;
            });
        }
    }
}
