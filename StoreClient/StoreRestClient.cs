using StoceClient.DatabaseModels;
using StoreClient.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace StoreClient
{
    public class StoreRestClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private SessionCredentials _sessionCredentials;

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
            var response = await _httpClient.GetStringAsync(_baseUrl + "/order");
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var orderList = JsonSerializer.Deserialize<List<ORDER>>(response, options);
            return orderList;
        }
        public async bool CreateSession(string username, string password)
        {
            var message = new HttpRequestMessage(HttpMethod.Post, _baseUrl + "/session");
            message.Headers.Add("username", username);
            message.Headers.Add("password", password);
            var response = await _httpClient.SendAsync(message);
            var statusCode = response.StatusCode;
            if (statusCode == HttpStatusCode.NotFound)
                throw new NotImplementedException();
            else if (statusCode == HttpStatusCode.BadRequest)
                return false;
            else if (statusCode == HttpStatusCode.OK)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                _sessionCredentials = JsonSerializer.Deserialize<SessionCredentials>(response.ToString(), options);
                return true;
            }
        }
    }
}
