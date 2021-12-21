namespace ConsoleIndexingApp.Schema
{
    public interface IContainer<T>
    {
        T GetInnerComponent();
    }
}