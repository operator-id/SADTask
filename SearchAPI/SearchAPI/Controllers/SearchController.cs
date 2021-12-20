using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nest;
using Newtonsoft.Json;
using SearchAPI.Models;

namespace SearchAPI.Controllers
{
    [ApiController]
    [Route("api")]
    public class SearchController : Controller
    {
        private readonly IElasticClient _client;

        public SearchController(IElasticClient client)
        {
            _client = client;
        }
        
        [HttpGet("[action]/{searchPhrase}/{market?}/{limit:int?}")]
        public async Task<IActionResult> Search(string searchPhrase, string market = null, int limit = 25)
        {
            var boolQuery = new BoolQuery
            {
                Must = new QueryContainer[] { new MultiMatchQuery
                    {
                        Query = searchPhrase,
                        Fields = "*"
                    }
                }
            };

            var search = await _client.SearchAsync<Property>(s =>
                s.Index("properties").Query(q => boolQuery).Size(limit));
            Console.WriteLine(search.DebugInformation);
            Console.WriteLine(search.ServerError);
            Console.WriteLine("Found " + search.Hits.Count + " matches");
            return Ok(search.Documents);
        }
        
        [HttpGet("index-properties")]
        public async Task<IActionResult> IndexProperties()
        {
            // var createIndexResponse = await _client.Indices.CreateAsync("properties",
            //     x => x
            //         .Map<Property>(p => p.AutoMap())
            // );
            // Console.WriteLine(createIndexResponse.DebugInformation);
            // Console.WriteLine(createIndexResponse.ServerError);
            
            // var propertiesJsonString = await File.ReadAllTextAsync("properties.json");
            // var deserializedContainers = JsonConvert.DeserializeObject<List<PropertyContainer>>(propertiesJsonString);
            // Console.WriteLine(deserializedContainers.Aggregate(string.Empty, (a,b) => a +' ' + b.Property.City));
            // if (deserializedContainers == null)
            // {
            //     return new NotFoundResult();
            // }
            // var properties = deserializedContainers.Select(container => container.property);
            // properties = properties.Take(100);
            // var indexResponse = await _client.IndexManyAsync(properties, "properties");
            //
            // Console.WriteLine(indexResponse.DebugInformation);
            // Console.WriteLine(indexResponse.ServerError);
            //
            // var contentResult = new ContentResult
            // {
            //     Content = string.Join("\r\n", "Success")
            // };
            // return contentResult;
            return null;
        }

        [HttpGet("test")]
        public IActionResult Test()
        {
            return new ContentResult {Content = string.Join("\r\n", "Success")};
        }
    }
}