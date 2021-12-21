using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SearchAPI.Models;
using SearchAPI.Models.Schema;
using SearchAPI.Services;

namespace SearchAPI.Controllers
{
    [ApiController]
    [Route("api/[action]")]
    public class SearchController : Controller
    {
        private readonly IElasticSearchService _searchService;

        public SearchController(IElasticSearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromBody] SearchParams searchParams)
        {
            var response = await _searchService.SearchAsync<RealEstateBase>(searchParams);
            var responseMessage = JsonConvert.SerializeObject(response);
            return new ContentResult
            {
                Content = responseMessage,
                ContentType = "application/json",
                StatusCode = 200
            };
        }
        
        [HttpPost]
        public async Task Index(IndexParams indexParams)
        {
            await _searchService.IndexItemsAsync<RealEstateBase>(indexParams);
        }
    }
}