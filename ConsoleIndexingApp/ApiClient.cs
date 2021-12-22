using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using ConsoleIndexingApp.Schema;
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
        }

        public static async Task<string> IndexFiles<TContainer, TDocument>(string filePath, string indexName, string typeName)
            where TContainer : IContainer<TDocument>
            where TDocument : class
        {
            Console.WriteLine("Indexing {0}...", filePath);
            using (var fileStream = File.OpenRead(filePath))
            {
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("multipart/form-data"));
                var json = await ParseTextFromStream<TContainer, TDocument>(fileStream);
                //Console.WriteLine(JsonConvert.SerializeObject(json));
                var indexParams = new IndexParams
                {
                    IndexName = indexName,
                    JsonString = JsonConvert.SerializeObject(json),
                    ModelType = typeName
                };
                
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(_httpClient.BaseAddress + "api/index"),
                    Content = new StringContent(JsonConvert.SerializeObject(indexParams), Encoding.UTF8, MediaTypeNames.Application.Json)
                };
                Console.WriteLine("Sending indexing request to the server...");
                var response = await _httpClient.SendAsync(request);
                return response.ReasonPhrase;
            }

        }

        private static async Task<List<TDocument>> ParseTextFromStream<TContainer, TDocument>(Stream streamContent)
            where TContainer : IContainer<TDocument>
        {
            Console.WriteLine("Parsing the file...");
            
            var items = new List<TDocument>();
            var serializer = new JsonSerializer();
            using (var stringReader = new StreamReader(streamContent))
            using (var jsonReader = new JsonTextReader(stringReader))
            {
                for (; await jsonReader.ReadAsync();)
                {
                    if (jsonReader.TokenType != JsonToken.StartObject)
                    {
                        continue;
                    }
                    var container = serializer.Deserialize<TContainer>(jsonReader);
                    if (container == null)
                    {
                        continue;
                    }
                    items.Add(container.GetInnerComponent());
                }

            }

            return items;
        }
    }
}