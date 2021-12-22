using System.Collections.Generic;

namespace SearchDesktopApp.Models
{
    public class SearchParams
    {
        public string SearchPhrase { get; set; }

        public List<string> Market { get; set; }

        public int Limit { get; set; }
        
        public List<string> IndexNames { get; set; }

        public int MinSearchCharacters { get; set; }

        public SearchParams(string searchPhrase, string market, int minSearchCharacters)
        {
            SearchPhrase = searchPhrase;
            Market = market == null ? null : new List<string> {market};
            MinSearchCharacters = minSearchCharacters;
            IndexNames = new List<string> {"property", "management"};
        }
    }
}