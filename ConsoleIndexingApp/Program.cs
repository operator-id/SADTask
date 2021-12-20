using System;
using System.Net.Http;
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
            var url = GetUrlInput();
            Console.WriteLine("Sending a request to " + url);
            var result = await GetPage(url);
            Console.WriteLine(result);
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
            // while (true)
            // {
            //
            //     break;
            // }
        }

        private static async Task<string> GetPage(string url)
        {
            var client = new HttpClient();
            System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => true;
            client.DefaultRequestHeaders.Accept.Clear();
            client.BaseAddress = new Uri(url + "/test");
            Console.WriteLine(client.BaseAddress);
            var response = await client.GetAsync(client.BaseAddress);
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            return response.ToString();
        }

        private static string GetUrlInput()
        {
            Console.WriteLine("Please enter the url of the API:\n");
            return Console.ReadLine();
        }

    }
}