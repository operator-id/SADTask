using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SearchAPI.Models;
using SearchAPI.Services;

namespace SearchAPI.Controllers
{
    [ApiController]
    [Route("api")]
    public class SearchController : Controller
    {
        private readonly IElasticSearchService _searchService;

        public SearchController(IElasticSearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpGet("[action]/{searchPhrase}/{market?}/{limit:int?}")]
        public async Task<IActionResult> Search(string searchPhrase, string market = null, int limit = 25)
        {
            var response = await _searchService.SearchAsync<RealEstateBase>(searchPhrase, market, limit);
            var responseMessage = JsonConvert.SerializeObject(response);
            return new ContentResult
            {
                Content = responseMessage,
                ContentType = "application/json",
                StatusCode = 200
            };
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