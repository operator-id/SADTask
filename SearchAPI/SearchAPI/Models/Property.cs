using Nest;

namespace SearchAPI.Models
{
    public class Property
    {
        public int propertyID { get; set; }
        [Text(Analyzer = "snowball", SearchAnalyzer = "snowball")]
        public string name { get; set; }
        [Text(Analyzer = "snowball", SearchAnalyzer = "snowball")]
        public string formerName { get; set; }
        [Text(Analyzer = "snowball", SearchAnalyzer = "snowball")]
        public string streetAddress { get; set; }
        [Text(Analyzer = "snowball", SearchAnalyzer = "snowball")]
        public string city { get; set; }
        [Text(Analyzer = "snowball", SearchAnalyzer = "snowball")]
        public string market { get; set; }
        [Text(Analyzer = "snowball", SearchAnalyzer = "snowball")]
        public string state { get; set; }
        public float lat { get; set; }
        public float lng { get; set; }
    }
}