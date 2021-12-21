namespace ConsoleIndexingApp.Schema
{
    public class PropertyContainer : IContainer<PropertyModel>
    {
        public PropertyModel Property { get; set; }
        
        public PropertyModel GetInnerComponent()
        {
            return Property;
        }
    }
}