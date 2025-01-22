using Elastic.Clients.Elasticsearch;
using SearchService.Services.ElasticSearch.MapCreators;

namespace SearchService.Services.ElasticSearch
{
    public class ElasticSearchService
    {
        private IMapCreator[] _mapCreators;
        private ElasticsearchClient _elasticsearch;

        public ElasticSearchService(IMapCreator[] mapCreators, ElasticsearchClient client)
        {
            _mapCreators = mapCreators;
            _elasticsearch = client;
        }

        public async Task CreateIndicesWithMappings()
        {
            foreach (var mapCreator in _mapCreators)
            {
                await _elasticsearch.Indices.CreateAsync(mapCreator.Create());
            }
        }
    }
}
