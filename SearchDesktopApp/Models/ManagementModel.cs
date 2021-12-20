namespace SearchDesktopApp.Models
{
    public class ManagementModel : RealEstateBase
    {
        public int MgmtID { get; set; }
        public string Name { get; set; }
        public string State { get; set; }

        public override string FullName
        {
            get
            {
                return Name;
            }
        }

        public override string Details
        {
            get
            {
                return string.Format("{0}, {1}", Market, State);
            }
        }
    }
}