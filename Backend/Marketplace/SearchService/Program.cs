using Marketplace.Shared.Config.Middleware;
using SearchService.Extenstions;
using SearchService.Models.Search;
using SearchService.Services;
using SearchService.Services.Background;
using SearchService.Services.ElasticSearch;
using SearchService.Services.ElasticSearch.MapCreators;

namespace SearchService
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();
            
            builder.Services.AddScoped<IMapCreator[]>(x => [ new RussianMapCreator() ]);

            builder.Services.AddElasticSearch(builder.Configuration);
            builder.Services.AddScoped<ElasticSearchService>();
            builder.Services.AddSingleton<ISearchRepository, ElasticSearchRepository>();
            builder.Services.AddScoped<FindService>();
            
            await builder.Services.AddRabbitMQ(builder.Configuration);
            builder.Services.AddHostedService<IndexingService>();


            var app = builder.Build();
            { 
                // Elasticsearch preparing for working
                using var scope = app.Services.CreateScope();
                var elasticService = scope.ServiceProvider.GetRequiredService<ElasticSearchService>();
                
                await elasticService.CreateIndicesWithMappings();
            }

            if (app.Environment.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            app.UseRouting();
            
            if (app.Environment.IsDevelopment())
                app.UseDevApi();

            app.MapControllers();
            
            app.Run();
        }
    }
}
