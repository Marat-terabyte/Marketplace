namespace SearchService.Models.Search
{
    public interface ISearchRepository
    {
        public Task<List<SearchedProduct>?> GetPoductsAsync(string query);
        public Task<bool> AddProductAsync(SearchedProduct product);
        public Task<bool> UpdateProductAsync(string id, SearchedProduct product);
        public Task<bool> DeleteProductAsync(string productId);
    }
}
