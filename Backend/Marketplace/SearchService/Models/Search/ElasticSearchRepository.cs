using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;

namespace SearchService.Models.Search
{
    public class ElasticSearchRepository : ISearchRepository
    {
        private ElasticsearchClient _elastic;

        public ElasticSearchRepository(ElasticsearchClient elastic)
        {
            _elastic = elastic;
        }

        public async Task<bool> AddProductAsync(SearchedProduct product)
        {
            var response = await _elastic.IndexAsync<SearchedProduct>(product, x =>
            {
                x.Index("products");
                x.Id(product.Id);
            });
            
            return response.IsSuccess();
        }

        public async Task<bool> DeleteProductAsync(string productId)
        {
            var response = await _elastic.DeleteAsync<SearchedProduct>("products", productId);

            return response.IsSuccess();
        }

        public async Task<List<SearchedProduct>?> GetPoductsAsync(string query, int from, int to)
        {
            var response = await _elastic.SearchAsync<SearchedProduct>(s =>
                s.Query(q =>
                    q.Bool(b =>
                        b.Should(sh =>
                            sh.MultiMatch(mq =>
                                mq.Fields(new Field[] { new("name"), new("description") }).
                                Operator(Operator.And).
                                Fuzziness(new Fuzziness("auto")).
                                Query(query)
                            )
                        )
                    )
                )
                .From(from)
                .Size(to)
            );

            if (response == null || !response.IsValidResponse)
                return null;

            return response.Documents.ToList();
        }

        public async Task<bool> UpdateProductAsync(string id, SearchedProduct product)
        {
            var request = new UpdateRequest<object, object>("products", id)
            {
                Doc = new
                {
                    Name = product.Name,
                    Description = product.Description,
                }
            };

            var response = await _elastic.UpdateAsync(request);

            return response.IsSuccess();
        }
    }
}
