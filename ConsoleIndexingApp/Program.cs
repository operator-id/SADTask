using System;
using System.IO;
using System.Threading.Tasks;
using ConsoleIndexingApp.Schema;

namespace ConsoleIndexingApp
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            StartProgram().GetAwaiter().GetResult();
        }

        private static async Task StartProgram()
        {
            Console.WriteLine("Please enter the api url");
            var url = Console.ReadLine();
            try
            {
                ApiClient.Init(url);
                await TryIndexFileAsync<PropertyContainer, PropertyModel>("properties.json", "property", typeof(PropertyModel).Name);
                await TryIndexFileAsync<ManagementContainer, ManagementModel>("mgmt.json", "management", typeof(ManagementModel).Name);
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static async Task TryIndexFileAsync<TContainer, TDocument>(string filePath, string indexName, string typeName)
            where TContainer : IContainer<TDocument>
            where TDocument : class
        {
            if (!File.Exists(filePath))
            {
                ShowMissingFileWarning(filePath);
                return;
            }
            var result = await ApiClient.IndexFiles<TContainer, TDocument>(filePath, indexName, typeName);
            Console.WriteLine(result);
        }

        private static void ShowMissingFileWarning(string filePath)
        {
            Console.WriteLine("Can't find file {0} in the output folder {1}. Make sure to include it", filePath, Directory.GetCurrentDirectory());
        }

    }
}