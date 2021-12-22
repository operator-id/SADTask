using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nest;
using SearchAPI.Models;
using SearchAPI.Models.Schema;

namespace SearchAPI.Services
{
    public class AwsElasticSearch : IElasticSearchService
    {
        private readonly IElasticClient _client;

        
        public AwsElasticSearch(IElasticClient client)
        {
            _client = client;
        }
        
        public async Task IndexItemsAsync(IndexParams indexParams)
        {
            var index = indexParams.IndexName;
            await _client.Indices.DeleteAsync(indexParams.IndexName);
            var existsResponse = await _client.Indices.ExistsAsync(index);
            var documentType = TypeHelper.GetTypeFromName(indexParams.ModelType);
            if (!existsResponse.Exists)
            {
                await _client.Indices.CreateAsync(index, ElasticSearchHelper.CreateIndexingSelector(documentType));
            }
            
            var items = await JsonParsingHelper.ParseFromTextAsync(indexParams);
            if (existsResponse.Exists)
            {
                await _client.DeleteManyAsync(items, indexParams.IndexName);
            }
            await _client.IndexManyAsync(items, indexParams.IndexName);
        }

        public async Task<IReadOnlyCollection<dynamic>> SearchAsync(SearchParams searchParams)
        {
            var boolQuery = ElasticSearchHelper.CreateQuery(searchParams.SearchPhrase, searchParams.Market);
            var search = await _client.SearchAsync<dynamic>(selector =>
                selector.Index(searchParams.IndexNames.ToArray())
                    .Query(query => boolQuery)
                    .Size(searchParams.Limit)
                    .Sort(sort => sort.Descending(SortSpecialField.Score))
            );
            
            return search.Documents;
        }
    }
}