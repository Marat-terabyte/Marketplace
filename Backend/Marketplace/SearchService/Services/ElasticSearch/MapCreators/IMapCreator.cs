using Elastic.Clients.Elasticsearch.IndexManagement;

namespace SearchService.Services.ElasticSearch.MapCreators
{
    public interface IMapCreator
    {
        CreateIndexRequest Create();
    }
}
