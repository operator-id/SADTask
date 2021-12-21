using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ConsoleIndexingApp
{
    public class ApiClient
    {
        private static HttpClient _httpClient;

        public static void Init(string baseUrl)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(baseUrl);
            //System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => true;
        }

        public static async Task<string> Test()
        {
            var uri = _httpClient.BaseAddress + "api/test";
            Console.WriteLine(uri);
            var response = await _httpClient.GetAsync(uri);
            return response.ToString();
        }

        public static async Task<string> IndexFiles(string filePath, string indexName)
        {
            using (var fs = File.OpenRead(filePath))
            using (var streamContent = new StreamContent(fs))
            {
                var fileContent = await streamContent.ReadAsStringAsync();
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("multipart/form-data"));
                var indexParams = new IndexParams
                {
                    IndexName = indexName,
                    JsonString = fileContent
                };
                
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(_httpClient.BaseAddress + "api/index"),
                    Content = new StringContent(JsonConvert.SerializeObject(indexParams), Encoding.UTF8, MediaTypeNames.Application.Json)
                };
                
                var response = await _httpClient.SendAsync(request);
                return response.StatusCode + " " + response.ReasonPhrase;
            }

        }
    }
}