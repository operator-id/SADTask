namespace ConsoleIndexingApp.Schema
{
    public class ManagementContainer : IContainer<ManagementModel>
    {
        public ManagementModel Mgmt { get; set; }
        
        public ManagementModel GetInnerComponent()
        {
            return Mgmt;
        }
    }
}