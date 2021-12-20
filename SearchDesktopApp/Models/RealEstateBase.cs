namespace SearchDesktopApp.Models
{
    public class RealEstateBase
    {
        public string Market { get; set; }
        
        public virtual string FullName
        {
            get { return string.Empty; }
        }

        public virtual string Details
        {
            get { return string.Empty; }
        }
    }
}