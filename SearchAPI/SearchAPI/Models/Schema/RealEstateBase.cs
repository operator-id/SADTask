using Nest;

namespace SearchAPI.Models.Schema
{
    public class RealEstateBase
    {
        [Text(Analyzer = "snowball", SearchAnalyzer = "snowball")]
        public string Market { get; set; }
        public string Name { get; set; }
        public string State { get; set; }
    }
}