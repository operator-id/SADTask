using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
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
                Console.WriteLine(text);
                return JsonConvert.DeserializeObject<List<RealEstateBase>>(text);
            }

            return null;
        }
    }
}