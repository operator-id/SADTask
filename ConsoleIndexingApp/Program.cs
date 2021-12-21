using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

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
            var result = await ApiClient.Test();

            Console.WriteLine(result);
            Console.WriteLine("Indexing properties...");
            //var path = Console.ReadLine();
            result = await ApiClient.IndexFiles("properties.json", "properties");
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