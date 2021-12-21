using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Nest;
using Newtonsoft.Json;
using SearchAPI.Models;

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

        public async Task<IReadOnlyCollection<T>> SearchAsync<T>(SearchParams searchParams) 
            where T : class
        {
            var boolQuery = CreateQuery(searchParams.SearchPhrase, searchParams.Market);

            var search = await _client.SearchAsync<PropertyModel>(s =>
                s.Index("properties").Query(q => boolQuery).Size(searchParams.Limit));
            
            Console.WriteLine(search.DebugInformation);

            Console.WriteLine("Found " + search.Hits.Count + " matches");
            return new List<T>((IEnumerable<T>) search.Documents);
        }

        public Task<string> SuggestAsync(SearchParams searchParams)
        {
            throw new NotImplementedException();
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
                        .Map<PropertyModel>(p => p.AutoMap<PropertyModel>())
                );
            }
            
            {
                if (existsResponse.Exists)
                {
                    Console.WriteLine("Deleting the existing documents for the index {0}...", index);
                    await ExecuteBulkOperationAsync<T>(index, indexParams.JsonString, DeleteManyExtensions.DeleteManyAsync);
                }
                await ExecuteBulkOperationAsync<T>(index, indexParams.JsonString, IndexManyExtensions.IndexManyAsync);
            }
        }

        private async Task ExecuteBulkOperationAsync<T>(string indexName, string text, BulkOperationAsync<PropertyModel> bulkOperationTask) 
            where T : RealEstateBase
        {
            const int step = 2000;
            var serializer = new JsonSerializer();
            using (var stringReader = new StringReader(text))
            using (var jsonReader = new JsonTextReader(stringReader))
            {
                var items = new List<PropertyModel>();
                
                for (; await jsonReader.ReadAsync();)
                {
                    //Console.WriteLine(jsonReader.Value);
                    if (jsonReader.TokenType != JsonToken.StartObject)
                    {
                        continue;
                    }

                    var item = serializer.Deserialize<PropertyContainer>(jsonReader);
                    items.Add(item.Property);
                    if (items.Count < step)
                    {
                        continue;
                    }
                    
                    var result = await bulkOperationTask(_client,  items, indexName);
                    //_client.DeleteManyAsync()
                    Console.WriteLine(result.DebugInformation);
                    items.Clear();
                }

                if (items.Count > 0)
                {
                    await bulkOperationTask(_client,  items, indexName);
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
            
            if (markets != null && markets.Count > 0)
            {
                boolQuery.Filter = new QueryContainer[]
                {
                    new MatchQuery
                    {
                        Query = markets[0],//todo
                        Field = "market"
                    }
                };
            }

            return boolQuery;
        }
    }
}