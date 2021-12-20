using System.Collections.Generic;
using System.Threading.Tasks;
using SearchAPI.Models;

namespace SearchAPI.Services
{
    public interface IElasticSearchService
    {
        Task<IReadOnlyCollection<T>> SearchAsync<T>(string searchPhrase, string market = null, int limit = 25) 
            where T : class;

        Task<string> SuggestAsync(string input, string market = null);
        
        Task IndexItemsAsync<T> (List<T> items, string index)
            where T : class;
    }
}