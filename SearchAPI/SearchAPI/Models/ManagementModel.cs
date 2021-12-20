namespace SearchAPI.Models
{
    public class ManagementModel : RealEstateBase
    {
        public int MgmtID { get; set; }
        public string Name { get; set; }
        public string Market { get; set; }
        public string State { get; set; }
    }
}