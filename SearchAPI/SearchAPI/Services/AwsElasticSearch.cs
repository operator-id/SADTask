using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nest;
using SearchAPI.Models;

namespace SearchAPI.Services
{
    public class AwsElasticSearch : IElasticSearchService
    {
        private readonly IElasticClient _client;
        
        public AwsElasticSearch(IElasticClient client)
        {
            _client = client;
        }
        
        public void CreateIndex<T>(string name)
            where T : class
        {
            var createPropertyIndexResponse = _client.Indices.Create(name,
                x => x
                    .Map<T>(p => p.AutoMap())
            );
        }
        
        public void IndexItems<T>(List<T> items, string index) 
            where T : class
        {
            const int step = 1000;
            for (var i = 0; i < items.Count; i += step)
            {
                _client.IndexMany(items.Skip(i).Take(step), index);
            }
            
        }

        public async Task<IReadOnlyCollection<T>> SearchAsync<T>(string searchPhrase, string market = null, int limit = 25) 
            where T : class
        {
            searchPhrase = string.IsNullOrWhiteSpace(searchPhrase) ? "*" : searchPhrase;

            var boolQuery = CreateQuery(searchPhrase, market);

            var search = await _client.SearchAsync<PropertyModel>(s =>
                s.Index("properties").Query(q => boolQuery).Size(limit));
            var search1 = await _client.SearchAsync<ManagementModel>(s =>
                s.Index("management").Query(q => boolQuery).Size(limit));
            
            Console.WriteLine(search.DebugInformation);
            Console.WriteLine(search.ServerError);
            Console.WriteLine("Found " + search.Hits.Count + " matches");
            var readOnlyCollection = new List<T>((IEnumerable<T>) search.Documents);
            readOnlyCollection.AddRange((IEnumerable<T>) search1.Documents);
            return readOnlyCollection;
        }

        public Task<string> SuggestAsync(string input, string market = null)
        {
            throw new NotImplementedException();
        }

        public Task IndexItemsAsync<T>(List<T> items, string index) where T : class
        {
            throw new NotImplementedException();
        }

        private static BoolQuery CreateQuery(string searchPhrase, string market)
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
            
            if (!string.IsNullOrWhiteSpace(market))
            {
                boolQuery.Filter = new QueryContainer[]
                {
                    new MatchQuery
                    {
                        Query = market,
                        Field = "market"
                    }
                };
            }

            return boolQuery;
        }
    }
}