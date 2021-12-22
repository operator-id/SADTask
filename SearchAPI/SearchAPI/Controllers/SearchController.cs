using System;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SearchAPI.Models;
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
            if (searchParams.IndexNames == null || searchParams.IndexNames.Count < 1)
            {
                return new BadRequestResult();
            }
            
            var response = await _searchService.SearchAsync(searchParams);
            if (!response.IsValid)
            {
                return StatusCode(500);
            }
            var responseMessage = JsonConvert.SerializeObject(response.Documents);
            
            return new ContentResult
            {
                Content = responseMessage,
                ContentType = "application/json",
                StatusCode = 200
            };
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] IndexParams indexParams)
        {
            if (string.IsNullOrWhiteSpace(indexParams.IndexName))
            {
                return new BadRequestResult();
            }

            try
            {
                await _searchService.IndexItemsAsync(indexParams);
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

        }
    }
}