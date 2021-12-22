using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SearchDesktopApp.Models;

namespace SearchDesktopApp
{
    public class ApiClient
    {
        private readonly HttpClient _client;

        public ApiClient(string baseUrl)
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri(baseUrl + "/api")
            };
        }

        public async Task<List<RealEstateBase>> Search(string searchPhrase, string market = null)
        {
            var address = _client.BaseAddress + "/search";
            var searchParams = new SearchParams(searchPhrase, market, 2);
            
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(address),
                Content = new StringContent(JsonConvert.SerializeObject(searchParams), Encoding.UTF8, MediaTypeNames.Application.Json)
            };
    
            var response = await _client.SendAsync(request);
            Console.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                var text = await response.Content.ReadAsStringAsync();
                
                return await ParseText(text);
            }

            return null;
        }

        private static async Task<List<RealEstateBase>> ParseText(string text)
        {
            var items = new List<RealEstateBase>();
            using (var stringReader = new StringReader(text))
            using (var jsonReader = new JsonTextReader(stringReader))
            {
                for (; await jsonReader.ReadAsync();)
                {
                    if (jsonReader.TokenType != JsonToken.StartObject)
                    {
                        continue;
                    }

                    var jObject = await JObject.LoadAsync(jsonReader);
                    if (jObject["typeName"] == null)
                    {
                        throw new ArgumentNullException();
                    }
                    var type = TypeHelper.GetTypeFromName(jObject["typeName"].ToString());
                    var item = jObject.ToObject(type);
                    if (item == null)
                    {
                        continue;
                    }
                    items.Add(item as RealEstateBase);
                }

            }

            return items;
        }
    }
}