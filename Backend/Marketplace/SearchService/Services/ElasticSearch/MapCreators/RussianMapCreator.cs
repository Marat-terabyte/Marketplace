using Elastic.Clients.Elasticsearch.Analysis;
using Elastic.Clients.Elasticsearch.IndexManagement;
using Elastic.Clients.Elasticsearch.Mapping;

namespace SearchService.Services.ElasticSearch.MapCreators
{
    public class RussianMapCreator : IMapCreator
    {
        public CreateIndexRequest Create()
        {
            var index = new CreateIndexRequest("products")
            {
                Settings = new IndexSettings
                {
                    Analysis = new IndexSettingsAnalysis
                    {
                        TokenFilters = new TokenFilters
                        {
                            { "russian_stop", new StopTokenFilter() { Stopwords = ["_russian_"] } },
                            { "russian_stemmer", new StemmerTokenFilter() { Language = "russian" } },
                        },
                        Analyzers = new Analyzers
                        {
                            { "russian_analyzer", new CustomAnalyzer
                                {
                                    Tokenizer = "standard",
                                    Filter = [ "russian_stop", "russian_stemmer", "lowercase" ],
                                }
                            },
                        },
                    }
                },

                Mappings = new TypeMapping()
                {
                    Properties = new Properties
                    {
                        { "name", new TextProperty() { Analyzer = "russian_analyzer" } },
                        { "description", new TextProperty() { Analyzer = "russian_analyzer" }  },
                    },
                },
            };

            return index;
        }
    }
}
