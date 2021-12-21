using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Nest;
using Newtonsoft.Json;
using SearchAPI.Models;
using SearchAPI.Models.Schema;

namespace SearchAPI.Services
{
    public class AwsElasticSearch : IElasticSearchService
    {
        private readonly IElasticClient _client;
        private delegate Task<BulkResponse> BulkOperationAsync<in T>(IElasticClient client, IEnumerable<T> objects, 
            IndexName index = null, CancellationToken cancellationToken = default(CancellationToken));
        
        public AwsElasticSearch(IElasticClient client)
        {
            _client = client;
        }
        
        public async Task IndexItemsAsync<T>(IndexParams indexParams)
            where T : RealEstateBase
        {
            var index = indexParams.IndexName;
            var existsResponse = await _client.Indices.ExistsAsync(index);
            if (!existsResponse.Exists)
            {
                await _client.Indices.CreateAsync(index,
                    x => x
                        .Map<T>(p => p.AutoMap<T>())
                );
            }

            if (existsResponse.Exists)
            {
                Console.WriteLine("Deleting the existing documents for the index {0}...", index);
                await ExecuteBulkOperationAsync<T>(indexParams, DeleteManyExtensions.DeleteManyAsync);
            }

            await ExecuteBulkOperationAsync<T>(indexParams, IndexManyExtensions.IndexManyAsync);
        }

        public async Task<IReadOnlyCollection<T>> SearchAsync<T>(SearchParams searchParams) 
            where T : class
        {
            var boolQuery = CreateQuery(searchParams.SearchPhrase, searchParams.Market);
            var search = await _client.SearchAsync<T>(s =>
                s.Index("properties")
                    .Query(q => boolQuery)
                    .Size(searchParams.Limit)
            );
            
            Console.WriteLine(search.DebugInformation);

            Console.WriteLine("Found " + search.Hits.Count + " matches");
            return search.Documents;
        }

        public Task<string> SuggestAsync(SearchParams searchParams)
        {
            throw new NotImplementedException();
        }

        private async Task ExecuteBulkOperationAsync<T>(IndexParams indexParams, BulkOperationAsync<T> bulkOperationTask) 
            where T : RealEstateBase
        {
            const int step = 5000;
            var serializer = new JsonSerializer();
            using (var stringReader = new StringReader(indexParams.JsonString))
            using (var jsonReader = new JsonTextReader(stringReader))
            {
                var items = new List<T>();
                
                for (; await jsonReader.ReadAsync();)
                {
                    if (jsonReader.TokenType != JsonToken.StartObject)
                    {
                        continue;
                    }

                    var type = TypeHelper.GetTypeFromName(indexParams.ModelType);
                    if (type == null)
                    {
                        continue;
                    }
                    var item = (T) serializer.Deserialize(jsonReader, type);
                    items.Add(item);
                    if (items.Count < step)
                    {
                        continue;
                    }
                    
                    var result = await bulkOperationTask(_client,  items, indexParams.IndexName);
                    Console.WriteLine(result.DebugInformation);
                    items.Clear();
                }

                if (items.Count > 0)
                {
                    await bulkOperationTask(_client,  items, indexParams.IndexName);
                }
            }
        }

        private static BoolQuery CreateQuery(string searchPhrase, List<string> markets)
        {
            var boolQuery = new BoolQuery
            {
                Must = new QueryContainer[]
                {
                    new MultiMatchQuery
                    {
                        Query = searchPhrase,
                        Fields = "*"
                    }
                }
            };
            
            if (markets != null && markets.Count > 0 && !string.IsNullOrWhiteSpace(markets[0]))
            {
                var market = markets[0];
                boolQuery.Filter = new QueryContainer[]
                {
                    new MatchQuery
                    {
                        Query = market,//todo
                        Field = "market",
                        IsVerbatim = true
                    }
                };
            }

            
            return boolQuery;
        }
    }
}