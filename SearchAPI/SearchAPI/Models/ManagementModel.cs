using Nest;

namespace SearchAPI.Models
{
    [ElasticsearchType(IdProperty = "MgmtID")]
    public class ManagementModel : RealEstateBase
    {
        public int MgmtID { get; set; }
    }
}