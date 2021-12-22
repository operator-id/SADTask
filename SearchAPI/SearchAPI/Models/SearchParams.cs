using System.Collections.Generic;

namespace SearchAPI.Models
{
    public class SearchParams
    {
        private string _searchPhrase;
        private int _limit;

        public string SearchPhrase
        {
            get
            {
                return string.IsNullOrEmpty(_searchPhrase) ? "*" : _searchPhrase;
            }
            set { _searchPhrase = value; }
        }

        public List<string> Market { get; set; }
        public List<string> IndexNames { get; set; }

        public int Limit
        {
            get
            {
                return _limit <= 0 ? 25 : _limit;
            }
            set { _limit = value; }
        }
        
        public int MinSearchCharacters { get; set; }
    }
}