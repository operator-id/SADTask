using Nest;

namespace SearchAPI.Models
{
    public class PropertyModel : RealEstateBase
    {
        public int PropertyID { get; set; }
        [Text(Analyzer = "snowball", SearchAnalyzer = "snowball")]
        public string Name { get; set; }
        [Text(Analyzer = "snowball", SearchAnalyzer = "snowball")]
        public string FormerName { get; set; }
        [Text(Analyzer = "snowball", SearchAnalyzer = "snowball")]
        public string StreetAddress { get; set; }
        [Text(Analyzer = "snowball", SearchAnalyzer = "snowball")]
        public string City { get; set; }
        [Text(Analyzer = "snowball", SearchAnalyzer = "snowball")]
        public string State { get; set; }
        public float Lat { get; set; }
        public float Lng { get; set; }
    }
}