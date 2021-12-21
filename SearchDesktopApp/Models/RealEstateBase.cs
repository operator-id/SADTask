namespace SearchDesktopApp.Models
{
    public class RealEstateBase
    {
        private string _market;

        public string Market
        {
            get { return string.Format("Market: {0}", _market); }
            set { _market = value; }
        }

        public string Name { get; set; }
        public string State { get; set; }
        public virtual string FullName
        {
            get { return Name; }
        }

        public virtual string Details
        {
            get
            {
                return State;
            }
        }
    }
}