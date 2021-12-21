namespace SearchDesktopApp.Models
{
    public class PropertyModel : RealEstateBase
    {
        public int PropertyID { get; set; }
        public string FormerName { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        
        public float Lat { get; set; }
        public float Lng { get; set; }

        public override string FullName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(FormerName))
                {
                    return Name;
                }

                return string.Format("{0} (formerly {1})", Name, FormerName);
            }
        }

        public override string Details
        {
            get
            {
                return string.Format("{0}, {1}, {2}", StreetAddress, City, State);
            }
        }

        public override string ToString()
        {
            return FullName;
        }
    }
}