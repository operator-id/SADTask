using System;
using System.Collections.Generic;
using System.Linq;
using Nest;

namespace SearchAPI.Services
{
    public static class ElasticSearchHelper
    {
        public static Func<CreateIndexDescriptor, ICreateIndexRequest> CreateIndexingSelector(Type documentType)
        {
            return selector => selector
                .Map(mappingDescriptor => mappingDescriptor.AutoMap(documentType))
                .Settings(settingsDescriptor => settingsDescriptor.Analysis(analysisDescriptor =>
                    analysisDescriptor.Analyzers(analyzersDescriptor =>
                        analyzersDescriptor.Snowball("snowball", snowballAnalyzerDescriptor =>
                            snowballAnalyzerDescriptor.Language(SnowballLanguage.English)
                                .StopWords("_english_")))));
        }

        public static BoolQuery CreateQuery(string searchPhrase, IReadOnlyCollection<string> markets)
        {
            var boolQuery = new BoolQuery();
            var must = new List<QueryContainer>
            {
                new MultiMatchQuery
                {
                    Query = searchPhrase,
                    Fuzziness = Fuzziness.Auto
                }
            };

            if (markets != null && markets.Count > 0)
            {
                var subQueries = markets
                    .Where(market => !string.IsNullOrWhiteSpace(market))
                    .Select(market => (QueryContainer) new MatchQuery
                    {
                        Query = market,
                        Field = "market", 
                        IsStrict = true
                    });

                must.AddRange(subQueries);
            }

            boolQuery.Must = must;
            return boolQuery;
        }
    }
}