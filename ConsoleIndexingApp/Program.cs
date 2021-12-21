using System;
using System.Threading.Tasks;
using ConsoleIndexingApp.Schema;

namespace ConsoleIndexingApp
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            ReadInput().GetAwaiter().GetResult();
        }

        private static async Task ReadInput()
        {
            //var url = GetUrlInput();
            var url = "https://localhost:7299";
            Console.WriteLine("Sending a request to " + url);
            ApiClient.Init(url);

            Console.WriteLine("Indexing properties.json...");
            var result = await ApiClient.IndexFiles<PropertyContainer, PropertyModel>("properties.json", "properties", typeof(PropertyModel).Name);
            Console.WriteLine(result);
            Console.WriteLine("Indexing mgmt.json...");
            result = await ApiClient.IndexFiles<ManagementContainer, ManagementModel>("mgmt.json", "properties", typeof(ManagementModel).Name);
            Console.WriteLine(result);
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }


        private static string GetUrlInput()
        {
            Console.WriteLine("Please enter the url of the API:\n");
            return Console.ReadLine();
        }

    }
}