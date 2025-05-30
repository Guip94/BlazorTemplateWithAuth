using BlazorApp2.AuthSystems.Infrastructure.Interfaces;
using System.Net.Http.Json;
using System.Text.Json;

namespace BlazorApp2.AuthSystems.Infrastructure
{
    public class CustomHttpClient : ICustomHttpClient
    {
        private readonly HttpClient _httpClient;

        public CustomHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<T> GetAsync<T>(string uri)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(content);
        }

        public async Task<T> PostAsync<T>(string uri, object data)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(uri, data);
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(content);
        }
    }
}

