using SearchService.Models.Search;

namespace SearchService.Services
{
    public class FindService
    {
        private ISearchRepository _search;

        public FindService(ISearchRepository search)
        {
            _search = search;
        }

        public async Task<List<SearchedProduct>?> SearchProducts(string query, int from, int to)
        {
            return await _search.GetPoductsAsync(query, from, to);
        }
    }
}
