using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SearchAPI.Models;
using SearchAPI.Models.Schema;

namespace SearchAPI.Services
{
    public interface IElasticSearchService
    {
        Task<IReadOnlyCollection<T>> SearchAsync<T>(SearchParams searchParams) 
            where T : class;

        Task<string> SuggestAsync(SearchParams searchParams);
        
        Task IndexItemsAsync<T> (IndexParams indexParams)
            where T : RealEstateBase;
    }
}