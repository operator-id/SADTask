using Nest;

namespace SearchAPI.Models.Schema
{
    [ElasticsearchType(IdProperty = "PropertyId")]
    public class PropertyModel : RealEstateBase
    {
        public int PropertyID { get; set; }
        [Text(Analyzer = "snowball", SearchAnalyzer = "snowball")]
        public string FormerName { get; set; }
        [Text(Analyzer = "snowball", SearchAnalyzer = "snowball")]
        public string StreetAddress { get; set; }
        [Text(Analyzer = "snowball", SearchAnalyzer = "snowball")]
        public string City { get; set; }
        public float Lat { get; set; }
        public float Lng { get; set; }
    }
}