using StoceClient.DatabaseModels;
using StoreClient.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace StoreClient
{
    public class StoreRestClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public StoreRestClient(string baseUrl)
        {
            _baseUrl = baseUrl;
            _httpClient = new HttpClient();
        }

        public async Task<List<STORAGE>> GetStorage()
        {
            var response = await _httpClient.GetStringAsync(_baseUrl + "/storage");
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var storageList = JsonSerializer.Deserialize<List<STORAGE>>(response, options);
            return storageList;
        }

        public async Task<List<ORDER>> GetOrders()
        {
            var response = await _httpClient.GetStringAsync(_baseUrl + "/orders");
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var orderList = JsonSerializer.Deserialize<List<ORDER>>(response, options);
            return orderList;
        }
    }
}
