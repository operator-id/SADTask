using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
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
            System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => true;
            _client = new HttpClient
            {
                BaseAddress = new Uri(baseUrl + "/api")
            };
        }

        public async Task<List<PropertyModel>> SearchProperties(string searchPhrase, string market = null)
        {
            var address = _client.BaseAddress + "/search/" + searchPhrase;
            if (!string.IsNullOrWhiteSpace(market))
            {
                address += "/" + market;
            }
            var response = await _client.GetAsync(address);
            Console.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                var text = await response.Content.ReadAsStringAsync();
                Console.WriteLine(text);
                return JsonConvert.DeserializeObject<List<PropertyModel>>(text);
            }

            return null;
        }
    }
}