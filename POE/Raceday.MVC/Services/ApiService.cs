using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace RaceDay.MVC.Services
{
    public class ApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private HttpClient GetClient() => _httpClientFactory.CreateClient("RaceDayApi");

        // Generic GET
        public async Task<T?> GetAsync<T>(string endpoint)
        {
            var client = GetClient();
            var response = await client.GetAsync(endpoint);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            return default;
        }

        // Generic POST
        public async Task<HttpResponseMessage> PostAsync<T>(string endpoint, T data)
        {
            var client = GetClient();
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return await client.PostAsync(endpoint, content);
        }

        // Generic PUT
        public async Task<HttpResponseMessage> PutAsync<T>(string endpoint, T data)
        {
            var client = GetClient();
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return await client.PutAsync(endpoint, content);
        }

        // Generic DELETE
        public async Task<HttpResponseMessage> DeleteAsync(string endpoint)
        {
            var client = GetClient();
            return await client.DeleteAsync(endpoint);
        }

        // POST for Multipart (For File Uploads later)
        public async Task<HttpResponseMessage> PostFormAsync(string endpoint, MultipartFormDataContent content)
        {
            var client = GetClient();
            return await client.PostAsync(endpoint, content);
        }
    }
}