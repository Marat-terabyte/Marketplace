using Elastic.Clients.Elasticsearch;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SearchService.Extenstions;
using SearchService.Models.Search;
using SearchService.Services;
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
            builder.Services.AddScoped<ISearchRepository, ElasticSearchRepository>();
            builder.Services.AddScoped<FindService>();

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
