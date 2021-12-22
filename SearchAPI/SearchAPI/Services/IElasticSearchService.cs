using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Nest;
using SearchAPI.Models;
using SearchAPI.Models.Schema;

namespace SearchAPI.Services
{
    public interface IElasticSearchService
    {
        Task<ISearchResponse<dynamic>> SearchAsync(SearchParams searchParams);
        Task IndexItemsAsync(IndexParams indexParams);
    }
}