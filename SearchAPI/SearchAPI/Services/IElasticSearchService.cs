using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SearchAPI.Models;
using SearchAPI.Models.Schema;

namespace SearchAPI.Services
{
    public interface IElasticSearchService
    {
        Task<IReadOnlyCollection<dynamic>> SearchAsync(SearchParams searchParams);
        Task<string> SuggestAsync(SearchParams searchParams);
        
        Task IndexItemsAsync<T> (IndexParams indexParams)
            where T : RealEstateBase;
    }
}