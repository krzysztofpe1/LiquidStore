﻿using StoceClient.DatabaseModels;
using StoreClient.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Text.Json;
using System.Net.Http.Headers;
using Newtonsoft.Json;

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
            var storageList = System.Text.Json.JsonSerializer.Deserialize<List<STORAGE>>(response, options);
            return storageList;
        }

        public async Task<List<ORDER>> GetOrders()
        {
            var response = await _httpClient.GetStringAsync(_baseUrl + "/order");
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var orderList = System.Text.Json.JsonSerializer.Deserialize<List<ORDER>>(response, options);
            return orderList;
        }
        
        public async Task<bool> CreateSession(string username, string password)
        {
            var message = new HttpRequestMessage(HttpMethod.Post, _baseUrl + "/session");
            message.Headers.Add("username", username);
            message.Headers.Add("password", BCrypt.Net.BCrypt.HashPassword(password));
            HttpResponseMessage response;
            try
            {
                response = await _httpClient.SendAsync(message);
            }
            catch(InvalidOperationException ex)
            {
                MessageBox.Show("Serwer nie odpowiada, skontaktuj się z właścicielem.", "Błąd połączenia z serwerem", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd połączenia z serwerem", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }
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
                _sessionCredentials = System.Text.Json.JsonSerializer.Deserialize<SessionCredentials>(await response.Content.ReadAsStringAsync(), options);
                _sessionCredentials.Username = username;
                return true;
            }
            return false;
        }

        public bool SaveStorageItem(ref STORAGE item)
        {
            if (item.Id == null) item.Id = 0;
            //item.PopulateEmptyFields();
            var httpMessage = new HttpRequestMessage(HttpMethod.Put, _baseUrl + "/storage");
            var content = new StringContent(JsonConvert.SerializeObject(item));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            httpMessage.Content = content;
            var response = _httpClient.SendAsync(httpMessage).Result;
            item = JsonConvert.DeserializeObject<STORAGE>(response.Content.ReadAsStringAsync().Result);
            if (response.StatusCode == HttpStatusCode.Created) return true;
            return false;
        }
    }
}
