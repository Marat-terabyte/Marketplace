using Microsoft.AspNetCore.Mvc;
using SearchService.Models.Search;

namespace SearchService.Extenstions
{
    public static class DevelopmentApiExtensions
    {
        public static void UseDevApi(this WebApplication app)
        {
            app.MapPost("/api/search", async (HttpContext context, ISearchRepository repos, SearchedProduct product) =>
            {
                await repos.AddProductAsync(product);
            });

            app.MapPut("/api/search", async (HttpContext context, ISearchRepository repos, SearchedProduct product) =>
            {
                await repos.UpdateProductAsync(product.Id, product);
            });


            app.MapDelete("/api/search", async (HttpContext context, [FromServices] ISearchRepository repos, [FromBody] SearchedProduct product) =>
            {
                await repos.DeleteProductAsync(product.Id);
            });
        }
    }
}
