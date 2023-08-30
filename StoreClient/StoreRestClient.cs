using StoceClient.DatabaseModels;
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
        #region private vars
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private SessionCredentials _sessionCredentials;
        #endregion
        #region contructors
        public StoreRestClient(string baseUrl)
        {
            _baseUrl = baseUrl;
            #if (DEBUG)
            _baseUrl = "http://127.0.0.1:5000";
            #endif
            _httpClient = new HttpClient();
        }
        #endregion
        #region session
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
            catch (InvalidOperationException ex)
            {
                MessageBox.Show("Serwer nie odpowiada, skontaktuj się z właścicielem.", "Błąd połączenia z serwerem", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }
            catch (Exception ex)
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
        #endregion
        #region get item
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
        #endregion
        #region save item
        public bool SaveStorageItem(ref STORAGE item)
        {
            var httpMessage = new HttpRequestMessage(HttpMethod.Put, _baseUrl + "/storage");
            var content = new StringContent(JsonConvert.SerializeObject(item));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            httpMessage.Content = content;
            var response = _httpClient.SendAsync(httpMessage).Result;
            if (response.StatusCode == HttpStatusCode.Created)
            {
                item = JsonConvert.DeserializeObject<STORAGE>(response.Content.ReadAsStringAsync().Result);
                return true;
            }
            return false;
        }

        internal bool SaveOrder(ORDER item)
        {
            var httpMessage = new HttpRequestMessage(HttpMethod.Put, _baseUrl + "/order");
            var content = new StringContent(JsonConvert.SerializeObject(item));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            httpMessage.Content = content;
            var response = _httpClient.SendAsync(httpMessage).Result;
            if (response.StatusCode == HttpStatusCode.Created) return true;
            return false;
        }

        public bool SaveOrderDetailsItem(ref ORDERDETAILS item)
        {
            var httpMessage = new HttpRequestMessage(HttpMethod.Put, _baseUrl + "/order/details");
            var content = new StringContent(JsonConvert.SerializeObject(item));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            httpMessage.Content = content;
            var response = _httpClient.SendAsync(httpMessage).Result;
            if (response.StatusCode == HttpStatusCode.Created)
            {
                item = JsonConvert.DeserializeObject<ORDERDETAILS>(response.Content.ReadAsStringAsync().Result);
                return true;
            }
            return false;
        }
        #endregion
        #region delete item
        internal bool DeleteStorageItem(STORAGE item)
        {
            var httpMessage = new HttpRequestMessage(HttpMethod.Delete, _baseUrl + "/storage");
            var content = new StringContent(JsonConvert.SerializeObject(item));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            httpMessage.Content = content;
            var response = _httpClient.SendAsync(httpMessage).Result;
            if (response.StatusCode == HttpStatusCode.OK) return true;
            return false;
        }
        internal bool DeleteOrderDetailsItem(ORDERDETAILS item)
        {
            var httpMessage = new HttpRequestMessage(HttpMethod.Delete, _baseUrl + "/order/details");
            var content = new StringContent(JsonConvert.SerializeObject(item));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            httpMessage.Content = content;
            var respone = _httpClient.SendAsync(httpMessage).Result;
            if (respone.StatusCode == HttpStatusCode.OK) return true;
            return false;
        }
        #endregion
    }
}
