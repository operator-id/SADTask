using System.Collections.Generic;
using System.Linq;
using Nest;

namespace SearchAPI.Services
{
    public class AwsElasticSearch
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
    }
}