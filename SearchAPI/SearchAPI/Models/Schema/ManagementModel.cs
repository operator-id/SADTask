using Nest;

namespace SearchAPI.Models.Schema
{
    [ElasticsearchType(IdProperty = "MgmtID")]
    public class ManagementModel : RealEstateBase
    {
        public int MgmtID { get; set; }

    }
}