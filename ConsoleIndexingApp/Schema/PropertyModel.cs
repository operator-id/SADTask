
namespace ConsoleIndexingApp.Schema
{

    public class PropertyModel : RealEstateBase
    {
        public int PropertyID { get; set; }
        public string FormerName { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public float Lat { get; set; }
        public float Lng { get; set; }
    }
}