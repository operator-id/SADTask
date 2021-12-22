using Nest;

namespace SearchAPI.Models.Schema
{
    public class RealEstateBase
    {
        public string Market { get; set; }
        [Text(Analyzer = "snowball", SearchAnalyzer = "snowball")]
        public string Name { get; set; }
        [Text(Analyzer = "snowball", SearchAnalyzer = "snowball")]
        public string State { get; set; }
        
        public string TypeName
        {
            get
            {
                return GetType().Name;
            }
        }
    }
}